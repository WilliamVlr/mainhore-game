using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public GameObject coin;

    public void setCoin(int coinValue)
    {
        Text coinText = coin.GetComponent<Text>();
        if (coinText != null)
        {
            coinText.text = coinValue.ToString(); // Convert int to string and set the text
        }
        else
        {
            Debug.LogError("Text component not found on coin GameObject!");
        }
    }

    // Method to get the coin text value
    public string getCoin()
    {
        Text coinText = coin.GetComponent<Text>();
        if (coinText != null)
        {
            return coinText.text; // Return the text value
        }
        else
        {
            Debug.LogError("Text component not found on coin GameObject!");
            return "0"; // Return default value
        }
    }
}
