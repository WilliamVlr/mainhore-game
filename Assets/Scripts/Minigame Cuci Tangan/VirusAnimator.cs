using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VirusAnimator : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    // Function to pause the animator
    public void PauseAnimator()
    {
        if (animator != null)
        {
            animator.speed = 0; // Pause the animation
        }
    }

    // Function to resume the animator
    public void PlayAnimator()
    {
        if (animator != null)
        {
            animator.speed = 1; // Resume the animation
        }
    }
}
