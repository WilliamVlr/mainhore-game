using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseCondition : MonoBehaviour
{
    public Text Time;

    public GameObject Timer;
    public GameObject PauseButton;

    public Fader Fader;

    public int gameDoneCheck;

    public isWin _isWin;
    public isLose _isLose;

    GameObject[] viruses;

    int check = 0;

    public Spawner _spawner;


    // Start is called before the first frame update
    void Start()
    {
        gameDoneCheck = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.text == "00" && check == 0)  // Game over due to time running out
        {
            fadePauseButton();
            if (_spawner._virusDestroyed == 10)  // If the time is up and enough viruses are destroyed, it's a win
            {
                Debug.Log("You Win!");
                StartCoroutine(Fader.FadeOutGameObject(Timer, (float)0.5));
                
                _isWin.Condition();
                check++; // Prevent further checks after a condition is met
                gameDoneCheck++;
            }
            else
            {
                Debug.Log("You Lose!");
                StartCoroutine(Fader.FadeOutGameObject(Timer, (float)0.5));

                GameObject[] viruses = GameObject.FindGameObjectsWithTag("Virus");
                Debug.Log(viruses.Length);

                foreach (GameObject virus in viruses)
                {
                    VirusMove virusMove = virus.GetComponent<VirusMove>();

                    virusMove.speed = 0f;

                    if (!virusMove)
                    {
                        Debug.LogError("VirusMove script not found on a virus object!");
                    }
                }

                _isLose.Condition();
                check++; // Prevent further checks after a condition is met
                gameDoneCheck++;
            }
        }
        else if (_spawner._virusDestroyed == 10 && check == 0)  // If enough viruses are destroyed before time is up
        {
            fadePauseButton();
            Debug.Log("You Win!");
            StartCoroutine(Fader.FadeOutGameObject(Timer, 1));

            _isWin.Condition();
            check++; // Prevent further checks after a condition is met
            gameDoneCheck++;
        }
        
    }

    public int getGameDoneCheck()
    {
        return gameDoneCheck;
    }

    private void fadePauseButton()
    {
        StartCoroutine(Fader.FadeOutGameObject(PauseButton, 0.5f));
    }
}
