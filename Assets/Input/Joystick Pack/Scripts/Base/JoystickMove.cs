using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JoystickMove : MonoBehaviour
{
    public Joystick movementJoystick;
    public GameObject background;
    public float playerSpeed;
    private Rigidbody2D rb_player, rb_background;
    Vector2 backgroundPosition;
    Vector2 playerPosition;
    int isBgMoving = 0;
    int notmid = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb_player = GetComponent<Rigidbody2D>();
        rb_background = background.GetComponent<Rigidbody2D>();
        notmid = 0;
    }

    private void FixedUpdate()
    {
        backgroundPosition = background.transform.position;
        playerPosition = this.transform.position;

        
        if(backgroundPosition.x <= -3 || backgroundPosition.x >= 3)
        {
            if (Mathf.Abs(playerPosition.x) < 0.3 && notmid == 1)
            {
                isBgMoving = 1;
                notmid = 0;
                //Debug.Log(notmid);
            }
            else
            {
                isBgMoving = 0; // gaboleh gerak
                notmid = 1;
                //Debug.Log(notmid);
            }
        }
        else
        {
            isBgMoving = 1;
        }

        if (isBgMoving == 1)
        {
            rb_player.velocity = new Vector2(0, movementJoystick.Direction.y * playerSpeed);
            rb_background.velocity = new Vector2(-movementJoystick.Direction.x * playerSpeed, 0);
        }
        else
        {
            if(playerPosition.x >= 7.5)
            {
                if(movementJoystick.Direction.x <= 0)
                {
                    rb_player.velocity = new Vector2(movementJoystick.Direction.x * playerSpeed, movementJoystick.Direction.y * playerSpeed);
                }
                else
                {
                    rb_player.velocity = new Vector2(0, movementJoystick.Direction.y * playerSpeed);
                }

            } 
            else if (playerPosition.x <= -7.5)
            {
                if (movementJoystick.Direction.x >= 0)
                {
                    rb_player.velocity = new Vector2(movementJoystick.Direction.x * playerSpeed, movementJoystick.Direction.y * playerSpeed);
                }
                else
                {
                    rb_player.velocity = new Vector2(0, movementJoystick.Direction.y * playerSpeed);
                }
            }
            else
            {
                rb_player.velocity = new Vector2(movementJoystick.Direction.x * playerSpeed, movementJoystick.Direction.y * playerSpeed);
            }
            rb_background.velocity = new Vector2(0, 0);
        }
    }
}
