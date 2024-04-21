using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    public float beatTempo;
    public bool hasStarted;
    public float skipDuration = 0f; // Duration to skip into the song

    private GameManager gameManager;
    private float pixelsPerSecond;
    private float pixelsPerBeat;

    // Start is called before the first frame update
    void Start()
    {
        beatTempo = beatTempo / 15f;

        // Calculate y pixels per beat
        pixelsPerSecond = beatTempo;
        pixelsPerBeat = pixelsPerSecond * (60f / 116f); // edit bpm (denominator) its an approximation
        //Debug.Log("Pixels per beat: " + pixelsPerBeat);

        // Skip into the song
        float skipDistance = beatTempo * skipDuration;
        transform.position -= new Vector3(0f, skipDistance, 0f);

        // Subscribe to the OnGameStarted event
        GameManager.OnGameStarted += StartScrolling;
    }

    // Method to start scrolling when the game starts
    void StartScrolling()
    {
        hasStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted)
        {
            // Move the beat scroller
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        }
    }
}
