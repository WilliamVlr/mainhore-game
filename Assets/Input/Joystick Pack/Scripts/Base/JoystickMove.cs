using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JoystickMove : MonoBehaviour
{
    public Joystick movementJoystick;
    public GameObject background;
    public PlayerDirection _playerdirection;

    private Rigidbody2D rb_player, rb_background;

    Vector2 backgroundPosition;
    Vector2 playerPosition;

    int isBgMoving = 0;
    int notmid = 0;
    int maxLR = 0;

    public float playerSpeed;

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

        _playerdirection.setDirection(movementJoystick);
        
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
            checkMovY();
            rb_background.velocity = new Vector2(-movementJoystick.Direction.x * playerSpeed, 0);
        }
        else
        {
            checkMovX();
            rb_background.velocity = new Vector2(0, 0);
        }
    }

    void checkMovX()
    {
        if (playerPosition.x >= 7.5)
        {
            if (movementJoystick.Direction.x <= 0)
            {
                rb_player.velocity = new Vector2(movementJoystick.Direction.x * playerSpeed, 0);
                maxLR = 0;
                checkMovY();
            }
            else
            {
                maxLR = 1;
                checkMovY();
            }

        }
        else if (playerPosition.x <= -7.5)
        {
            if (movementJoystick.Direction.x >= 0)
            {
                rb_player.velocity = new Vector2(movementJoystick.Direction.x * playerSpeed, 0);
                maxLR = 0;
                checkMovY();
            }
            else
            {
                maxLR = 1;
                checkMovY();
            }
        }
        else
        {
            rb_player.velocity = new Vector2(movementJoystick.Direction.x * playerSpeed, 0);
            checkMovY();
        }
    }

    void checkMovY()
    {
        Vector2 currentVelocity;
        if (isBgMoving == 1 || maxLR == 1)
        {
            currentVelocity = new Vector2(0, 0);
        }
        else
        {
            currentVelocity = rb_player.velocity; // Ambil kecepatan saat ini
        } 
        float newYVelocity = 0;

        if (playerPosition.y >= -1.55)
        {
            if (movementJoystick.Direction.y <= 0)
            {
                newYVelocity = movementJoystick.Direction.y * playerSpeed;
            }
            else
            {
                newYVelocity = 0;
            }
        }
        else if (playerPosition.y <= -4.25)
        {
            if (movementJoystick.Direction.y >= 0)
            {
                newYVelocity = movementJoystick.Direction.y * playerSpeed;
            }
            else
            {
                newYVelocity = 0;
            }
        }
        else
        {
            newYVelocity = movementJoystick.Direction.y * playerSpeed;
        }

        // Tetapkan hanya komponen Y, pertahankan komponen X
        rb_player.velocity = new Vector2(currentVelocity.x, newYVelocity);
    }
}
