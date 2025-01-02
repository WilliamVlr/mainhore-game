using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButtonIns : MonoBehaviour
{
    public GameObject InstructionLayout, InstructionParent;
    public void CloseButtonCuciTangan()
    {
        InstructionLayout.gameObject.SetActive(false);
        InstructionParent.gameObject.SetActive(true);
    }
}
