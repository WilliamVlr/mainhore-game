using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcRandomPosition : MonoBehaviour
{
    int random;
    [SerializeField] private int l,r;
    private void Awake()
    {
        random = (int)Random.Range(l, r + 1);
        transform.position = new Vector2(random, -2.7f);
    }
}
