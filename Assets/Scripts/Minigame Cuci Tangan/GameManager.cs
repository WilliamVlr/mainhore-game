using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject roundText;
    public Text preRoundText;

    public GameObject background, air, gelap;

    public float fadeDuration = 1.0f; // Duration of the fade effect

    private Fader Fader;

    public GameObject win, lose;
    public GameObject PauseButton, PauseInterface;

    int firstcheck = 0;

    void Start()
    {
        Fader = FindObjectOfType<Fader>();

        // roundText.enabled = false;
        roundText.gameObject.SetActive(false);

        win.gameObject.SetActive(false);
        lose.gameObject.SetActive(false);
        PauseButton.gameObject.SetActive(false);
        air.gameObject.SetActive(false);
        PauseInterface.gameObject.SetActive(false);

        StartCoroutine(Fader.FadeOutGameObject(win, fadeDuration));
        StartCoroutine(Fader.FadeOutGameObject(lose, fadeDuration));
        StartCoroutine(Fader.FadeOutGameObject(PauseButton, 0.3f)); 
    }

    // Update is called once per frame
    void Update()
    {
        if (preRoundText.text == "GO!" && firstcheck == 0)
        {
            firstcheck++;
            preRoundText.enabled = false;
            roundText.gameObject.SetActive(true);
            // StartCoroutine(spriteFader.FadeInText(roundText, fadeDuration));

            if (Fader != null)
            {

                StartCoroutine(Fader.FadeOutGameObject(gelap, 0.3f));

                PauseButton.gameObject.SetActive(true);
                StartCoroutine(Fader.FadeInGameObject(PauseButton, 0.3f));
                air.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("spriteFader is not assigned!");
            }
        }
    }
}
