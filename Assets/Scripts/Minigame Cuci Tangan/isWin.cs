using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class isWin : MonoBehaviour
{
    public Text roundText;

    public Fader Fader;
    public float fadeDuration;

    public GameObject wincondition;
    public void Start()
    {

    }
    public void Condition()
    {
        wincondition.gameObject.SetActive(true);

        if (Fader != null)
        {
            // Use the FadeOutGameObject coroutine
            StartCoroutine(Fader.FadeInGameObject(wincondition, fadeDuration));
        }
        else
        {
            Debug.LogError("spriteFader is not assigned!");
        }

        Debug.Log("win");
    }
}
