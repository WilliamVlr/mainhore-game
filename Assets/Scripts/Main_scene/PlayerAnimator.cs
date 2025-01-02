using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public SO_ListAnim idleAnimList;
    public SO_ListMovingAnim moveAnimList;
    public Animator animator;

    public Joystick movementJoystick;

    float timeStop = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if(movementJoystick.Direction.x == 0 && movementJoystick.Direction.y == 0)
        {
            animator.runtimeAnimatorController = idleAnimList.listAnim[0].anim;
            animator.speed = 0;
            timeStop += Time.deltaTime;
        }
        else
        {
            animator.runtimeAnimatorController = moveAnimList.movinglistAnim[0].anim;
            animator.speed = 1;
            timeStop = 0;
        }

        if(timeStop >= 3f)
        {
            animator.speed = 1;
        }
    }
}
