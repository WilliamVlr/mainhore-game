using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Handle layers in minigame scene, handle timer, handle scoring & target
public abstract class Minigame : MonoSingleton<Minigame>
{
    //Scoring
    [SerializeField] private TextMeshProUGUI currentScoreTXT;
    [SerializeField] private TextMeshProUGUI targetScoreTXT;
    private int currentScore;
    private int targetScore;
    private bool isWin;

    //Timer

    //Layouts

    //Awake function
    

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
    }

    public virtual void checkScore() {
        if(currentScore >= targetScore) isWin = true;
    }




}
