using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private SO_item item;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if(spriteRenderer.sprite != null)
        {
            spriteRenderer.sprite = null;
        }

        spriteRenderer.sprite = item.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
