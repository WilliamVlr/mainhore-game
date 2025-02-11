using System.Collections;
using UnityEngine;

public class VirusMove : MonoBehaviour
{
    public float speed;
    private Vector2 targetPosition;
    private int direction;

    void Start()
    {
        SetNewTarget();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTarget();
        }
    }

    void SetNewTarget()
    {
        Camera cam = Camera.main;
        Vector3 maxPosition = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        Vector3 minPosition = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));

        targetPosition.y = Random.Range(minPosition.y + 1f, maxPosition.y - 1f);

        direction = (direction == 0) ? 1 : 0; // Toggle direction
        targetPosition.x = (direction == 0) ? minPosition.x + 1.5f : maxPosition.x - 1.5f;
        transform.localScale = new Vector3((direction == 0) ? -0.3f : 0.3f, transform.localScale.y, transform.localScale.z);
    }
}
