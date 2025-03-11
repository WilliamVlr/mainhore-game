using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenjagaTokoBehavior : MonoBehaviour
{
    [SerializeField] private CanvasBehavior dialogCanvas;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogCanvas.showCanvas();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        dialogCanvas.hideCanvas();
    }
}
