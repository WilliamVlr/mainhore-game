using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationManager : MonoBehaviour
{
    public ConfirmationBehavior confirmationPanel;
    // Start is called before the first frame update
    void Start()
    {
        confirmationPanel.gameObject.SetActive(false);
    }
}
