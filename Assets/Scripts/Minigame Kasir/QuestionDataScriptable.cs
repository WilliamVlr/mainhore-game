using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="QuestionData", menuName="QuestionData", order=1)]
public class QuestionDataScriptable : ScriptableObject
{
    public List<QuestionData> questions;

    public void ShuffleQuestions()
    {
        System.Random rng = new System.Random();
        int n = questions.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            QuestionData temp = questions[k];
            questions[k] = questions[n];
            questions[n] = temp;
        }
    }
}