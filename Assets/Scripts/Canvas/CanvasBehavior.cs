using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBehavior : MonoBehaviour
{
    public bool hideInStart;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        //if (hideInStart)
        //{
        //    hideCanvas();
        //} 
        //else
        //{
        //    showCanvas();
        //}
    }

    public void showCanvas()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void hideCanvas()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
