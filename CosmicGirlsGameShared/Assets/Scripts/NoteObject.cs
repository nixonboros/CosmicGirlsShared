using UnityEngine;
using UnityEngine.SceneManagement;

public class NoteObject : MonoBehaviour
{
    public bool canBePressed;

    public KeyCode keyToPress;

    public GameObject hitEffect, goodEffect, perfectEffect, missEffect;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            if (canBePressed)
            {
                gameObject.SetActive(false);

                // Get the x-axis position where the note was pressed
                Vector3 notePosition = transform.position;

                // Set the y-axis position to the bottom 3/4 of the screen
                Vector3 effectPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 4, Camera.main.nearClipPlane));

                // Set the x-axis position to where the note was pressed
                effectPosition.x = notePosition.x;

                if (Mathf.Abs(transform.position.y) > 1f)
                {
                    Debug.Log("Hit");
                    if (SceneManager.GetActiveScene().name == "Level0" || SceneManager.GetActiveScene().name == "Level0Tutorial")
                    {
                        TutorialGameManager.instance.NormalHit();
                    }
                    else
                    {
                        GameManager.instance.NormalHit();
                    }
                    Destroy(gameObject);
                    Instantiate(hitEffect, effectPosition, hitEffect.transform.rotation);
                }
                else if (Mathf.Abs(transform.position.y) > 0.5f)
                {
                    Debug.Log("Good");
                    if (SceneManager.GetActiveScene().name == "Level0" || SceneManager.GetActiveScene().name == "Level0Tutorial")
                    {
                        TutorialGameManager.instance.GoodHit();
                    }
                    else
                    {
                        GameManager.instance.GoodHit();
                    }
                    Destroy(gameObject);
                    Instantiate(goodEffect, effectPosition, goodEffect.transform.rotation);
                }
                else
                {
                    Debug.Log("Perfect");
                    if (SceneManager.GetActiveScene().name == "Level0" || SceneManager.GetActiveScene().name == "Level0Tutorial")
                    {
                        TutorialGameManager.instance.PerfectHit();
                    }
                    else
                    {
                        GameManager.instance.PerfectHit();
                    }
                    Destroy(gameObject);
                    Instantiate(perfectEffect, effectPosition, perfectEffect.transform.rotation);
                }
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (gameObject.activeInHierarchy)
        {
            Destroy(gameObject);

            canBePressed = false;

            if (SceneManager.GetActiveScene().name == "Level0" || SceneManager.GetActiveScene().name == "Level0Tutorial")
            {
                TutorialGameManager.instance.NoteMissed();
            }
            else
            {
                GameManager.instance.NoteMissed();
            }

            // Get the x-axis position where the note was pressed
            Vector3 notePosition = transform.position;

            // Set the y-axis position to the bottom 3/4 of the screen
            Vector3 effectPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 4, Camera.main.nearClipPlane));

            // Set the x-axis position to where the note was pressed
            effectPosition.x = notePosition.x;

            Instantiate(missEffect, effectPosition, missEffect.transform.rotation);
        }
    }
}
