using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Detection : MonoBehaviour {

    public UnityEvent onDetected;

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Player")) {
            GameObject other = collider.gameObject;
            /* Raycast check */
            GameObject parent = transform.parent.gameObject;
            Vector2 origin = parent.transform.position;
            Vector2 target = other.GetComponent<Transform>().position;
            Vector2 direction = target - origin;
            /* don't want to detect enemy layer (8) */
            int layerMask = 1 << LayerMask.NameToLayer("Enemy");
            layerMask = ~layerMask;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, layerMask);
            Debug.DrawRay(origin, direction * 1000, Color.red);
            if (hit.collider.CompareTag("Player")) {
                print("Detected");
                onDetected?.Invoke();
            }
        }
    }
}
