using UnityEngine;
using System.Collections;

public class ZoneData : Object {

    public Vector2 gridLoc;
    public string zonePrefab;
    public GameObject gameObj;
    private bool loaded = false;


    public ZoneData(Vector2 newLoc)
    {
        gridLoc = newLoc;
        zonePrefab = "Zone" + gridLoc.x.ToString() + "x" + gridLoc.y.ToString();
    }

	public void Load ()
    {
        if (!loaded)
        {
            gameObj = (GameObject)GameObject.Instantiate(Resources.Load(zonePrefab));
            gameObj.transform.position = new Vector3(gridLoc.x * 1000, 0, gridLoc.y * 1000);
            gameObj.transform.Find("ZoneTriggerTopLeft").gameObject.GetComponent<ZoneTrigger>().SetLoc(gridLoc);
            gameObj.transform.Find("ZoneTriggerTopRight").gameObject.GetComponent<ZoneTrigger>().SetLoc(gridLoc);
            gameObj.transform.Find("ZoneTriggerBottomLeft").gameObject.GetComponent<ZoneTrigger>().SetLoc(gridLoc);
            gameObj.transform.Find("ZoneTriggerBottomRight").gameObject.GetComponent<ZoneTrigger>().SetLoc(gridLoc);
            loaded = true;
        }
    }

    public void Unload()
    {
        loaded = false;
        GameObject.Destroy(gameObj);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
