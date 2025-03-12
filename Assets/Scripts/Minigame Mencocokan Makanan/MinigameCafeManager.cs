using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//Handle layers in minigame scene, handle timer, handle scoring & target
public class MinigameCafeManager : Minigame, IDataPersistence
{
    [SerializeField]
    private GameObject targetPanel;
    [SerializeField] private TimerScript timer;
    [SerializeField] private int coinMultiplier;
    [SerializeField] private List<CafeCustomerSpawner> spawners;
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private Sprite winPanelImg;
    [SerializeField] private Sprite losePanelImg;
    [SerializeField] private Image resultPanel;
    [SerializeField] private TextMeshProUGUI targetTxtResult;
    [SerializeField] private TextMeshProUGUI skormuTxtResult;
    [SerializeField] private CanvasBehavior resultCanvas;

    private int chosenLevel;
    private int progress;

    public override void checkScore()
    {
        if (currentScore >= targetScore)
        {
            isWin = true;
            targetPanel.GetComponent<Animator>().SetBool("targetAccomplished", true);
            int level = chosenLevel - 1;
            if(progress < 2)
            {
                progress = level + 1;
            }
        }
    }

    public override int calculateCoinGained()
    {
        int initial = 100;
        //if (chosenLevel == 2) initial = 100;
        //else if (chosenLevel == 3) initial = 200;
        return initial + currentScore * 10 * coinMultiplier;
    }

    protected override void CheckResult()
    {
        if (Time.timeScale == 1f && timerText.text == "00" && !isEnded)
        {
            hideLayout(layouts.playLayout);
            layouts.baseInGameCanvas.hideCanvas();
            layouts.coinLayout.showCanvas();
            layouts.staticLayout.showCanvas();
            targetTxtResult.text = targetScore.ToString();
            skormuTxtResult.text = currentScore.ToString();
            if (isWin)
            {
                SetWinPanel();
            }
            else
            {
                SetLosePanel();
            }
            resultCanvas.showCanvas();
            isEnded = true;
        }
    }

    public void SetWinPanel()
    {
        resultPanel.sprite = winPanelImg;
        coinGained = calculateCoinGained();
        CoinManager.Instance.addCoin(coinGained);
        setCoinGainedTxt();
        targetTxtResult.color = new Color32(85, 180, 1, 255);
        SoundManager.Instance.PlaySFXInList("win");
    }

    public void SetLosePanel()
    {
        resultPanel.sprite = losePanelImg;
        coinGained = calculateCoinGained();
        if (coinGained > 100) coinGained = 100;
        CoinManager.Instance.addCoin(coinGained);
        setCoinGainedTxt();
        targetTxtResult.color = new Color32(231, 26, 0, 255);
        SoundManager.Instance.PlaySFXInList("Lose");
    }

    public override void SetLevel1()
    {
        timer.SetTimerMaxValue(35f);
        targetScore = 8;
        coinMultiplier = 1;
        foreach (var spawner in spawners)
        {
            spawner.SetCustomerWaitTime(4, 6);
        }
        Init();
        SoundManager.Instance.StopMusic();
        chosenLevel = 1;
    }

    public override void SetLevel2()
    {
        timer.SetTimerMaxValue(30f);
        targetScore = 15;
        coinMultiplier = 2;
        foreach (var spawner in spawners)
        {
            spawner.SetCustomerWaitTime(3, 5);
        }
        Init();
        SoundManager.Instance.StopMusic();
        chosenLevel = 2;
    }
    public override void SetLevel3()
    {
        timer.SetTimerMaxValue(25f);
        targetScore = 15;
        coinMultiplier = 3;
        foreach (var spawner in spawners)
        {
            spawner.SetCustomerWaitTime(2, 4);
        }
        Init();
        SoundManager.Instance.StopMusic();
        chosenLevel = 3;
    }

    public void PopulateLevelButtonInteractable()
    {
        for(int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
        }

        for (int i = 0; i <= progress; i++)
        {
            levelButtons[i].interactable = true;
        }
    }

    public void LoadData(GameData data)
    {
        this.progress = data.minigamesProgress["cafe"];
        PopulateLevelButtonInteractable();
    }

    public void SaveData(ref GameData data)
    {
        data.minigamesProgress["cafe"] = this.progress;
    }
}
