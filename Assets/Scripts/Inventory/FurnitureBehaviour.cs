using UnityEngine;
using static UnityEditor.Progress;

public class FurnitureBehavior : MonoBehaviour
{
    public SO_Furniture furnitureData; // Reference to the furniture's ScriptableObject

    private bool isDragging = false;
    private bool draggingEnabled;
    private Camera mainCamera;
    private Vector3 originalPosition;   // Store the original position when the item is first placed

    private Rigidbody2D rb;
    private Transform transformer;
    private SpriteRenderer spriteRenderer;

    // The Y value where the fall should stop (e.g., -2.6)
    private float stopFallAtY = -2.6f;

    public void Initialize(SO_Furniture item)
    {
        furnitureData = item;
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Disable physics by default
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        // Record the original position when the furniture is placed
        originalPosition = transform.position;
        transformer = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Disable physics by default
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // Freeze rotation on the Z-axis to prevent the object from rotating
        rb.freezeRotation = true;
    }

    void Update()
    {
        HouseManager house = FindAnyObjectByType<HouseManager>();
        if(house != null )
        {
            draggingEnabled = house.IsInDecorationMode;
        }

        if(draggingEnabled)
        {
            // Handle dragging logic
            if (isDragging)
            {
                Vector3 touchPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                touchPosition.z = 0;  // Set Z to 0 for 2D view
                transform.position = touchPosition;
            }

            // Handle touch/mouse input for dragging
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);  // Get the first touch

                if (touch.phase == TouchPhase.Began)
                {
                    // Start dragging if the player touches the furniture
                    if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touch.position))
                    {
                        isDragging = true;
                        rb.isKinematic = true;

                        // Set the sorting order to be above the object below it
                        AdjustSortingOrder();
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    // End the drag and check for overlap when the touch ends
                    if (isDragging)
                    {
                        isDragging = false;
                        rb.isKinematic = false;
                        CheckForOverlappingFurniture();
                    }
                }
            }

            // Handle mouse input for dragging (for testing on desktop)
            if (Input.GetMouseButtonDown(0))
            {
                if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
                {
                    isDragging = true;
                    rb.isKinematic = true;

                    // Set the sorting order to be above the object below it
                    AdjustSortingOrder();
                }
            }
            else if (Input.GetMouseButtonUp(0) && isDragging)
            {
                isDragging = false;
                rb.isKinematic = false;
                CheckForOverlappingFurniture();
            }
        }

        if (!isDragging)
        {
            // If the Y position is below the threshold, hold it at that Y value
            if (transformer.position.y <= stopFallAtY)
            {
                // Hold the position at the specified Y value
                transformer.position = new Vector2(transform.position.x, stopFallAtY);

                // Optionally, stop physics interactions
                rb.isKinematic = true;
                //rb.velocity = Vector2.zero; // Optional: prevent residual velocity if needed

                // Reset sorting order when it reaches the stop position
                ResetSortingOrder();
            }
        }
    }

    // Adjust the sorting order to always be above the object below
    private void AdjustSortingOrder()
    {
        // Find the object beneath this furniture item
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f); // Adjust radius as necessary
        int highestSortingOrder = 0;

        // Check for any other furniture objects and get the highest sorting order
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                SpriteRenderer otherRenderer = collider.GetComponent<SpriteRenderer>();
                if (otherRenderer != null)
                {
                    highestSortingOrder = Mathf.Max(highestSortingOrder, otherRenderer.sortingOrder);
                }
            }
        }

        // Set this furniture's sorting order to be 1 above the highest one found
        spriteRenderer.sortingOrder = highestSortingOrder + 1;
    }

    // Reset sorting order when the furniture reaches the stop position
    private void ResetSortingOrder()
    {
        // Reset the sorting order to the default or desired value
        spriteRenderer.sortingOrder = 0;
    }

    // Check for overlap with other furniture when dropped
    private void CheckForOverlappingFurniture()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);  // Adjust radius as necessary
        bool canPlace = true;

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                // If it overlaps, prevent placement
                canPlace = false;
                break;
            }
        }

        // If an overlap was detected, snap back to original position
        if (!canPlace)
        {
            transform.position = originalPosition;
        }
    }

}
