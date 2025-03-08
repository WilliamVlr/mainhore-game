using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void Start()
    {
        float height = GetComponent<SpriteRenderer>().bounds.size.y;
        float width = GetComponent<SpriteRenderer>().bounds.size.x;
        Vector3 topLeftWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, 20)) + new Vector3(width/2, -height/2, 0);
        transform.position = topLeftWorldPoint;
    }
}
