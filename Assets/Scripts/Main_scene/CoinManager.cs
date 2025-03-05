using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour, IDataPersistence
{
    public static CoinManager Instance;
    public int coinAmount;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        updateUI();
    }

    public void LoadData(GameData data)
    {
        this.coinAmount = data.coinAmount;
        CoinManager.Instance.updateUI();
    }

    public void SaveData(ref GameData data)
    {
        Debug.Log("Coin manager amount: " + coinAmount);
        Debug.Log("data coin amount: " +  data.coinAmount);
        data.coinAmount = this.coinAmount;
    }

    public void addCoin(int coinValue)
    {
        coinAmount += coinValue;
        updateUI();
    }

    public bool canSubstractCoin(int coinValue)
    {
        return coinAmount >= coinValue;
    }

    public void substractCoin(int coinValue)
    {
        if(coinAmount < coinValue)
        {
            Debug.Log("Coin amount not enough to be substracted by "  + coinValue + ".\nCoin Amount value remains = " + coinAmount);
            return;
        }
        coinAmount -= coinValue;
        updateUI();
    }

    public void updateUI()
    {
        CoinUI coin = FindAnyObjectByType<CoinUI>();
        if (coin)
        {
            coin.setText(coinAmount.ToString());
        }
    }
}
