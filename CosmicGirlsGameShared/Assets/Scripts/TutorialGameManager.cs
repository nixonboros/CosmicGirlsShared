using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Diagnostics;

public class TutorialGameManager : MonoBehaviour
{
    public AudioSource music;
    public AudioSource hitSound;
    public AudioSource tutorialCompletedSound;
    public AudioSource BackgroundMusic;
    public VideoPlayer backgroundVideo;
    public Slider progressBar;

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
    public Image PlayfieldPanel;

    public float musicLength; // Length of the music clip

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
        progressBar.gameObject.SetActive(false);
        comboCounter = 0;
        maxCombo = 0;

        musicLength = music.clip.length;

        dialogueCanvas.SetActive(false);

        music.time = beatScroller.skipDuration;

        startText.gameObject.SetActive(true);

        progressBar.maxValue = musicLength;

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
            progressBar.value = music.time;
            if (!startPlaying)
            {
                startPlaying = true;
                beatScroller.hasStarted = true;

                music.Play();
            }
            if (SceneManager.GetActiveScene().name == "Level0Tutorial")
            {
                if (normalHits + goodHits + perfectHits >= 8)
                {
                    ShowDialogueAgain();
                }
                if (missedHits > 0)
                {
                    SceneManager.LoadScene("Level0Tutorial");
                }
            }
            else if (SceneManager.GetActiveScene().name == "Level0")
            {
                if (!music.isPlaying)
                {
                    ShowDialogueAgain();
                }
            }
        }
    }

    void StartGame()
    {
        gameStarted = true;
        startText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        comboText.gameObject.SetActive(true);
        progressBar.gameObject.SetActive(true);
        PlayfieldPanel.gameObject.SetActive(true);

        if (backgroundVideo != null)
        {
            backgroundVideo.Play();
        }

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
        dialogueCanvas.SetActive(true);
        dialogueManager.ResetDialogue();

        scoreText.gameObject.SetActive(false);
        comboText.gameObject.SetActive(false);
        buttonCanvas.SetActive(false);
        progressBar.gameObject.SetActive(false);
        backgroundVideo.gameObject.SetActive(false);
        PlayfieldPanel.gameObject.SetActive(false);

        if (tutorialCompletedSound != null)
        {
            tutorialCompletedSound.Play();
        }

        if (BackgroundMusic != null)
        {
            BackgroundMusic.Play();
        }

        gameStarted = false;
    }
}
