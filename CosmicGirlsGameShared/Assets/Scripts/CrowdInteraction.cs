using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdInteraction : MonoBehaviour
{
    public GameObject ExclamationChatbox;
    public GameObject CrossChatboxS;
    public GameObject HeartChatbox;
    public GameObject CrossChatboxL;

    //private GameManager gameManager;
    private bool coroutineRightRunning = false;
    private bool coroutineLeftRunning = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!coroutineLeftRunning)
        {
            int randomLeft = Random.Range(0, 2);
            Debug.Log(randomLeft);
            if (randomLeft == 0)
            {
                StartCoroutine(SkillCheck(ExclamationChatbox, false, false));
            }
            else
            {
                StartCoroutine(SkillCheck(CrossChatboxS, false, true));
            }     
        }

        if (!coroutineRightRunning)
        {
            int randomRight = Random.Range(0, 2);
            if (randomRight == 0)
            {
                StartCoroutine(SkillCheck(HeartChatbox, true, false));
            }
            else
            {
                StartCoroutine(SkillCheck(CrossChatboxL, true, true));
            }
        }
    }

    IEnumerator SkillCheck(GameObject go, bool right, bool isCross)
    {
        if (right)
        {
            coroutineRightRunning = true;
        }
        else
        {
            coroutineLeftRunning = true;
        }

        float randomNumber = Random.Range(5f, 10f);
        yield return new WaitForSeconds(randomNumber);
        go.SetActive(true);

        //float timeLimit = Random.Range(2f, 5f);
        float timeLimit = 1f;
        for (float timer = timeLimit; timer >= 0; timer -= Time.deltaTime)
        {
            if (right && Input.GetKeyDown(KeyCode.L))
            {
                if (!isCross)
                {
                    GameManager.instance.ChatboxHit();
                    go.SetActive(false);
                    coroutineRightRunning = false;
                    yield break;
                }
                else
                {
                    GameManager.instance.CrossHit();
                    go.SetActive(false);
                    coroutineRightRunning = false;
                    yield break;
                }
                
            }
            else if (!right && Input.GetKeyDown(KeyCode.S))
            {
                if (!isCross)
                {
                    GameManager.instance.ChatboxHit();
                    go.SetActive(false);
                    coroutineLeftRunning = false;
                    yield break;
                }
                else
                {
                    GameManager.instance.CrossHit();
                    go.SetActive(false);
                    coroutineRightRunning = false;
                    yield break;
                }
            }
            yield return null;
        }

        go.SetActive(false);

        if (right)
        {
            coroutineRightRunning = false;
        }
        else
        {
            coroutineLeftRunning = false;
        }
    } 

}

