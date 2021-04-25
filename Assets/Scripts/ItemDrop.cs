using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour {
    public List<GameObject> items = new List<GameObject>();

    public void DropAll() {
        if (items != null) {
            for (int i = 0; i < items.Count; i++) {
                Instantiate(items[i], transform.position, Quaternion.identity);
            }
        }
    }
}
