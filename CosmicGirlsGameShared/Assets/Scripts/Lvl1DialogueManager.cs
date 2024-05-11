using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Lvl1DialogueManager : MonoBehaviour
{
    public static Lvl1DialogueManager Instance;

    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines = new Queue<DialogueLine>();
    public bool isDialogueActive = false;

    //[SerializableField]
    public float typingSpeed = 0.2f;

    

    private void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            DisplayNextDialogueLine();
        }
    }

    public void StartDialogue(Dialogue dialogue)
  {
    if (isDialogueActive)
        {
            Debug.LogWarning("Dialogue is already active. Cannot start a new dialogue.");
            return;
        }

    isDialogueActive = true;
    lines.Clear();

    foreach (DialogueLine dialogueLine in dialogue.dialogueLines) 
    {
        lines.Enqueue(dialogueLine);
    }
    DisplayNextDialogueLine();
  }

    private void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;

        //StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
    }
}
