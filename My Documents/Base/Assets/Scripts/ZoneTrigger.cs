using UnityEngine;
using System.Collections;

public class ZoneTrigger : MonoBehaviour {

    public enum direction
    {
        topRight,
        topLeft,
        bottomRight,
        bottomLeft
    };
    public Vector2 gridLoc;
    private GameManager gm;
    public direction triggerDirection;

    // Use this for initialization
    void Start () {
        GameObject gmGameObj = GameObject.Find("GameManager");
        gm = gmGameObj.GetComponent<GameManager>();
        enabled = true;
    }

    public void SetLoc(int x, int y)
    {
        gridLoc = new Vector2(x, y);
    }

    public void SetLoc(Vector2 newLoc)
    {
        gridLoc = newLoc;
    }

    void OnTriggerEnter(Collider other)
    {
        //Check to see if the GameManager object is assisgned. If it isn't assign it.
        if (!gm)
        {
            GameObject gmGameObj = GameObject.Find("GameManager");
            gm = gmGameObj.GetComponent<GameManager>();
        }
        if (other.CompareTag("MainPlayer"))
        {
            if (triggerDirection == direction.topLeft)
            {
                gm.LoadZone((int)gridLoc.x - 1, (int)gridLoc.y + 1);
                gm.LoadZone((int)gridLoc.x - 1, (int)gridLoc.y);
                gm.LoadZone((int)gridLoc.x, (int)gridLoc.y + 1);

                gm.UnloadZone((int)gridLoc.x + 1, (int)gridLoc.y - 1);
                gm.UnloadZone((int)gridLoc.x + 1, (int)gridLoc.y);
                gm.UnloadZone((int)gridLoc.x, (int)gridLoc.y - 1);
                gm.UnloadZone((int)gridLoc.x - 1, (int)gridLoc.y - 1);
                gm.UnloadZone((int)gridLoc.x + 1, (int)gridLoc.y + 1);
            }
            if (triggerDirection == direction.topRight)
            {
                gm.LoadZone((int)gridLoc.x + 1, (int)gridLoc.y + 1);
                gm.LoadZone((int)gridLoc.x + 1, (int)gridLoc.y);
                gm.LoadZone((int)gridLoc.x, (int)gridLoc.y + 1);

                gm.UnloadZone((int)gridLoc.x - 1, (int)gridLoc.y + 1);
                gm.UnloadZone((int)gridLoc.x - 1, (int)gridLoc.y);
                gm.UnloadZone((int)gridLoc.x - 1 , (int)gridLoc.y - 1);
                gm.UnloadZone((int)gridLoc.x, (int)gridLoc.y - 1);
                gm.UnloadZone((int)gridLoc.x + 1, (int)gridLoc.y -  1);
            }
            if (triggerDirection == direction.bottomLeft)
            {
                gm.LoadZone((int)gridLoc.x - 1, (int)gridLoc.y - 1);
                gm.LoadZone((int)gridLoc.x - 1, (int)gridLoc.y);
                gm.LoadZone((int)gridLoc.x, (int)gridLoc.y - 1);

                gm.UnloadZone((int)gridLoc.x + 1, (int)gridLoc.y - 1);
                gm.UnloadZone((int)gridLoc.x + 1, (int)gridLoc.y);
                gm.UnloadZone((int)gridLoc.x + 1, (int)gridLoc.y + 1);
                gm.UnloadZone((int)gridLoc.x, (int)gridLoc.y + 1);
                gm.UnloadZone((int)gridLoc.x - 1, (int)gridLoc.y + 1);
            }
            if (triggerDirection == direction.bottomRight)
            {
                gm.LoadZone((int)gridLoc.x + 1, (int)gridLoc.y - 1);
                gm.LoadZone((int)gridLoc.x + 1, (int)gridLoc.y);
                gm.LoadZone((int)gridLoc.x, (int)gridLoc.y - 1);

                gm.UnloadZone((int)gridLoc.x - 1, (int)gridLoc.y + 1);
                gm.UnloadZone((int)gridLoc.x - 1, (int)gridLoc.y);
                gm.UnloadZone((int)gridLoc.x - 1, (int)gridLoc.y - 1);
                gm.UnloadZone((int)gridLoc.x, (int)gridLoc.y + 1);
                gm.UnloadZone((int)gridLoc.x + 1, (int)gridLoc.y + 1);
            }
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
