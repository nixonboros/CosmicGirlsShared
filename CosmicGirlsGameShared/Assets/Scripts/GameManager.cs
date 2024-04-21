using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioSource music;
    public AudioSource missSound;
    public AudioSource hitSound;

    public bool startPlaying;

    public BeatScroller beatScroller;

    public static GameManager instance;

    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;

    public int comboCounter; // Tracks the current combo
    public int maxCombo; // Tracks the maximum combo achieved

    public Text scoreText;
    public Text comboText;
    public Text startText; // Text to display "Press Space to Start"

    public float totalNotes;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;

    public GameObject resultsScreen;
    public Text percentHitText, normalsText, goodsText, perfectsText, missesText, rankText, finalScoreText;

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

        totalNotes = FindObjectsOfType<NoteObject>().Length; //total amount of notes

        // Display "Press Space to Start" text
        startText.gameObject.SetActive(true);

        music.time = beatScroller.skipDuration;
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

                music.Play();
            }
            else
            {
                if (!music.isPlaying && !resultsScreen.activeInHierarchy) //if results screen isnt up, and music is done
                {
                    resultsScreen.SetActive(true);
                    normalsText.text = "" + normalHits;
                    goodsText.text = goodHits.ToString(); //display value as string
                    perfectsText.text = perfectHits.ToString();
                    missesText.text = "" + missedHits;

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

        if (missSound != null)
        {
            missSound.Play();
        }

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
}
