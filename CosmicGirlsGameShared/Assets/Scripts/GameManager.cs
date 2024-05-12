using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public AudioSource music;
    public AudioSource hitSound;
    public VideoPlayer backgroundVideo;
    public Slider progressBar;
    public AudioSource crossSound;
    public AudioSource chatboxSound;

    public bool startPlaying;

    public BeatScroller beatScroller;

    public static GameManager instance;

    public GameObject buttonCanvas;

    public int currentScore;
    public int scorePerNormalNote = 50;
    public int scorePerGoodNote = 100;
    public int scorePerPerfectNote = 150;
    public int scorePerChatbox = 14900;

    public int comboCounter; // Tracks the current combo
    public int maxCombo; // Tracks the maximum combo achieved

    public Text scoreText;
    public Text comboText;
    public Text startText; // Text to display "Press Space to Start"

    public float musicLength; // Length of the music clip

    public float totalNotes;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;
    public float chatboxHits;

    public GameObject resultsScreen;
    public Text percentHitText, normalsText, goodsText, perfectsText, missesText, rankText, finalScoreText, resultsComboText, chatboxText;

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

        totalNotes = FindObjectsOfType<NoteObject>().Length; //total amount of notes

        musicLength = music.clip.length;

        // Display "Press Space to Start" text
        startText.gameObject.SetActive(true);

        music.time = beatScroller.skipDuration;

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
            else
            {
                if (!music.isPlaying && !resultsScreen.activeInHierarchy) //if results screen isnt up, and music is done
                {
                    buttonCanvas.SetActive(false);

                    resultsScreen.SetActive(true);
                    normalsText.text = "" + normalHits;
                    goodsText.text = goodHits.ToString(); //display value as string
                    perfectsText.text = perfectHits.ToString();
                    missesText.text = "" + missedHits;
                    if (SceneManager.GetActiveScene().name == "Level4")
                    {
                        chatboxText.text = chatboxHits.ToString();
                    }

                    float totalHit = normalHits + goodHits + perfectHits;
                    float percentHit = (totalHit / totalNotes) * 100f;

                    percentHitText.text = percentHit.ToString("F1") + "%"; //1dp

                    //ranks
                    string rankVal = "F";
                    if (percentHit > 40)
                    {
                        rankVal = "D";
                        if (percentHit > 55)
                        {
                            rankVal = "C";
                            if (percentHit > 70)
                            {
                                rankVal = "B";
                                if (percentHit > 85)
                                {
                                    rankVal = "A";
                                    if (percentHit > 95)
                                    {
                                        rankVal = "S";
                                    }
                                }
                            }
                        }
                    }
                    rankText.text = rankVal;
                    finalScoreText.text = currentScore.ToString();
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

        UpdateUI();

        hitSound.Play();
    }

    public void NormalHit()
    {
        currentScore += scorePerNormalNote * (comboCounter + 1);
        NoteHit();
        normalHits++;
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote * (comboCounter + 1);
        NoteHit();
        goodHits++;
    }

    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote * (comboCounter + 1);
        NoteHit();
        perfectHits++;
    }

    public void ChatboxHit()
    {
        currentScore += scorePerChatbox;
        NoteHit();

        chatboxHits++;

        if (chatboxSound != null)
        {
            chatboxSound.Play();
        }
    }

    public void CrossHit()
    {
        currentScore -= scorePerChatbox;

        if (crossSound != null)
        {
            Debug.Log("BOOM");
            crossSound.Play();
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
            resultsComboText.text = maxCombo.ToString();
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

    public static implicit operator GameManager(TutorialGameManager v)
    {
        throw new NotImplementedException();
    }
}
