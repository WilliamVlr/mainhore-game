using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mulaiButton_clicked : MonoBehaviour
{
    public Sprite normalSprite; // Assign in the Inspector
    public Sprite clickedSprite; // Assign in the Inspector
    public SpriteRenderer myButtonSpriteRenderer; // Renamed to avoid conflicts
    private int isClicked = 0, timeDef;

    public GameObject PreRoundTimer, instruksi;
    preRoundTimer preroundtimer_script;

    void Start()
    {
        preroundtimer_script = PreRoundTimer.GetComponent<preRoundTimer>();
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

            if (hit.collider != null && hit.collider.CompareTag("mulaiButton"))
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

            if (hit.collider != null && hit.collider.CompareTag("mulaiButton"))
            {
                isClicked++;
            }
        }

        if (isClicked == 2)
        {
            Debug.Log("Play...");

            instruksi.gameObject.SetActive(false);
            PreRoundTimer.gameObject.SetActive(true);
            preroundtimer_script.timeDeficit = 1;

            isClicked = 0;
        }
    }
}
