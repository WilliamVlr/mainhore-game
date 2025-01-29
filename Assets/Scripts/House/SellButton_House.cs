using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellButton_House : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite clickedSprite;
    private int isClicked;
    private Collider2D myCollider;
    FurnitureBehavior furniture;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure that the object has a collider attached to it
        myCollider = GetComponent<Collider2D>();
        if (myCollider == null)
        {
            Debug.LogError("No Collider found on the GameObject! Please add one.");
        }

        furniture = GetComponentInParent<FurnitureBehavior>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (furniture != null)
        {
            Debug.Log("The parent is " + furniture.furnitureData.itemName);
        }
    }

    void Update()
    {
        // Handle Mouse or Touch input based on the platform
        if (Input.GetMouseButtonDown(0)) // Mouse click (or touch on mobile)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider == myCollider)
            {
                isClicked++;
                spriteRenderer.sprite = clickedSprite;
            }
        }

        if (Input.GetMouseButtonUp(0)) // Mouse click (or touch on mobile)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider == myCollider)
            {
                isClicked++;
                spriteRenderer.sprite = defaultSprite;
            }
        }

        if(isClicked == 2)
        {
            //Add functionality here

            isClicked = 0;
        }

    }
}
