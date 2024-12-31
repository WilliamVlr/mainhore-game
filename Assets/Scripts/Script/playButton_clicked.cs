using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playButton_clicked : MonoBehaviour
{
    public Sprite normalSprite; // Assign in the Inspector
    public Sprite clickedSprite; // Assign in the Inspector
    public SpriteRenderer myButtonSpriteRenderer; // Renamed to avoid conflicts
    private int isClicked = 0;

    public GameObject pauseInterface, pauseButton;


    void Start()
    {
        // Ensure all components are assigned
        if (myButtonSpriteRenderer == null)
            myButtonSpriteRenderer = GetComponent<SpriteRenderer>();

        myButtonSpriteRenderer.sprite = normalSprite;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("PlayButton"))
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

            if (hit.collider != null && hit.collider.CompareTag("PlayButton"))
            {
                isClicked++;
            }
        }

        if (isClicked == 2)
        {
            Debug.Log("Play...");

            pauseInterface.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);
            Time.timeScale = 1; // Set the time scale to zero after animation
            isClicked = 0;
        }
    }
}
