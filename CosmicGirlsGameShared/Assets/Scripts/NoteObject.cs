using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

                //GameManager.instance.NoteHit();

                if (Mathf.Abs(transform.position.y) > 1f)
                {
                    Debug.Log("Hit");
                    if (SceneManager.GetActiveScene().name == "Level0") {
                        TutorialGameManager.instance.NormalHit();
                    }
                    else
                    {
                        GameManager.instance.NormalHit();
                    }
                      
                    Instantiate(hitEffect, transform.position, hitEffect.transform.rotation);
                }
                else if (Mathf.Abs(transform.position.y) > 0.5f)
                {
                    Debug.Log("Good");
                    if (SceneManager.GetActiveScene().name == "Level0")
                    {
                        TutorialGameManager.instance.GoodHit();
                    }
                    else
                    {
                        GameManager.instance.GoodHit();
                    }
                    Instantiate(goodEffect, transform.position, goodEffect.transform.rotation);
                }
                else
                {
                    Debug.Log("Perfect");
                    if (SceneManager.GetActiveScene().name == "Level0")
                    {
                        TutorialGameManager.instance.PerfectHit();
                    }
                    else
                    {
                        GameManager.instance.PerfectHit();
                    }
                    Instantiate(perfectEffect, transform.position, perfectEffect.transform.rotation);
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
            canBePressed = false;

            if (SceneManager.GetActiveScene().name == "Level0")
            {
                TutorialGameManager.instance.NoteMissed();
            }
            else
            {
                GameManager.instance.NoteMissed();
            }
            Instantiate(missEffect, transform.position, missEffect.transform.rotation);

        }
    }
}