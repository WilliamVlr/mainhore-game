using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PackButtonBehavior : MonoBehaviour
{
    FurnitureBehavior furniture;
    private Collider2D myCollider;

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

        if(furniture != null )
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

            if(hit.collider == myCollider)
            {
                //Debug.Log("Masuk ke if kedua ray hits any collider");
                // Check if the hit object is this object (i.e. the one this script is attached to)
                if (hit.collider == myCollider)
                {
                    Debug.Log("Clicked on the GameObject: " + gameObject.name);
                    Debug.Log(gameObject.name + " for " + furniture.furnitureData.itemName);
                    // Add your logic here for when the object is clicked
                }
            }
        }
    }
}
