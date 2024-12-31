using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class storeButton_clicked : MonoBehaviour
{
    public GameObject timerObject; // Assign in the Inspector
    public Sprite normalSprite; // Assign in the Inspector
    public Sprite clickedSprite; // Assign in the Inspector
    public SpriteRenderer myButtonSpriteRenderer; // Renamed to avoid conflicts
    private int isClicked = 0;
    private int isDone = 0;

    public GameObject gameManager; // Assign in the Inspector
    private WinLoseCondition winLoseCondition;

    void Start()
    {
        // Ensure all components are assigned
        if (myButtonSpriteRenderer == null)
            myButtonSpriteRenderer = GetComponent<SpriteRenderer>();

        myButtonSpriteRenderer.sprite = normalSprite;

        if (gameManager != null)
            winLoseCondition = gameManager.GetComponent<WinLoseCondition>();
    }

    void Update()
    {
        if (winLoseCondition != null)
            isDone = winLoseCondition.getGameDoneCheck();

        if (Input.GetMouseButtonDown(0) && isDone == 1)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("StoreButton"))
            {
                myButtonSpriteRenderer.sprite = clickedSprite;
                isClicked++;
            }
        }

        if (Input.GetMouseButtonUp(0) && isDone == 1)
        {
            myButtonSpriteRenderer.sprite = normalSprite;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("StoreButton"))
            {
                isClicked++;
            }
        }

        if (isClicked == 2)
        {
            Debug.Log("Going Home...");
            SceneManager.LoadScene("STORE");
        }
    }
}
