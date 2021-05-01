using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
    public AudioClip sfx;
    public GameObject teleportTo;
    public KeyCode keyToPress = KeyCode.E;
    private GameObject other;
    private Camera cam;
    private bool teleportbool;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        teleportbool = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress) && teleportbool)
        {
            if (sfx) AudioManager.PlayAtPoint(sfx, transform.position);
            Vector3 playernew = teleportTo.transform.position;
            playernew[2] = other.transform.position.z;
            Vector3 camnew = teleportTo.transform.position;
            camnew[2] = -10;
            other.transform.position = playernew;
            cam.transform.position = camnew;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            other = collider.gameObject;
            teleportbool = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            other = null;
            teleportbool = false;
        }
    }
}
