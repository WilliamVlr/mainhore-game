using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirusMove : MonoBehaviour
{
    public float speed;
    private float targetX;
    private float targetY;
    int check = -1;
    GameObject preRoundTimer;
    int firstTarget = 0;
    // Start is called before the first frame update
    void Start()
    {
        preRoundTimer = GameObject.FindWithTag("preRoundTimer");
        // Initial target position is random
        //check = SetNewTarget(check);
    }

    // Update is called once per frame
    void Update()
    {
        if(firstTarget == 0)
        {
            check = SetNewTarget(check);
            firstTarget++;
        }
        Text timerText = preRoundTimer.GetComponent<Text>();
        // Move towards the target position
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = new Vector2(targetX, targetY);

        // Move towards the target position using linear interpolation (Lerp)
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        // Once the object reaches the target position, set a new random target
        if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
        {
            check = SetNewTarget(check);
        }
    }

    int SetNewTarget(int _chk)
    {
        int temp;
        Vector3 currentScale = transform.localScale;
        // Randomize y position
        targetY = Random.Range(-4f, 4f);
        if (_chk == 0) // di kiri
        {
            targetX = 8f;
            currentScale.x = 0.3f;
            temp = 1;

        } else if( _chk == 1) // di kanan
        {
            targetX = -8f;
            currentScale.x = -0.3f;
            temp = 0;
        }
        else
        {
            targetX = 0;
            if (Random.Range(0, 2) == 0)
            {
                targetX = -8f;
                currentScale.x = -0.3f;
                temp = 0;
            }
            else
            {
                targetX = 8f; 
                currentScale.x = 0.3f;
                temp = 1;
            }
        }

        transform.localScale = currentScale;
        return temp;
    }
}
