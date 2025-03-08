using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegister : MonoBehaviour
{
    void Start()
    {        
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.24f, 0.37f, 10f));
    }

}
