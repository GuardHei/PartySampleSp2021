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
            other.GetComponent<SpriteRenderer>().enabled = false;
            other.GetComponent<PlayerController>().inControl = false;
            other.GetComponent<BoxCollider2D>().enabled = false;
            inLocker = true;
        }

        else if (Input.GetKeyDown(keyToPress) && inLocker)
        {
            other.GetComponent<SpriteRenderer>().enabled = true;
            other.GetComponent<PlayerController>().inControl = true;
            other.GetComponent<BoxCollider2D>().enabled = true;
            inLocker = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            other = collision.gameObject;
            lockerbool = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //other = null;
            lockerbool = false;
        }
    }
}