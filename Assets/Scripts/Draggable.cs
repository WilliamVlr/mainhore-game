using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Draggable : MonoBehaviour
{
    private bool isDragging = false; // To track if the object is being dragged
    private Camera mainCamera; // Reference to the main camera
    public SO_item itemSO;
    public UnityEvent onDrop;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main; // Get the main camera
    }

    // Update is called once per frame
    void Update()
    {
        //Dragging logic
        if (isDragging)
        {
            // Update the position of the dragged object
            Vector3 touchPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = 0; // Set z to 0 for 2D
            transform.position = touchPosition;
        }

        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Check if the touch started on this GameObject
                    if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touch.position))
                    {
                        isDragging = true; // Start dragging
                    }
                    break;

                case TouchPhase.Ended:
                    if (isDragging)
                    {
                        isDragging = false; // Stop dragging
                        onDrop?.Invoke();
                    }
                    break;
            }
        }
        else
        {
            // Check for mouse input as a fallback
            if (Input.GetMouseButtonDown(0))
            {
                if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
                {
                    isDragging = true; // Start dragging
                }
            }

            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                isDragging = false; // Stop dragging
                onDrop?.Invoke();
            }
        }

        // Allow dragging if the original food was clicked
        if (CafeItemSpawner.isOriginalFoodClicked && !isDragging)
        {
            isDragging = true; // Start dragging immediately if the original food was clicked
            CafeItemSpawner.isOriginalFoodClicked = false; // Reset the flag after starting to drag
        }
    }
}
