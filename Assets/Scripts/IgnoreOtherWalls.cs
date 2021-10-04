using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreOtherWalls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach(GameObject wall in walls)
        {
            Physics.IgnoreCollision(wall.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}
