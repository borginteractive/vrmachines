using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappingController : MonoBehaviour
{
    List<GameObject> snapObjects;

	// Use this for initialization
	void Start ()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("SnapObject");
        snapObjects = new List<GameObject>(objects);
        foreach(GameObject g in snapObjects)
        {
            print(g.name);
        }
	}

    public void doSnap(GameObject snapObject)
    {
        List<GameObject> snapPoints = new List<GameObject>();
        findAllSnapPoints(snapPoints, snapObject.transform);
        foreach (GameObject snapPoint in snapPoints)
        {
            Transform t = snapPoint.transform;
            t.localPosition = new Vector3(0, 0.002f, 0);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void findAllSnapPoints(List<GameObject> result, Transform parent)
    {
        for(int i = 0; i < parent.childCount; ++i)
        {
            Transform child = parent.GetChild(i);
            if (child.tag.Equals("SnapPoint"))
            {
                result.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                findAllSnapPoints(result, child);
            }
        }
    }
}
