using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LvlDialogueManager : MonoBehaviour
{
    public static LvlDialogueManager Instance;

    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;
    public string sceneName;

    private Queue<DialogueLine> lines = new Queue<DialogueLine>();
    public bool isDialogueActive = false;

    private bool isTyping = false;
    private DialogueLine currentLine;

    //[SerializableField]
    public float typingSpeed = 0.2f;

    

    public void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Update()
    {
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            if (!isTyping)
            {
                DisplayNextDialogueLine();
            }
            else
            {
                // Finish current sentence
                StopAllCoroutines();
                dialogueArea.text = currentLine.line;
                isTyping = false;
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
    if (isDialogueActive)
        {
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

    public void DisplayNextDialogueLine()
    {
        Debug.Log("Dialogue Lines Left: " + lines.Count);
        if (lines.Count == 0)
        {
            Debug.Log("Dialogue Finished");
            EndDialogue();
            return;
        }

        currentLine = lines.Dequeue();

        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;

        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        isTyping = true;
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        StartCoroutine(EndDialogueCoroutine());
    }

    IEnumerator EndDialogueCoroutine()
    {
        // Wait for a short duration after the text has finished typing
        yield return new WaitForSeconds(0.1f); // Adjust the duration as needed

        // Switch to the new scene
        SceneManager.LoadScene(sceneName);
        isDialogueActive = false;
    }
}
