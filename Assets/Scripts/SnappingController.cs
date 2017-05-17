using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappingController : MonoBehaviour
{
    private const float snapDist = 0.05f; 
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
        List<GameObject> ownSnapPoints = new List<GameObject>();
        findAllSnapPoints(ownSnapPoints, snapObject.transform);
        foreach (GameObject otherObject in snapObjects)
        {
            if(otherObject != snapObject)
            {
                List<GameObject> otherSnapPoints = new List<GameObject>();
                findAllSnapPoints(otherSnapPoints, otherObject.transform);
                foreach(GameObject ownSnapPoint in ownSnapPoints)
                {
                    foreach(GameObject otherSnapPoint in otherSnapPoints)
                    {
                        if(Vector3.Distance(ownSnapPoint.transform.position, otherSnapPoint.transform.position) < snapDist)
                        {
                            Quaternion qalign = Quaternion.FromToRotation(ownSnapPoint.transform.forward, -otherSnapPoint.transform.forward);
                            snapObject.transform.rotation = qalign * snapObject.transform.rotation;
                            Vector3 align = otherSnapPoint.transform.position - ownSnapPoint.transform.position; // + (otherSnapPoint.transform.forward * 0.1f);
                            //align = snapObject.transform.InverseTransformVector(align);
                       
                            snapObject.transform.Translate(align, Space.World);
                            AddFixedJoint(snapObject, otherObject);
                        }
                    }
                }
            }
        }
    }

    private void AddFixedJoint(GameObject obj1, GameObject obj2)
    {
        FixedJoint fx = obj1.AddComponent<FixedJoint>();
        fx.breakForce = 1000;
        fx.breakTorque = 1000;
        fx.connectedBody = obj2.GetComponent<Rigidbody>();
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
