using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public string destinationSceneName;
    public KeyCode transitionKey; // can make private when we are sure what this key will be
    public bool letPlayerThrough = true; // set to false if you are waiting for an event to finish first
    private bool inRange = false;

    /**
     * Get the key from Player Controller.
     */
    void Start()
    {
        transitionKey = GetComponent<PlayerController>().warpExteriorKey;
    }

    /**
     * If the player presses the transition key, check that they're in range. If so, transition the scene.
     * Only happens if we are letting the player through the door.
     */
    void Update()
    {
        if (letPlayerThrough)
        {
            if (Input.GetKeyDown(transitionKey) && inRange)
            {
                Debug.Log("Scene transition to: " + destinationSceneName);
                SceneManager.LoadScene(destinationSceneName); // change scene
            }
        }
    }

    /**
     * If the player collides with this object, they're in range.
     */
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            Debug.Log("Player in range of door.");
            inRange = true;
        }
    }

    /**
     * When the player is no longer colliding with this object, they're not in range.
     */
    private void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            Debug.Log("Player left door range.");
            inRange = false;
        }
    }
}