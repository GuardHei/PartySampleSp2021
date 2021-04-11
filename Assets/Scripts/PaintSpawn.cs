using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintSpawn : MonoBehaviour {
    public GameObject paintObj;

    public void SpawnNew() {

        Debug.Log("New Paint spawned");

        Instantiate(paintObj, transform.position, Quaternion.identity);
    }
}
