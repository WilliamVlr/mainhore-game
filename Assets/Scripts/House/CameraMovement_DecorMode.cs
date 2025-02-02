using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement_DecorMode : MonoBehaviour
{
    public float moveSpeed = 1.0f; // Speed of the camera's movement
    public float maxLeftThreshold = 20.0f; // Maximum distance the camera can move to the left
    public float maxRightThreshold = 15.0f; // Maximum distance the camera can move to the right
    private Vector3 currentPosition; // The camera's current position for smooth movement

    private float touchStartPosX = 0f; // To store the initial touch position on the X axis

    // Reference to the character's transform
    public Transform characterTransform;
    private HouseManager houseManager;

    // Reference to the background's transform
    public Transform backgroundTransform;

    void Start()
    {
        houseManager = FindAnyObjectByType<HouseManager>();
        if ( houseManager == null )
        {
            Debug.Log("House manager not found from CameraMovement");
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isInDecorationMode = houseManager.IsInDecorationMode;

        if ( isInDecorationMode )
        {
            if(!houseManager.isFurnitureBeingDragged)
            {
                HandleCameraMovement();
            }
        }
    }

    // Call this function when the player enters decoration mode
    public void onEnterDecorationMode()
    {
        // Save the camera's current position for smooth movement
        currentPosition = transform.position;

        // Dynamically calculate the left and right thresholds based on the background's position
        maxLeftThreshold = backgroundTransform.position.x - 20; // Left threshold is the current position of the background
        maxRightThreshold = backgroundTransform.position.x + 15; // Right threshold is the background's position plus its width
    }

    // Call this function when the player exits decoration mode
    public void onExitDecorationMode()
    {
        FocusOnCharacter(); // Focus the camera back on the character when exiting decoration mode
    }

    private void HandleCameraMovement()
    {
        // Detect touch input on mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch

            if (touch.phase == TouchPhase.Began)
            {
                // When the touch starts, store the initial X position
                touchStartPosX = touch.position.x;
            }

            // Calculate how much the player has moved their finger horizontally
            float touchDeltaX = touch.position.x - touchStartPosX;

            // Invert the direction of the camera's movement
            touchDeltaX = -touchDeltaX;

            // Calculate the new target X position based on touch input
            float targetXPosition = currentPosition.x + touchDeltaX * moveSpeed * Time.deltaTime;

            // Limit the camera's movement using the absolute thresholds
            if (targetXPosition < maxLeftThreshold)
            {
                targetXPosition = maxLeftThreshold; // Limit to the left
            }
            else if (targetXPosition > maxRightThreshold)
            {
                targetXPosition = maxRightThreshold; // Limit to the right
            }

            // Update the camera's position to the new target position
            currentPosition.x = targetXPosition;

            // Update the camera's position with smooth movement
            transform.position = new Vector3(currentPosition.x, transform.position.y, transform.position.z);
        }
    }


    // Focus the camera back to the character when exiting decoration mode
    private void FocusOnCharacter()
    {
        if (characterTransform != null)
        {
            transform.position = new Vector3(characterTransform.position.x, transform.position.y, transform.position.z);
        }
    }


}
