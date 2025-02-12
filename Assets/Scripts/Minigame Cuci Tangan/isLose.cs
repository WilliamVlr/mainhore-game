using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class isLose : MonoBehaviour
{
    [SerializeField] private GameObject layoutTimer;

    [SerializeField] private Fader Fader;
    [SerializeField] private float fadeDuration;

    [SerializeField] private GameObject losecondition;
    [SerializeField] private GameObject Bg;
    private void Start()
    {

    }
    public void Condition()
    {
        Time.timeScale = 0;
        layoutTimer.gameObject.SetActive(false);
        losecondition.gameObject.SetActive(true);
        Bg.gameObject.SetActive(true);

        if (Fader != null)
        {
            // Use the FadeOutGameObject coroutine
            StartCoroutine(Fader.FadeInGameObject(Bg, fadeDuration));
            StartCoroutine(Fader.FadeInGameObject(losecondition, fadeDuration));
        }
        else
        {
            Debug.LogError("spriteFader is not assigned!");
        }

        //Debug.Log("lose");
    }
}
