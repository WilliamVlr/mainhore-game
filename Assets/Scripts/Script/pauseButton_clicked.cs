using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pauseButton_clicked : MonoBehaviour
{
    public Sprite normalSprite; // Assign in the Inspector
    public Sprite clickedSprite; // Assign in the Inspector
    public SpriteRenderer myButtonSpriteRenderer; // Renamed to avoid conflicts
    private int isClicked = 0;

    public GameObject pauseInterface, pauseButton;
    public Fader Fader;

    void Start()
    {
        Debug.Log("START");
        // Ensure all components are assigned
        if (myButtonSpriteRenderer == null)
            myButtonSpriteRenderer = GetComponent<SpriteRenderer>();

        myButtonSpriteRenderer.sprite = normalSprite;

        pauseInterface.gameObject.SetActive(false);
        StartCoroutine(Fader.FadeOutGameObject(pauseInterface, (float)0.1));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("PauseButton"))
            {
                myButtonSpriteRenderer.sprite = clickedSprite;
                isClicked++;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            myButtonSpriteRenderer.sprite = normalSprite;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("PauseButton"))
            {
                isClicked++;
            }
        }

        if (isClicked == 2)
        {
            Debug.Log("Pause...");

            StartCoroutine(PauseGameWithAnimation());
            isClicked = 0;
        }
    }

    private IEnumerator PauseGameWithAnimation()
    {
        pauseInterface.gameObject.SetActive(true);

        // Wait for the fade-in animation to complete
        yield return StartCoroutine(Fader.FadeInGameObject(pauseInterface, 0.1f));
        pauseButton.gameObject.SetActive(false);

        Time.timeScale = 0; // Set the time scale to zero after animation
    }
}
