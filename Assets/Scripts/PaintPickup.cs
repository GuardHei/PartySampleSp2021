using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPickup : MonoBehaviour {
    public int paintValue;
    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {

            Debug.Log("Picked up Paint");

            col.gameObject.GetComponent<PaintResource>().AddPaint(paintValue);
            Destroy(gameObject);
        }
    }
}
