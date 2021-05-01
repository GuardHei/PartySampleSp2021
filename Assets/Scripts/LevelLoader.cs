using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    public AudioClip sfx;
    public string destinationSceneName;
    public Color inRangeColor = Color.green;
    public KeyCode
        keyToPress = KeyCode.Space; // could also change this to have it retrieve a keycode from playerController

    public bool letPlayerThrough = true; // set to false if you are waiting for an event to finish first
    private bool inRange = false;
    private Color originalColor;
    private SpriteRenderer renderer;

    /**
     * Get the key from Player Controller.
     */
    void Start()
    {
        // keyToPress = GetComponent<PlayerController>().warpExteriorKey;
    }

    void Awake() {
        renderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = renderer.color;
    }

    /**
     * If the player presses the transition key, check that they're in range. If so, transition the scene.
     * Only happens if we are letting the player through the door.
     */
    void Update()
    {
        if (letPlayerThrough)
        {
            if ((Input.GetKeyDown(keyToPress) && inRange) || Input.GetKeyDown(KeyCode.Backspace))
            {
                Debug.Log("Scene transition to: " + destinationSceneName);
                AudioManager.PlayAtPoint(sfx, transform.position);
                SceneManager.LoadScene(destinationSceneName); // change scene
            }
        }
    }

    /**
     * If the player collides with this object, they're in range.
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player in range of door.");
            inRange = true;
            renderer.color = inRangeColor;
        }
    }

    /**
     * When the player is no longer colliding with this object, they're not in range.
     */
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left door range.");
            inRange = false;
            renderer.color = originalColor;
        }
    }
}