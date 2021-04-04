using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueSettings : MonoBehaviour
{
    public string speaker;
    [TextArea(6, 10)] public string text;
    public AudioClip letterSound;
    public Sprite sprite;
    [Range(0.0f, 10.0f)] public float textDisplayTime;
    public UnityEvent onOpen;
    public UnityEvent onClose;
    public DialogueSettings nextDialogue;
    
    public void setSpeaker(Text speaker)
    {
        speaker.text = this.speaker;
        speaker.enabled = true;
    }
    
    public int setText(Text text, float currTime)
    {
        int endIndex = textDisplayTime == 0 ? this.text.Length : 
            (int) Math.Min(this.text.Length * (currTime / textDisplayTime), this.text.Length);
        string currString = this.text.Substring(0, endIndex);
        text.text = currString;
        text.enabled = true;
        return currString.Length;
    }
    
    public void setImage(Image image)
    {
        if (sprite != null)
        {
            image.sprite = sprite;
            image.enabled = true;
        }
    }
}