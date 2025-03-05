using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Handle layers in minigame scene, handle timer, handle scoring & target
public class MinigameCafeManager : Minigame
{
    [SerializeField]
    private GameObject targetPanel;
    [SerializeField] private TimerScript timer;
    [SerializeField] private int coinMultiplier;

    public override void checkScore()
    {
        if (currentScore >= targetScore)
        {
            isWin = true;
            targetPanel.GetComponent<Animator>().SetBool("targetAccomplished", true);
        }
    }

    public override int calculateCoinGained()
    {
        return currentScore * 10 * coinMultiplier;
    }

    protected override void CheckResult()
    {
        if (Time.timeScale == 1f && timerText.text == "00" && !isEnded)
        {
            hideLayout(layouts.playLayout);
            showLayout(layouts.endLayout);
            layouts.baseInGameCanvas.hideCanvas();
            layouts.coinLayout.showCanvas();
            layouts.staticLayout.showCanvas();
            if (isWin)
            {
                coinGained = calculateCoinGained();
                CoinManager.Instance.addCoin(coinGained);
                setCoinGainedTxt();
                showLayout(layouts.winLayout);
                SoundManager.Instance.PlayMusicInList("win");
            }
            else
            {
                showLayout(layouts.loseLayout);
                SoundManager.Instance.PlayMusicInList("Lose");
            }
            isEnded = true;
        }
    }

    public override void SetLevel1()
    {
        timer.SetTimerMaxValue(35f);
        targetScore = 8;
        coinMultiplier = 1;
        Init();
        SoundManager.Instance.StopMusic();
    }

    public override void SetLevel2()
    {
        timer.SetTimerMaxValue(30f);
        targetScore = 15;
        coinMultiplier = 2;
        Init();
        SoundManager.Instance.StopMusic();
    }
    public override void SetLevel3()
    {
        timer.SetTimerMaxValue(25f);
        targetScore = 15;
        coinMultiplier = 3;
        Init();
        SoundManager.Instance.StopMusic();
    }
}
