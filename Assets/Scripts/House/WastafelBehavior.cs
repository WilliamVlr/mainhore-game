using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WastafelBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform transformer;

    // The Y value where the fall should stop (e.g., -2.6)
    private float stopFallAtY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        transformer = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

        // Disable physics by default
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // Freeze rotation on the Z-axis to prevent the object from rotating
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        // If the Y position is below the threshold, hold it at that Y value
        if (transformer.position.y <= stopFallAtY)
        {
            // Hold the position at the specified Y value
            transformer.position = new Vector2(transform.position.x, stopFallAtY);

            // Optionally, stop physics interactions
            rb.isKinematic = true;
            //rb.velocity = Vector2.zero; // Optional: prevent residual velocity if needed
        }
    }
}
