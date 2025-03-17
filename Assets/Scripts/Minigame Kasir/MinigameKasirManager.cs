using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using Unity.Collections;
using UnityEngine.Rendering.Universal;
using TMPro;
using Unity.Burst.CompilerServices;

public class MinigameKasirManager : Minigame, IDataPersistence
{
    public static MinigameKasirManager instance;
    private GameStatus gameStatus;
    [SerializeField] private Image imageQuestion;
    [SerializeField] private Letter[] answerArray;
    [SerializeField] private Letter[] optionArray;
    [SerializeField] private QuestionDataScriptable questionData1;
    [SerializeField] private QuestionDataScriptable questionData2;
    [SerializeField] private QuestionDataScriptable questionData3;
    private QuestionDataScriptable questionData;
    [SerializeField] private Image[] lives;
    [SerializeField] private TimerScript timer;
    [SerializeField] private Canvas playPanel;
    [SerializeField] private CustomerHandler customerHandler;

    [Header("Result Panel")]
    [SerializeField] private CanvasBehavior resultCanvas;
    [SerializeField] private Image resultPanel;
    [SerializeField] private Sprite winPanelImg;
    [SerializeField] private Sprite losePanelImg;
    [SerializeField] private Button[] levelButtons;

    [Header("Audio")]
    [SerializeField] private AudioClip correctAudio;
    [SerializeField] private AudioClip wrongAudio;

    [Header("Hint")]
    [SerializeField] private GameObject hint;
    [SerializeField] private Sprite grayscaledHint;
    [SerializeField] private Sprite normalHint;

    private char[] charArray;
    private string answerWord;
    private List<int> letterChoiceIndex;
    private int currentLetterIndex;
    private int currentQuestionIndex;
    private bool correctAnswer;
    private int liveCount;
    private int availableHint;

    private int chosenLevel;
    private int progress;

    private int mustBeAnsweredQuestion;
    private int answeredQuestion;

