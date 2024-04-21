using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldNote : MonoBehaviour
{
    // Inspector variables for setting the sprites and movement speed
    public GameObject topNotePrefab;
    public GameObject middleNotePrefab;
    public GameObject bottomNotePrefab;
    public float holdNoteLength = 3.0f; // The desired length of the hold note
    public float speed = 2.0f; // The speed at which the note moves down

    // Private variable to hold the container GameObject
    private GameObject holdNoteContainer;

    void Start()
    {
        // Create a container to hold the parts of the note
        holdNoteContainer = new GameObject("HoldNoteContainer");

        // Call the method to create the hold note
        CreateHoldNote();
    }

    void Update()
    {
        // Move the entire container down by adjusting its position
        holdNoteContainer.transform.position += Vector3.down * speed * Time.deltaTime;
    }

    void CreateHoldNote()
    {
        // Instantiate the bottom part of the note
        GameObject bottom = Instantiate(bottomNotePrefab, this.transform.position, Quaternion.identity);
        bottom.transform.parent = holdNoteContainer.transform;

        // Calculate how many middle parts we need based on the hold note length
        SpriteRenderer middleSpriteRenderer = middleNotePrefab.GetComponent<SpriteRenderer>();
        float middleNoteHeight = middleSpriteRenderer.bounds.size.y;
        int numberOfMiddleNotes = Mathf.FloorToInt(holdNoteLength / middleNoteHeight);

        // Instantiate the middle parts
        for (int i = 0; i < numberOfMiddleNotes; i++)
        {
            Vector3 position = this.transform.position + new Vector3(0, middleNoteHeight * i, 0);
            GameObject middle = Instantiate(middleNotePrefab, position, Quaternion.identity);
            middle.transform.parent = holdNoteContainer.transform;
        }

        // Instantiate the top part of the note and place it at the end
        Vector3 topPosition = this.transform.position + new Vector3(0, middleNoteHeight * numberOfMiddleNotes, 0);
        GameObject top = Instantiate(topNotePrefab, topPosition, Quaternion.identity);
        top.transform.parent = holdNoteContainer.transform;

        // Adjust the entire container to be centered
        holdNoteContainer.transform.position -= new Vector3(0, holdNoteLength / 2, 0);
    }
}