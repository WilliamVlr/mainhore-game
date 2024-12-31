using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EaseOutYMovement : MonoBehaviour
{
    public float startY = 0f;
    public float endY = 5f;
    public float duration = 2f;

    public void easeOut(GameObject obj)
    {
        StartCoroutine(EaseOutCoroutine(obj));
    }

    private IEnumerator EaseOutCoroutine(GameObject obj)
    {
        float elapsedTime = 0f;

        Vector3 startPos = new Vector3(obj.transform.position.x, startY, obj.transform.position.z);
        Vector3 endPos = new Vector3(obj.transform.position.x, endY, obj.transform.position.z);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Ease-out interpolation
            float t = elapsedTime / duration;
            t = t * (2 - t); // Ease-out formula

            // Update the position
            obj.transform.position = Vector3.Lerp(startPos, endPos, t);

            yield return null; // Wait for the next frame
        }

        // Ensure the object reaches the exact target position
        obj.transform.position = endPos;
    }

}