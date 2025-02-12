using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseCondition : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _Time;

    [SerializeField] private GameObject Timer;

    [SerializeField] private Fader Fader;

    [SerializeField] private int gameDoneCheck;

    [SerializeField] private isWin _isWin;
    [SerializeField] private isLose _isLose;

    GameObject[] viruses;

    int check = 0;

    [SerializeField] private Spawner _spawner;


    // Start is called before the first frame update
    void Start()
    {
        gameDoneCheck = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_Time.text == "00" && check == 0)  // Game over due to time running out
        {
            if (_spawner.VirusDestroyed == 10)  // If the time is up and enough viruses are destroyed, it's a win
            {
                //Debug.Log("You Win!");
                StartCoroutine(Fader.FadeOutGameObject(Timer, (float)0.5));

                _isWin.Condition();
                check++; // Prevent further checks after a condition is met
                gameDoneCheck++;
            }
            else
            {
                //Debug.Log("You Lose!");
                StartCoroutine(Fader.FadeOutGameObject(Timer, (float)0.5));

                GameObject[] viruses = GameObject.FindGameObjectsWithTag("Virus");
                //Debug.Log(viruses.Length);

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
        else if (_spawner.VirusDestroyed == _spawner.SpawnCount && check == 0)  // If enough viruses are destroyed before time is up
        {
            //Debug.Log("You Win!");
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

}
