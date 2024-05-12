using UnityEngine;
using UnityEngine.SceneManagement;

public class NoteObject : MonoBehaviour
{
    public bool canBePressed;
    public bool FakeNote = false;
    public bool HiddenNote = false;

    public KeyCode keyToPress;

    public GameObject hitEffect, goodEffect, perfectEffect, missEffect;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 6f)
        {
            spriteRenderer.color = Color.Lerp(originalColor, new Color(originalColor.r, originalColor.g, originalColor.b, 0), (transform.position.y - 6f) / 6f);
        }

        if (HiddenNote && transform.position.y > 3f)
        {
            spriteRenderer.color = Color.Lerp(originalColor, new Color(originalColor.r, originalColor.g, originalColor.b, 0), (transform.position.y - 3f) / 3f);
        }

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

                if (SceneManager.GetActiveScene().name == "Level3" && FakeNote) // LEVEL 3 FAKE NOTE FUNCTIONALITY
                {
                    Debug.Log("Fake Note Miss");
                    GameManager.instance.NoteMissed();

                    Destroy(gameObject);
                    Instantiate(missEffect, effectPosition, missEffect.transform.rotation);
                }
                else
                {
                    if (Mathf.Abs(transform.position.y) > 0.6f)
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
                    else if (Mathf.Abs(transform.position.y) > 0.4f)
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
        if (SceneManager.GetActiveScene().name == "Level3" && FakeNote) // LEVEL 3 FAKE NOTE FUNCTIONALITY
        {
            Destroy(gameObject);
        }
        else
        {
            if (gameObject.activeInHierarchy)
            {
                Debug.Log("Miss");
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
}
