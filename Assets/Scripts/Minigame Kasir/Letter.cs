using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Letter : MonoBehaviour
{
    [SerializeField] private Text charText;
    [HideInInspector] public char charValue;
    private Button buttonObj;

    private void Awake()
    {
        buttonObj = GetComponent<Button>();
        if(buttonObj)
        {
            buttonObj.onClick.AddListener(() => CharSelected());
        }
    }

    public void SetChar(char value)
    {
        charText.text = value + "";
        charValue = value;
    }

    private void CharSelected()
    {
        MinigameKasirManager.instance.SelectedOption(this);
    }
}
