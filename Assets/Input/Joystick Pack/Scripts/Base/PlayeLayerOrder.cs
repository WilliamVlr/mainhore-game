using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeLayerOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition.y <= -7.85)
        {
            spriteRenderer.sortingOrder = 5;
        } 
        else
        {
            spriteRenderer.sortingOrder = 2;
        }
    }
}
