using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    private Text coinTxt;

    private void Awake()
    {
        coinTxt = GetComponent<Text>();
    }

    public void setText(string text)
    {
        coinTxt.text = text;
    }
}
