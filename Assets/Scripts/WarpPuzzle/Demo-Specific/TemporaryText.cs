using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TemporaryText : MonoBehaviour
{

    public bool enabled;
    public string text = "Tutorial Text";
    public Image backPanel;
    public Text tutorialText;

    public UnityEvent onEnter;
    public UnityEvent onExit;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enabled)
        {
            backPanel.enabled = true;
            tutorialText.text = text;
            onEnter.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enabled)
        {
            backPanel.enabled = false;
            tutorialText.text = "";
            onExit.Invoke();
        }
    }
}
