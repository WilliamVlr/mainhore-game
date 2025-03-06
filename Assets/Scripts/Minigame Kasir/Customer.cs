using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] float moveSpeed = 9f;
    [SerializeField] float frequency = 20f;
    [SerializeField] float magnitude = 0.5f;

    private Vector2 _startPos, _centerPos, _doorPos, _startScale, _endScale;
    private Vector2 camPos;
    private Vector3 currentPos;
    private bool status = false;
    private void Start()
    {
        camPos = Camera.main.ViewportToWorldPoint(new Vector2(1.05f, 0.5f));
        _startPos = transform.InverseTransformPoint(camPos);
        camPos = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
        _centerPos = transform.InverseTransformPoint(camPos);
        camPos = Camera.main.ViewportToWorldPoint(new Vector2(0.1f, 0.7f));
        _doorPos = transform.InverseTransformPoint(camPos);

        _startScale = new Vector2(3f, 3f);
        _endScale = new Vector2(1.2f, 1.2f);

        currentPos = _startPos;

        transform.localPosition = _startPos;
        transform.localScale = _startScale;
    }


    private void Update()
    {
        if(status == false)  walkIn();
        if(status == true) walkOut();
    }

    private void walkIn()
    {   
        if(currentPos.x > _centerPos.x) 
        {
            currentPos -= moveSpeed * Time.deltaTime * transform.right;
            transform.localPosition = currentPos + magnitude * Mathf.Sin(Time.time * frequency) * transform.up;
        }
        else{
            status = true;
        }
    }

    private void walkOut()
    {
        if(currentPos.x > _doorPos.x)
        {   
            currentPos = Vector3.MoveTowards(currentPos, _doorPos, moveSpeed*Time.deltaTime);
            transform.localPosition = currentPos;
            transform.localScale = _endScale;
        }
        else
        {
            Destroy(gameObject);
        }

    }
}   
