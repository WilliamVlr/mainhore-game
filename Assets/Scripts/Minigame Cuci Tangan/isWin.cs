using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class isWin : MonoBehaviour
{
    [SerializeField] private GameObject layoutTimer;
    private int coinGained;
    [SerializeField] private Fader Fader;
    [SerializeField] private float fadeDuration;

    [SerializeField] private GameObject wincondition;
    [SerializeField] private GameObject Bg;

    [SerializeField] private TimerScript timerScript;
    [SerializeField] private Spawner spawner;
    [SerializeField] private LayoutManager layoutmanager;

    [SerializeField] private CanvasBehavior coincanvas;
    [SerializeField] private CanvasBehavior staticcanvas;

    [SerializeField] private TextMeshProUGUI coinText;
    private void Start()
    {

    }
    public void Condition()
    {
        //Debug.Log(timerScript.timerRemains() + "sec");
        //Debug.Log(spawner.SpawnCount);
        int multiplier = 1;
        if(spawner.SpawnCount == 15)
        {
            multiplier = 2;
        } 
        else if (spawner.SpawnCount == 25)
        {
            multiplier = 5;
        }
        coinGained = timerScript.timerRemains() * spawner.SpawnCount * multiplier;
        layoutmanager.showCanvas(coincanvas);
        layoutmanager.showCanvas(staticcanvas);
        CoinManager.Instance.addCoin(coinGained);

        layoutTimer.gameObject.SetActive(false);
        wincondition.gameObject.SetActive(true);
        Bg.gameObject.SetActive(true);

        coinText.text = "+" + coinGained;

        if (Fader != null)
        {
            // Use the FadeOutGameObject coroutine
            StartCoroutine(Fader.FadeInGameObject(Bg, fadeDuration));
            StartCoroutine(Fader.FadeInGameObject(wincondition, fadeDuration));
        }
        else
        {
            Debug.LogError("spriteFader is not assigned!");
        }

        //Debug.Log("win");
    }
}
