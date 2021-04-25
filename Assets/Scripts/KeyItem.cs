using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public string _color;

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag("Player"))
        {
            target.GetComponent<PlayerInventory>().addItem(_color + " Key");
            Destroy(gameObject);
        }
    }
}
