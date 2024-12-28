using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MinigameLayouts
{
    public GameObject playLayout;
    public GameObject endLayout;
    public GameObject winLayout;
    public GameObject loseLayout;
    public GameObject baseButtonLayout;
}

//Handle layers in minigame scene, handle scoring & target, and coin gained
public abstract class Minigame : MonoSingleton<Minigame>
{
    //Scoring
    [SerializeField] private TextMeshProUGUI currentScoreTXT;
    [SerializeField] private TextMeshProUGUI targetScoreTXT;
    protected int currentScore;
    [SerializeField] protected int targetScore;
    protected bool isWin;

    //Coin gained
    [SerializeField] private TextMeshProUGUI coinGainedTXT;
    protected int coinGained;

    //Timer object
    [SerializeField] private TextMeshProUGUI timerText;

    //Layouts
    [SerializeField] private MinigameLayouts layouts;

    public override void Init()
    {
        setTargetScoreTxt();
        resetScore();
        resetIsWin();
        Time.timeScale = 0f;
    }

    private void resetIsWin()
    {
        isWin = false;
    }

    public void resetScore()
    {
        currentScore = 0;
    }

    public void setCurrentScoreTxt()
    {
        currentScoreTXT.text = $"{currentScore}";
    }
    
    public void setTargetScoreTxt()
    {
        targetScoreTXT.text = $"{targetScore}";
    }

    //All virtual functions must be override in child class
    public virtual void addScore()
    {
        currentScore++;
        setCurrentScoreTxt();
        if (!isWin)
        {
            checkScore();
        }
    }

    public virtual void checkScore() {
        if (currentScore >= targetScore)
        {
            isWin = true;
        }
    }

    public virtual int calculateCoinGained()
    {
        return coinGained;
    }

    private void setCoinGainedTxt()
    {
        coinGainedTXT.text = $"+{coinGained}";
    }

    public void showLayout(GameObject layout)
    {
        layout.SetActive(true);
    }

    public void hideLayout(GameObject layout)
    {
        layout.SetActive(false);
    }

    public void startMinigame()
    {
        Time.timeScale = 1f;
    }

    public void restartMinigame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restarting Minigame");
    }

    private void Update()
    {
        if(Time.timeScale == 1f && timerText.text == "00")
        {
            hideLayout(layouts.playLayout);
            showLayout(layouts.endLayout);
            showLayout(layouts.baseButtonLayout);
            if (isWin)
            {
                coinGained = calculateCoinGained();
                setCoinGainedTxt();
                showLayout(layouts.winLayout);
            } 
            else
            {
                showLayout(layouts.loseLayout);
            }
        }
    }
}
