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

public class MinigameKasirManager : Minigame
{
    public static MinigameKasirManager instance;
    private GameStatus gameStatus;
    [SerializeField] private Image imageQuestion;
    [SerializeField] private Letter[] answerArray;
    [SerializeField] private Letter[] optionArray;
    [SerializeField] private QuestionDataScriptable questionData;
    [SerializeField] private Image[] lives;
    [SerializeField] private Canvas playPanel;
    [SerializeField] private CustomerHandler customerHandler;

    private char[] charArray;
    private string answerWord;
    private List<int> letterChoiceIndex;
    private int currentLetterIndex;
    private int currentQuestionIndex;
    private bool correctAnswer;
    private int liveCount;
    private int availableHint;


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
        SetQuestion();
    }

    public GameStatus getGameStatus()
    {
        return gameStatus;
    }


    public void setGameStatus(GameStatus newGameStatus)
    {
        gameStatus = newGameStatus;
    }

    private void SetQuestion()
    {   
        customerHandler.spawnCustomer();
        currentLetterIndex = 0;
        availableHint = 1;
        letterChoiceIndex.Clear();
        imageQuestion.sprite = questionData.questions[currentQuestionIndex].questionImage;
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
            closePanel();
            if(currentQuestionIndex < questionData.questions.Count) Invoke("SetQuestion", 0.5f);
        }
        else if(!correctAnswer && currentLetterIndex >= answerWord.Length)
        {
            liveCount--;
            lives[liveCount].gameObject.SetActive(false);
            closePanel();
            if(currentQuestionIndex < questionData.questions.Count) Invoke("SetQuestion", 0.5f);
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
        if(currentLetterIndex > 0)
        {   
            int foundLastWrongIndex = currentLetterIndex;
            for(int i = currentLetterIndex-1; i >= 0; i--)
            {
                if(Char.ToUpper(answerArray[i].charValue) != Char.ToUpper(answerWord[i]))
                {
                    foundLastWrongIndex = i;
                }
            }

            int needToRemoveCount = currentLetterIndex-foundLastWrongIndex;
            for(int i = 0; i < needToRemoveCount; i++)
            {
                removeLastLetter();
            }
        }
        else
        {
            for(int i = 0; i < optionArray.Length; i++)
            {
                if((Char.ToUpper(optionArray[i].charValue) == Char.ToUpper(answerWord[currentLetterIndex])) && optionArray[i].isActiveAndEnabled)
                {
                    SelectedOption(optionArray[i]);
                    Debug.Log("Found Hint");
                    break;
                }
            }
        }
        availableHint--;
    }
}
