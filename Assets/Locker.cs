using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour
{
    public KeyCode keyToPress = KeyCode.E;
    public GameObject other;
    private bool lockerbool;
    private bool inLocker;

    // Start is called before the first frame update
    void Start()
    {
        lockerbool = false;
        inLocker = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress) && lockerbool && !inLocker)
        {
            other.GetComponentInChildren<SpriteRenderer>().enabled = false;
            other.GetComponent<PlayerController>().inControl = false;
            other.GetComponent<Collider2D>().enabled = false;
            inLocker = true;
        }

        else if (Input.GetKeyDown(keyToPress) && inLocker)
        {
            other.GetComponentInChildren<SpriteRenderer>().enabled = true;
            other.GetComponent<PlayerController>().inControl = true;
            other.GetComponent<Collider2D>().enabled = true;
            inLocker = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            other = collision.gameObject;
            lockerbool = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //other = null;
            lockerbool = false;
        }
    }
}