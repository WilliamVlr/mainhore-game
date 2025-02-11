using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotifPanelBehavior : MonoBehaviour
{
    private CanvasGroup notifCanvasGroup;

    public Button yesBtn;

    private void Awake()
    {
        notifCanvasGroup = transform.parent.transform.parent.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if(yesBtn != null)
        {
            yesBtn.onClick.RemoveAllListeners();
            yesBtn.onClick.AddListener(hideCanvas);
        }
        hideCanvas();
    }

    public void showCanvas()
    {
        notifCanvasGroup.alpha = 1;
        notifCanvasGroup.interactable = true;
        notifCanvasGroup.blocksRaycasts = true;
    }

    public void hideCanvas()
    {
        notifCanvasGroup.alpha = 0;
        notifCanvasGroup.interactable = false;
        notifCanvasGroup.blocksRaycasts = false;
    }
}
