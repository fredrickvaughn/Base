using UnityEngine;
using System.Collections;
using MassiveNet;
using System.Collections.Generic;
using System.Net;

public class GameManager : MonoBehaviour {

    public enum netMode
    {
        server,
        client,
        none
    }

    public netMode mode = netMode.none;

    private const int xSize = 2;
    private const int ySize = 2;
    public const int zoneSize = 1000;
    public ZoneData[,] zones = new ZoneData[xSize, ySize];
    public GameObject player;
    private GameObject netManager;
    



    // Used for running in server mode
    public string ServerAddress = "127.0.0.1";
    public int ServerPortRoot = 17000;
    public List<string> PeerAddresses = new List<string>();
    public int AiCount = 512;
    private NetSocket socket;
    private NetViewManager viewManager;
    private NetZoneServer zoneServer;
    private NetZoneManager zoneManager;
    public Camera serverCamera;

    // Use this for initialization
    void Start () {
        for ( int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                zones[i, j] = new ZoneData(new Vector2(i, j));
                zones[i, j].Load();
            }
    
        }
    }

    void OnGUI()
    {
        if (mode == netMode.client)
        {
            if (GUI.Button(new Rect(10, 10, 50, 50), "Client")) { }
        }
        else if (mode == netMode.server)
        {
            if (GUI.Button(new Rect(10, 10, 50, 50), "Server")) { }
            if (GUI.Button(new Rect(10, 70, 50, 50), "Peers"))
            {
                zoneManager.ListPeers();
            }
            if (GUI.Button(new Rect(10, 140, 50, 50), "UPeers"))
            {
                zoneManager.ListUPeers();
            }
        }
        else
        {
            if (GUI.Button(new Rect(10, 10, 50, 50), "Server"))
            {
                mode = netMode.server;
//                netManager = (GameObject)GameObject.Instantiate(Resources.Load("ServerGameManager"));
                gameObject.AddComponent<NetSocket>();
                socket = GetComponent<NetSocket>();

                gameObject.AddComponent<NetViewManager>();
                viewManager = GetComponent<NetViewManager>();

                gameObject.AddComponent<NetZoneManager>();
                zoneManager = GetComponent<NetZoneManager>();

                gameObject.AddComponent<NetZoneServer>();
                zoneServer = GetComponent<NetZoneServer>();
                zoneServer.OnAssignment += AssignedToZone;

                gameObject.AddComponent<NetScopeManager>();

                socket.ProtocolAuthority = true;
                socket.AcceptConnections = true;
                socket.MaxConnections = 512;

                socket.Events.OnClientDisconnected += ClientDisconnected;
                socket.Events.OnPeerApproval += PeerApproval;
                socket.Events.OnSocketStart += SocketStart;
                socket.Events.OnFailedToConnect += FailedToConnect;

                socket.StartSocket(ServerAddress + ":" + ServerPortRoot);
                socket.RegisterRpcListener(this);
                serverCamera.enabled = true;
            }
            if (GUI.Button(new Rect(10, 70, 50, 30), "Client"))
            {
                mode = netMode.client;
                netManager = (GameObject)GameObject.Instantiate(Resources.Load("ClientGameManager"));
            }
        }
    }

    public void LoadZone(Vector2 newZoneLoc)
    {
        if (newZoneLoc.x >= 0 && newZoneLoc.x < xSize && newZoneLoc.y >= 0 && newZoneLoc.y < ySize )
        {
            zones[(int)newZoneLoc.x, (int)newZoneLoc.y].Load();
        }
    }

    public void LoadZone(int x, int y)
    {
        if (x >= 0 && x < xSize && y >= 0 && y < ySize)
        {
            zones[x, y].Load();
        }
    }

    public void UnloadZone(int x, int y)
    {
        if (x >= 0 && x < xSize && y >= 0 && y < ySize)
        {
            zones[x, y].Unload();
        }
    }

    public void LoadStartZone(Vector3 location)
    {
        // Find the zone the player will spawn into and load it.
        int tempX = Mathf.RoundToInt((location.x / zoneSize) - ((location.x % zoneSize) / zoneSize));
        int tempY = Mathf.RoundToInt((location.z / zoneSize) - ((location.z % zoneSize) / zoneSize));
        zones[tempX, tempY].Load();
        // Spawn the Player and move them to their spawn location.
//        player = (GameObject)GameObject.Instantiate(Resources.Load("Player"));
//        player.transform.position = location;
    }

    // Update is called once per frame
    void Update () {
        if (!player)
        {
            //Debug.Log("No Player");
            if (player = GameObject.Find("ISOPlayer@Owner(Clone)")) 
            {
                Debug.Log("Player found at " + player.transform.position);
                //LoadStartZone(player.transform.position);
            }
        }
	
	}

    //Server Method
    private void SocketStart()
    {
        if (socket.Port != ServerPortRoot)
        {
            // If another server is on the same machine, connect to it:
            socket.ConnectToPeer(socket.Address + ":" + ServerPortRoot);
        }
        else if (PeerAddresses.Count > 0)
        {
            // Else, if there are peer addresses defined in PeerAddresses, connect:
            socket.ConnectToPeer(PeerAddresses[0]);
        }
        else
        {
            ConfigureZones();
        }
    }


    //Server Method
    private void ConfigureZones()
    {
        zoneManager.Authority = true;
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                zoneManager.CreateZone(new Vector3(i * zoneSize, 0, j * zoneSize));
            }

        }
        zoneManager.AddSelfAsServer();
    }

    //Server Method
    private void FailedToConnect(IPEndPoint endpoint)
    {
        string epString = endpoint.ToString();
        if (PeerAddresses.Contains(epString))
        {
            int index = PeerAddresses.IndexOf(epString);
            if (index + 1 == PeerAddresses.Count) return;
            index++;
            socket.ConnectToPeer(PeerAddresses[index]);
        }
        else if (socket.Address == endpoint.Address.ToString() && socket.Port - endpoint.Port > 1)
        {
            if (endpoint.Port == ServerPortRoot) return;
            socket.ConnectToPeer(ServerAddress + ":" + (endpoint.Port - 1));
        }
        else Debug.LogError("Failed to connect to peer(s).");
    }

    //Server Method
    private void SpawnPlayer(NetConnection connection)
    {
        viewManager.CreateView(connection, 0, "ISOPlayer");
    }

    //Server Method
    private void ClientDisconnected(NetConnection connection)
    {
        viewManager.DestroyAuthorizedViews(connection);
    }

    //Server Method
    [NetRPC]
    private void SpawnRequest(NetConnection connection)
    {
        SpawnPlayer(connection);
    }

    //Server Method
    private bool PeerApproval(IPEndPoint endPoint, NetStream data)
    {
        if (endPoint.Port > ServerPortRoot + 512 || endPoint.Port < ServerPortRoot) return false;
        string address = endPoint.Address.ToString();
        return (address == ServerAddress || PeerAddresses.Contains(address));
    }

    //Server Method
    private void AssignedToZone()
    {
        CreateAi(zoneServer.Position);
        serverCamera.transform.position = zoneServer.Position + new Vector3(zoneSize / 2, 350, zoneSize / 2);
    }

    //Server Method
    private void CreateAi(Vector3 origin)
    {
        for (int i = AiCount; i > 0; i--)
        {
            var ai = viewManager.CreateView(0, "SimpleAI");
            ai.transform.position = new Vector3 (origin.x + (zoneSize/2), 2, origin.z + (zoneSize/2));
            Debug.Log("AI being created at:" + ai.transform.position);
            ai.GetComponent<AiCreator>().SetTargetRoot(new Vector3(origin.x, 2, origin.z));
        }
    }

}
