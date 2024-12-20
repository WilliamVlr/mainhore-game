using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeItemSpawner : MonoBehaviour
{
    public GameObject foodPrefab; // Reference to the food prefab
    private Camera mainCamera; // Reference to the main camera
    public static bool isOriginalFoodClicked = false; // Static variable to track if the original food is clicked

    private void Start()
    {
        mainCamera = Camera.main; // Get the main camera
    }

    private void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Check if the touch started on this GameObject
                    //Debug.Log("Masuk Touch Phase Began");
                    Vector2 touchPosition = mainCamera.ScreenToWorldPoint(touch.position);
                    Collider2D hitCollider = Physics2D.OverlapPoint(touchPosition);

                    // Check if the touch started on this GameObject
                    if (hitCollider != null && hitCollider.gameObject == gameObject)
                    {
                        isOriginalFoodClicked = true; // Set the flag to true
                        SpawnFood(); // Spawn a new food item
                        Debug.Log($"{gameObject.name} clicked and food spawned.");
                    }
                    break;
            }
        }
        else
        {
            // Check for mouse input as a fallback
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Got Mouse click");
                // Convert mouse position to world point
                Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);
                //Debug.Log($"{mousePosition}, ");
                //Debug.Log($"{hitCollider}, ");
                //Debug.Log($"{hitCollider.gameObject}");

                // Check if the mouse is over this GameObject
                if (hitCollider != null && hitCollider.gameObject == gameObject)
                {
                    //Debug.Log("Masuk ifnya");
                    isOriginalFoodClicked = true; // Set the flag to true
                    SpawnFood(); // Spawn a new food item
                    //Debug.Log($"{gameObject.name} clicked and food spawned.");
                }
            }
        }
    }

    private void SpawnFood()
    {
        //Debug.Log($"{transform.position}, {Quaternion.identity}");
        // Instantiate a new food item at the original position
        Instantiate(foodPrefab, transform.position, Quaternion.identity);
    }

}
