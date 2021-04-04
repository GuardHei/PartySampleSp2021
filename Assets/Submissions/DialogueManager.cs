using UnityEngine;

//[CreateAssetMenu(fileName = "DialogueManager")] // (uncomment if you ever need to remake this asset)
public class DialogueManager : ScriptableObject
{

    public GameObject dialoguePrefab;
    
    public static DialogueManager Instance;

    public void OnEnable()
    {
        Instance = this;
    }
    
    public void Open(DialogueSettings dialogue)
    {
        if (dialogue == null) return;
        GameObject newDialogue = Instantiate(dialoguePrefab);
        DialogueMonoBehaviour dialogueMonoBehaviour = newDialogue.GetComponentInChildren<DialogueMonoBehaviour>();
        dialogueMonoBehaviour.settings = dialogue;
        dialogueMonoBehaviour.Initialize();
    }
}