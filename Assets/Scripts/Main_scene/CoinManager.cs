using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    public GameObject coin;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            CoinManager.Instance.updateUI();
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(CoinManager.Instance);
        }
    }

    public void addCoin(int coinValue)
    {
        Text coinText;
        int temp = 0;
        if (coin)
        {
            coinText = coin.GetComponent<Text>();
            temp += (int.Parse(coinText.text) + coinValue);
            coinText.text = temp.ToString();
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
    public void updateUI()
    {
        Text coinText;
        if (coin)
        {
            coinText = coin.GetComponent<Text>();
            coinText.text = getCoin();
        }
    }
}
