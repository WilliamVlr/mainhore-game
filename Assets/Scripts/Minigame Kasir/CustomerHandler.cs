using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHandler : MonoBehaviour
{
    [SerializeField] public Customer[] prefabs;
    public void spawnCustomer()
    {   
        Instantiate(prefabs[Random.Range(0, prefabs.Length)], this.transform);
    }
}
