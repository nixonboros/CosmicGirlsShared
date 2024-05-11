using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Lvl1DialogueManager : MonoBehaviour
{
    public static Lvl1DialogueManager Instance;

    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;
    public string sceneName;

    private Queue<DialogueLine> lines = new Queue<DialogueLine>();
    public bool isDialogueActive = false;

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
            DisplayNextDialogueLine();
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

    private void DisplayNextDialogueLine()
    {
        Debug.Log(lines.Count);
        if (lines.Count == 0)
        {
            Debug.Log("No more dialogue lines. Ending dialogue.");
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
        SceneManager.LoadScene(sceneName);
    }
}
