using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject other = collision.gameObject;

            /* Raycast check */
            GameObject parent = transform.parent.gameObject;
            Vector2 origin = parent.transform.position;
            Vector2 target = other.GetComponent<Transform>().position;
            Vector2 direction = target - origin;
            /* don't want to detect enemy layer (8) */
            int layerMask = 1 << 8;
            layerMask = ~layerMask;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, layerMask);
            Debug.DrawRay(origin, direction * 1000, Color.red);
            if (hit.collider.tag == "Player") {
                other.GetComponent<Health>().Hit(10000);
            }
        }
    }
}
