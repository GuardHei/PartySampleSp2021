using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPickup : MonoBehaviour {
    public int paintValue;
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            Debug.Log("Picked up Paint");
            col.gameObject.GetComponent<PaintResource>().AddPaint(paintValue);
            Destroy(gameObject);
        }
    }
}
