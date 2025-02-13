using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class LvlDialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        LvlDialogueManager.Instance.StartDialogue(dialogue);
    }
}
