using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Dialogue dialogue;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerDialogue();
        }
    }

    private void TriggerDialogue()
    {
    Debug.Log("Triggering dialogue...");
    if (dialogue != null)
        {
        FindObjectOfType<Lvl1DialogueManager>().StartDialogue(dialogue);
        }
    else
        {
        Debug.LogError("Dialogue object is null.");
        }
    }
}
