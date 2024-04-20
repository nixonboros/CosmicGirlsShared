using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{

    public float beatTempo;

    public bool hasStarted;

    public float skipDuration = 0f; // Duration to skip into the song

    private float pixelsPerSecond;
    private float pixelsPerBeat;

    // Start is called before the first frame update
    void Start()
    {
        beatTempo = beatTempo / 15f;

        // Calculate y pixels per beat
        pixelsPerSecond = beatTempo;
        pixelsPerBeat = pixelsPerSecond * (60f / 124f); // edit bpm (denominator) its an approximation
        Debug.Log(pixelsPerBeat);

        // Skip into the song
        float skipDistance = beatTempo * skipDuration;
        transform.position -= new Vector3(0f, skipDistance, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f); 
    }
}