    void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        letterChoiceIndex = new List<int>();
        currentQuestionIndex = 0;
        gameStatus = GameStatus.Starting;
        charArray = new char[optionArray.Length];
        liveCount = lives.Length;
        playPanel.gameObject.SetActive(false);
        isEnded = false;
        mustBeAnsweredQuestion = -1;
        answeredQuestion = 0;
        SoundManager.Instance.PlayMusicInList("Jalan");
    }

    public GameStatus getGameStatus()
    {
        return gameStatus;
    }


    public void setGameStatus(GameStatus newGameStatus)
    {
        gameStatus = newGameStatus;
    }
    public override int calculateCoinGained()
    {
        return (int) (liveCount*3 + chosenLevel*7)*answeredQuestion + timer.timerRemains()*answeredQuestion/2;
    }

    public override void checkScore()
    {
        if(liveCount == 0 || timerText.text == "00") isWin = false;
        else if(mustBeAnsweredQuestion == 0 && liveCount == 3)
        {   
            isWin = true; 
        }
    }

    public override void addScore()
    {
        currentScore++;
        setCurrentScoreTxt();
    }
    protected override void CheckResult()
    {
        if (Time.timeScale == 1f && (timerText.text == "00" || liveCount == 0 || mustBeAnsweredQuestion == 0) && !isEnded)
        {   
            checkScore();
            layouts.baseInGameCanvas.hideCanvas();
            layouts.coinLayout.showCanvas();
            layouts.staticLayout.showCanvas();
            targetScoreTXT.text = targetScore.ToString();
            currentScoreTXT.text = currentScore.ToString();
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
        targetScoreTXT.color = new Color32(85, 180, 1, 255);
        SoundManager.Instance.PlaySFXInList("win");
        int level = chosenLevel - 1;
        if (progress < 2)
        {
            progress = level + 1;
        }
    }

    public void SetLosePanel()
    {
        resultPanel.sprite = losePanelImg;
        coinGained = calculateCoinGained();
        if(coinGained > 70) coinGained = 70;
        CoinManager.Instance.addCoin(coinGained);
        setCoinGainedTxt();
        targetScoreTXT.color = new Color32(231, 26, 0, 255);
        SoundManager.Instance.PlaySFXInList("Lose");
    }

    private void SetQuestion()
    {   
        customerHandler.spawnCustomer();
        currentLetterIndex = 0;
        availableHint = 1;
        letterChoiceIndex.Clear();
        imageQuestion.sprite = questionData.questions[currentQuestionIndex].questionImage;
        hint.GetComponent<Image>().sprite = normalHint;
        answerWord = questionData.questions[currentQuestionIndex].answer;
        ResetQuestion();
        
        for(int i = 0; i < answerWord.Length; i++) charArray[i] = Char.ToUpper(answerWord[i]);
        for(int i = answerWord.Length; i < optionArray.Length; i++) charArray[i] = (char) UnityEngine.Random.Range(65, 91);
        charArray = ShuffleList.ShuffleListItems<char>(charArray.ToList()).ToArray();
        for(int i = 0; i < optionArray.Length; i++) optionArray[i].SetChar(charArray[i]);
        
        currentQuestionIndex++;
    }

    public void showPanel()
    {   
        SoundManager.Instance.PlaySFX(questionData.questions[currentQuestionIndex-1].sfxItem);
        playPanel.gameObject.SetActive(true);
        gameStatus = GameStatus.Playing;
    }

    public void closePanel()
    {
        playPanel.gameObject.SetActive(false);
        gameStatus = GameStatus.Ending;
    }
    public void SelectedOption(Letter letter)
    {
        if(gameStatus == GameStatus.Starting || currentLetterIndex >= answerWord.Length) return;
        letterChoiceIndex.Add(letter.transform.GetSiblingIndex());

        answerArray[currentLetterIndex++].SetChar(letter.charValue);
        letter.gameObject.SetActive(false);
        
        if(currentLetterIndex >= answerWord.Length)
        {
            correctAnswer = true;
            for(int i = 0; i < answerWord.Length; i++)
            {
                if(Char.ToUpper(answerArray[i].charValue) != Char.ToUpper(answerWord[i]))
                {
                    correctAnswer = false;
                    break;
                }
            }
        }

        if(correctAnswer && currentLetterIndex >= answerWord.Length)
        {   
            SoundManager.Instance.PlaySFX(correctAudio);
            closePanel();
            mustBeAnsweredQuestion--;
            answeredQuestion++;
            addScore();
            CheckResult();
            if(currentQuestionIndex < questionData.questions.Count && !isEnded) Invoke("SetQuestion", 0.5f);
        }
        else if(!correctAnswer && currentLetterIndex >= answerWord.Length)
        {
            SoundManager.Instance.PlaySFX(wrongAudio);
            liveCount--;
            lives[liveCount].gameObject.SetActive(false);
            closePanel();
            mustBeAnsweredQuestion--;
            CheckResult();
            if(currentQuestionIndex < questionData.questions.Count && !isEnded) Invoke("SetQuestion", 0.5f);
        }
    }

    private void ResetQuestion()
    {
        for(int i = 0; i < answerArray.Length; i++)
        {
            answerArray[i].gameObject.SetActive(true);
            answerArray[i].SetChar('_');
        }

        for(int i = answerWord.Length; i < answerArray.Length; i++)
        {
            answerArray[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < optionArray.Length; i++)
        {
            optionArray[i].gameObject.SetActive(true);
        }
        currentLetterIndex = 0;
    }

    public void removeLastLetter()
    {
        if(letterChoiceIndex.Count > 0)
        {
            int index = letterChoiceIndex[letterChoiceIndex.Count-1];
            optionArray[index].gameObject.SetActive(true);
            letterChoiceIndex.RemoveAt(letterChoiceIndex.Count-1);

            currentLetterIndex--;
            answerArray[currentLetterIndex].SetChar('_');
        }
    }

    public void resetAnswer()
    {
        for(int i = 0; i < answerArray.Length; i++) removeLastLetter();
    }

    public void getHint()
    {   
        if(availableHint == 0) return;
        // if(currentLetterIndex > 0)
        // {   
        //     int foundLastWrongIndex = currentLetterIndex;
        //     for(int i = currentLetterIndex-1; i >= 0; i--)
        //     {
        //         if(Char.ToUpper(answerArray[i].charValue) != Char.ToUpper(answerWord[i]))
        //         {
        //             foundLastWrongIndex = i;
        //         }
        //     }

        //     int needToRemoveCount = currentLetterIndex-foundLastWrongIndex;
        //     for(int i = 0; i < needToRemoveCount; i++)
        //     {
        //         removeLastLetter();
        //     }
        // }
        // else
        // {
        for(int i = 0; i < optionArray.Length; i++)
        {
            if((Char.ToUpper(optionArray[i].charValue) == Char.ToUpper(answerWord[currentLetterIndex])) && optionArray[i].isActiveAndEnabled)
            {
                SelectedOption(optionArray[i]);
                Debug.Log("Found Hint");
                break;
            }
        }
        // }
        availableHint--;

        if(availableHint == 0)
        {
            hint.GetComponent<Image>().sprite = grayscaledHint;
        }
    }

    public override void SetLevel1()
    {
        timer.SetTimerMaxValue(45f);
        questionData = questionData1;
        questionData.ShuffleQuestions();
        mustBeAnsweredQuestion = questionData.questions.Count();
        targetScore = 5;
        currentScore = 0;
        setTargetScoreTxt();
        SetQuestion();
        SoundManager.Instance.StopMusic();
        chosenLevel = 1;
    }

    public override void SetLevel2()
    {
        timer.SetTimerMaxValue(70f);
        questionData = questionData2;
        questionData.ShuffleQuestions();
        mustBeAnsweredQuestion = questionData.questions.Count();
        targetScore = 8;
        currentScore = 0;
        setTargetScoreTxt();
        SetQuestion();
        SoundManager.Instance.StopMusic();
        chosenLevel = 2;
    }

    public override void SetLevel3()
    {
        timer.SetTimerMaxValue(60f);
        questionData = questionData3;
        questionData.ShuffleQuestions();
        mustBeAnsweredQuestion = questionData.questions.Count();
        targetScore = 8;
        currentScore = 0;
        setTargetScoreTxt();
        SetQuestion();
        SoundManager.Instance.StopMusic();
        chosenLevel = 3;
    }

    public void PopulateLevelButtonInteractable()
    {
        for (int i = 0; i < levelButtons.Length; i++)
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
        this.progress = data.minigamesProgress["kasir"];
        PopulateLevelButtonInteractable();
    }

    public void SaveData(ref GameData data)
    {
        data.minigamesProgress["kasir"] = this.progress;
    }
}
