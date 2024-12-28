using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Handle layers in minigame scene, handle timer, handle scoring & target
public class MinigameCafeManager : Minigame
{
    [SerializeField]
    private GameObject targetPanel;

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
        return targetScore * 10;
    }
}
