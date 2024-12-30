using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class isLose : MonoBehaviour
{
    public Text roundText;

    public Fader Fader;
    public float fadeDuration;

    public GameObject losecondition;
    public void Start()
    {

    }
    public void Condition()
    {
        losecondition.gameObject.SetActive(true);

        if (Fader != null)
        {
            // Use the FadeOutGameObject coroutine
            StartCoroutine(Fader.FadeInGameObject(losecondition, fadeDuration));
        }
        else
        {
            Debug.LogError("spriteFader is not assigned!");
        }

        Debug.Log("lose");
    }
}
