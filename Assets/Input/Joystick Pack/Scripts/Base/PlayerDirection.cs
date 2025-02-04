using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirection : MonoBehaviour
{
    public GameObject player;
    public void setDirection(Joystick movementJoystick)
    {
        Vector3 currentScale = player.transform.localScale; // Get the current scale
        currentScale.x = -Mathf.Abs(currentScale.x);
        if (movementJoystick.Direction.x < 0)
        {
            currentScale.x = -Mathf.Abs(currentScale.x);
        } else if(movementJoystick.Direction.x > 0)
        {
            currentScale.x = Mathf.Abs(currentScale.x);
        }
        player.transform.localScale = currentScale;
    }
}
