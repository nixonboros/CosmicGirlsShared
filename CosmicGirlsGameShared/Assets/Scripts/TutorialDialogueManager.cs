using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;
    public string[] dialogues; // Array to hold your dialogues
    private int currentDialogueIndex;
    private int dialogueClickCount;
    private bool tutorialCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        currentDialogueIndex = 0;
        dialogueClickCount = 0;
        UpdateDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Level0Tutorial")
        {
            if (!tutorialCompleted && TutorialGameManager.instance.normalHits + TutorialGameManager.instance.goodHits + TutorialGameManager.instance.perfectHits >= 8)
            {
                tutorialCompleted = true;
            }

            if (tutorialCompleted && Input.GetMouseButtonDown(0))
            {
                dialogueClickCount++;
                if (dialogueClickCount < dialogues.Length)
                {
                    currentDialogueIndex++;
                    UpdateDialogue();
                }
                else
                {
                    SceneManager.LoadScene("Level0");
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "Level0")
        {
            if (!TutorialGameManager.instance.music.isPlaying && TutorialGameManager.instance.gameFinished && Input.GetMouseButtonDown(0))
            {
                dialogueClickCount++;
                if (dialogueClickCount < dialogues.Length)
                {
                    currentDialogueIndex++;
                    UpdateDialogue();
                }
                else
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                dialogueClickCount++;
                if (dialogueClickCount < dialogues.Length)
                {
                    currentDialogueIndex++;
                    UpdateDialogue();
                }
                else
                {
                    SceneManager.LoadScene("Level0Tutorial");
                }
            }
        }
    }


    void UpdateDialogue()
    {
        dialogueText.text = dialogues[currentDialogueIndex];
    }

    // Reset the dialogue to start again
    public void ResetDialogue()
    {
        currentDialogueIndex = 0;
        dialogueClickCount = 0;
        tutorialCompleted = false;
        UpdateDialogue();
    }
}
