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
    public CanvasBehavior baseInGameCanvas;
}

//Handle layers in minigame scene, handle scoring & target, and coin gained
public abstract class Minigame : MonoSingleton<Minigame>
{
    //Scoring
    [SerializeField] protected TextMeshProUGUI currentScoreTXT;
    [SerializeField] protected TextMeshProUGUI targetScoreTXT;
    protected int currentScore;
    [SerializeField] protected int targetScore;
    protected bool isWin;
    protected bool isEnded;

    //Coin gained
    [SerializeField] private TextMeshProUGUI coinGainedTXT;
    protected int coinGained;

    //Timer object
    [SerializeField] protected TextMeshProUGUI timerText;

    //Layouts
    [SerializeField] protected MinigameLayouts layouts;

    //Animation Scripts
    private Fader fader;
    public float fadeDuration = 1.0f;

    private void Start()
    {
        SoundManager.Instance.PlayMusicInList("Jalan");
    }

    public override void Init()
    {
        setTargetScoreTxt();
        resetScore();
        resetIsWin();
        isEnded = false;
        Time.timeScale = 0f;
        fader = FindObjectOfType<Fader>();
    }

    protected void resetIsWin()
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

    public virtual void SetLevel1()
    {
        //Must be override in child
    }

    public virtual void SetLevel2()
    {
        //Must be override in child
    }

    public virtual void SetLevel3()
    {
        //Must be override in child
    }

    public void SetTargetScore(int target)
    {
        this.targetScore = target;
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

    protected void setCoinGainedTxt()
    {
        coinGainedTXT.text = $"+{coinGained}";
    }

    public void showLayout(GameObject layout)
    {
        layout.SetActive(true);
        if(fader != null)
        {
            StartCoroutine(fader.FadeInGameObject(layout, fadeDuration));
        }
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
        if(fader != null)
        {
            StartCoroutine(fader.FadeOutGameObject(layout, fadeDuration));
        }
    }

    public void startMinigame()
    {
        Time.timeScale = 1f;
    }

    public void restartMinigame()
    {
        Time.timeScale = 1;
        Debug.Log("Calling DataPersistenceManager.SaveGame from Minigame class");
        DataPersistenceManager.Instance.saveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void returnToHomeScreen()
    {
        Time.timeScale = 1;
        DataPersistenceManager.Instance.saveGame();
        SceneManager.LoadScene("MainScreen");
    }

    protected void Update()
    {
        CheckResult();
    }

    protected virtual void CheckResult()
    {
        //override to change the logic for showing end result panel
    }
}
