using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JoystickMove : MonoBehaviour
{
    public Joystick movementJoystick;
    public GameObject background;
    public float backgroundLeftLimit;
    public float backgroundRightLimit;
    public PlayerDirection _playerdirection;

    public GameObject player;
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
        rb_player = player.GetComponent<Rigidbody2D>();
        rb_background = background.GetComponent<Rigidbody2D>();

        backgroundPosition = background.transform.position;
        playerPosition = player.transform.position;

        if (backgroundPosition.x <= -(backgroundRightLimit) || backgroundPosition.x >= backgroundLeftLimit)
        {
            if (Mathf.Abs(playerPosition.x) < 1f && notmid == 1)
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
            notmid = 0;
        }
    }

    private void FixedUpdate()
    {
        backgroundPosition = background.transform.position;
        playerPosition = player.transform.position;
        //Debug.Log(playerPosition);

        _playerdirection.setDirection(movementJoystick);

        if(Mathf.Abs(playerPosition.x) < 0.3f)
        {
            if (backgroundPosition.x > backgroundLeftLimit && movementJoystick.Direction.x < 0)
            {
                isBgMoving = 0; // gaboleh gerak
                //Debug.Log("kepanggil 1");
            }
            else if (backgroundPosition.x < -backgroundRightLimit && movementJoystick.Direction.x > 0)
            {
                isBgMoving = 0; // gaboleh gerak
                //Debug.Log(notmid);
            }
            else
            {
                isBgMoving = 1;
            }
        }
        else
        {
            isBgMoving = 0;
        }

        // check bg
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
        if (playerPosition.x >= 7)
        {
            //Debug.Log(movementJoystick.Direction.x);
            if (movementJoystick.Direction.x < 0)
            {
                rb_player.velocity = new Vector2(movementJoystick.Direction.x * playerSpeed, 0);
                maxLR = 0;
                checkMovY();
                //Debug.Log("1");
            }
            else
            {
                maxLR = 1;
                checkMovY();
                //Debug.Log("2");
            }

        }
        else if (playerPosition.x <= -7)
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
            //Debug.Log("3");
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

        if (playerPosition.y >= -1.5)
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
        else if (playerPosition.y <= -3.5)
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
