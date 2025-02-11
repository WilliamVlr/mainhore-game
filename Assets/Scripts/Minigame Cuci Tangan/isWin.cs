using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class isWin : MonoBehaviour
{
    [SerializeField] private GameObject layoutTimer;

    [SerializeField] private Fader Fader;
    [SerializeField] private float fadeDuration;

    [SerializeField] private GameObject wincondition;
    [SerializeField] private GameObject Bg;
    private void Start()
    {

    }
    public void Condition()
    {
        layoutTimer.gameObject.SetActive(false);
        wincondition.gameObject.SetActive(true);
        Bg.gameObject.SetActive(true);

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

        Debug.Log("win");
    }
}
