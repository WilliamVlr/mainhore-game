using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPatrol : MonoBehaviour
{
    public static NpcPatrol Instance;

    public GameObject pointA;
    public GameObject pointB;
    [SerializeField] private GameObject NpcUI;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 currentPoint;
    [SerializeField] private float speed;
    [SerializeField] private float acceleration = 1f;
    private float currentSpeed = 0f;

    [SerializeField] private float stopChance = 0.1f;
    [SerializeField] private float minStopDuration = 1f;
    [SerializeField] private float maxStopDuration = 3f;

    [SerializeField] private bool isStopped = false;
    private float stopTimer = 0f;

    private float delayAfterPause = 0f;

    public bool IsStopped()
    {
        return isStopped;
    }

    public void IsStopped(bool a)
    {
        isStopped = a;
    }

    public float CurrentSpeed => currentSpeed;

    void Start()
    {
        rb = NpcUI.GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        anim = NpcUI.GetComponent<Animator>();
        currentPoint = pointB.transform.localPosition; // Gunakan localPosition
    }

    void Update()
    {
        //Debug.Log((int)delayAfterPause);
        if ((InteractionManager.Instance.getInteract()) == 1)
        {
            //Debug.Log("Manual");
            //isStopped = true;
            if (stopTimer > 0f)
            {
                stopTimer = 0f;
            }
            delayAfterPause = 3f;
        }

        if (delayAfterPause <= 0f)
        {
            if (!isStopped && Random.Range(0f, 1f) < stopChance * Time.deltaTime)
            {
                //Debug.Log("Auto");
                isStopped = true;
                currentSpeed = Mathf.Lerp(currentSpeed, 0, acceleration * Time.deltaTime);
                stopTimer = Random.Range(minStopDuration, maxStopDuration);
                rb.velocity = new Vector2(currentSpeed, 0);
                return;
            }
        }
        else
        {
            delayAfterPause -= Time.deltaTime;
        }

        if (isStopped)
        {
            stopTimer -= Time.deltaTime;
            if (stopTimer <= 0 && (InteractionManager.Instance.getInteract()) == 0)
            {
                isStopped = false;
            }
            else
            {
                if (currentSpeed != 0)
                {
                    currentSpeed = Mathf.Lerp(currentSpeed, 0, 4 * acceleration * Time.deltaTime);
                    rb.velocity = new Vector2(currentSpeed, 0);
                    if (currentSpeed < 0.5f)
                    {
                        rb.velocity = Vector2.zero;
                    }
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
                return;
            }
        }
        else
        {
            if (currentPoint == pointB.transform.localPosition)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, speed, 4 * acceleration * Time.deltaTime);
            }
            else
            {
                currentSpeed = Mathf.Lerp(currentSpeed, -speed, 4 * acceleration * Time.deltaTime);
            }
        }
        
        if (currentPoint == pointB.transform.localPosition)
        {
            rb.velocity = new Vector2(currentSpeed, 0);
            if (currentSpeed > 0)
            {
                NpcUI.transform.localScale = new Vector2(3f, 3f);
            }
        }
        else
        {
            rb.velocity = new Vector2(currentSpeed, 0);
            if (currentSpeed < 0)
            {
                NpcUI.transform.localScale = new Vector2(-3f, 3f);
            }
        }

        if (Vector2.Distance(pointA.transform.localPosition, NpcUI.transform.localPosition) < 0.5f && currentPoint == pointB.transform.localPosition)
        {
            currentPoint = pointA.transform.localPosition;
        }
        if (Vector2.Distance(pointB.transform.localPosition, NpcUI.transform.localPosition) < 0.5f && currentPoint == pointA.transform.localPosition)
        {
            currentPoint = pointB.transform.localPosition;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
