using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueMonoBehaviour : MonoBehaviour
{
    public DialogueSettings settings;
    public Text speaker;
    public Text text;
    public Image image;
    public Button button;
    public AudioSource audioSource;
    private float currTime;
    private int lastTextLength;

    public void Initialize()
    {
        currTime = 0;
        lastTextLength = 0;
        settings.setSpeaker(speaker);
        settings.setImage(image);
        settings.setText(text, currTime);
        Time.timeScale = 0.0f;
        settings.onOpen.Invoke();
    }
    
    protected void Update()
    {
        currTime += Time.unscaledDeltaTime;
        int currTextLength = settings.setText(text, currTime);
        if (currTextLength > lastTextLength && settings.letterSound && text.text[currTextLength - 1] != ' ')
        {
            audioSource.clip = settings.letterSound;
            audioSource.Play();
            lastTextLength = currTextLength;
        }
    }

    public void Close()
    {
        print("Close()");
        if (currTime < settings.textDisplayTime) // complete text message
        {
            currTime = settings.textDisplayTime;
        }
        else
        {
            Time.timeScale = 1.0f; // (would need to cache old value if we ever try to mutate this elsewhere)
            settings.onClose.Invoke();
            print("Settings.nextDialogue is null: " + (settings.nextDialogue == null));
            if (settings.nextDialogue) DialogueManager.Instance.Open(settings.nextDialogue);
            GameObject o = gameObject;
            o.SetActive(false); // unsure if needed (I know destroy can be delayed, but it might already do this)
            Destroy(o);
        }
    }
}


