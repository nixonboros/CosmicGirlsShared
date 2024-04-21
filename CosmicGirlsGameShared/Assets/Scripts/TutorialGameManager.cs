using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialGameManager : MonoBehaviour
{
    public AudioSource hitSound;
    public AudioSource tutorialCompletedSound;
    public AudioSource BackgroundMusic;

    public bool startPlaying;

    public BeatScroller beatScroller;

    public static TutorialGameManager instance;

    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;

    public int comboCounter; // Tracks the current combo
    public int maxCombo; // Tracks the maximum combo achieved

    public Text scoreText;
    public Text comboText;
    public Text startText;
    public GameObject buttonCanvas;


    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;

    public GameObject dialogueCanvas;
    public DialogueManager dialogueManager;

    public bool gameStarted;
    public delegate void GameStartedAction();
    public static event GameStartedAction OnGameStarted;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        scoreText.gameObject.SetActive(false);
        comboText.gameObject.SetActive(false);
        comboCounter = 0;
        maxCombo = 0;

        dialogueCanvas.SetActive(false);

        startText.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        if (gameStarted)
        {
            if (!startPlaying)
            {
                startPlaying = true;
                beatScroller.hasStarted = true;
            }

            if (normalHits + goodHits + perfectHits >= 8)
            {
                ShowDialogueAgain();
            }
            if (missedHits > 0)
            {
                SceneManager.LoadScene("Level0");
            }
        }
    }

    void StartGame()
    {
        gameStarted = true;
        startText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        comboText.gameObject.SetActive(true);

        // Fire the event when the game starts
        OnGameStarted?.Invoke();
    }

    public void NoteHit()
    {
        IncrementCombo();

        currentScore += scorePerNote * comboCounter;
        UpdateUI();

        if (hitSound != null)
        {
            hitSound.Play();
        }
    }

    public void NormalHit()
    {
        currentScore += scorePerNote * comboCounter;
        NoteHit();

        normalHits++; //add one to amount of hits 

        if (hitSound != null)
        {
            hitSound.Play();
        }
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote * comboCounter;
        NoteHit();

        goodHits++;

        if (hitSound != null)
        {
            hitSound.Play();
        }
    }

    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote * comboCounter;
        NoteHit();

        perfectHits++;

        if (hitSound != null)
        {
            hitSound.Play();
        }
    }

    public void NoteMissed()
    {
        ResetCombo();

        missedHits++;
        UpdateUI();
    }

    // Increments the combo counter and updates maxCombo if necessary
    void IncrementCombo()
    {
        comboCounter++;
        if (comboCounter > maxCombo)
        {
            maxCombo = comboCounter;
        }
    }

    // Resets the combo counter
    void ResetCombo()
    {
        comboCounter = 0;
    }

    // Updates the UI elements
    void UpdateUI()
    {
        scoreText.text = "Score:" + currentScore;
        comboText.text = "" + comboCounter;
    }

    // Show the dialogue again after completing the tutorial
    void ShowDialogueAgain()
    {
        gameStarted = false;
        dialogueCanvas.SetActive(true);
        dialogueManager.ResetDialogue();

        scoreText.gameObject.SetActive(false);
        comboText.gameObject.SetActive(false);
        buttonCanvas.SetActive(false);

        if (tutorialCompletedSound != null)
        {
            tutorialCompletedSound.Play();
        }

        if (BackgroundMusic != null)
        {
            BackgroundMusic.Play();
        }
    }
}
