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
    public CanvasBehavior staticLayout;
    public CanvasBehavior coinLayout; 
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
    protected bool isEnded;

    //Coin gained
    [SerializeField] private TextMeshProUGUI coinGainedTXT;
    protected int coinGained;

    //Timer object
    [SerializeField] private TextMeshProUGUI timerText;

    //Layouts
    [SerializeField] private MinigameLayouts layouts;

    //Animation Scripts
    private Fader fader;
    public float fadeDuration = 1.0f;

    public override void Init()
    {
        setTargetScoreTxt();
        resetScore();
        resetIsWin();
        isEnded = false;
        Time.timeScale = 0f;
        fader = FindObjectOfType<Fader>();
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
        StartCoroutine(fader.FadeInGameObject(layout, fadeDuration));
    }

    public void showCanvas(CanvasBehavior canvas)
    {
        canvas.showCanvas();
    }

    public void hideCanvas(CanvasBehavior canvas)
    {
        canvas.hideCanvas();
    }

    public void hideLayout(GameObject layout)
    {
        layout.SetActive(false);
        StartCoroutine(fader.FadeOutGameObject(layout, fadeDuration));
    }

    public void startMinigame()
    {
        Time.timeScale = 1f;
    }

    public void restartMinigame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Debug.Log("Restarting Minigame");
    }

    public void returnToHomeScreen()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScreen");
    }

    private void Update()
    {
        if(Time.timeScale == 1f && timerText.text == "00" && !isEnded)
        {
            hideLayout(layouts.playLayout);
            showLayout(layouts.endLayout);
            layouts.coinLayout.showCanvas();
            layouts.staticLayout.showCanvas();
            if (isWin)
            {
                coinGained = calculateCoinGained();
                CoinManager.Instance.addCoin(coinGained);
                setCoinGainedTxt();
                showLayout(layouts.winLayout);
            } 
            else
            {
                showLayout(layouts.loseLayout);
            }
            isEnded = true;
        }
    }
}
