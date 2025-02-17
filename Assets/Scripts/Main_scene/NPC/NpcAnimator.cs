using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAnimator : MonoBehaviour
{
    private Animator anim;
    private bool stopped;
    private float currSpeed;
    [SerializeField] private GameObject NpcUI;
    [SerializeField] private NpcPatrol npcpatrol;
    [SerializeField] private SO_ListNpcAnim listNpcAnim;
    float freezetime = 0;
    int check = 0;
    // Start is called before the first frame update
    void Start()
    {
        anim = NpcUI.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        stopped = npcpatrol.IsStopped;
        currSpeed = npcpatrol.CurrentSpeed;

        Debug.Log(check);
        //Debug.Log(stopped);
        //Debug.Log(currSpeed);
        if (stopped)
        {
            anim.speed = 0;
            if (currSpeed > 0)
            {
                if(currSpeed < 0.5f)
                {
                    check = 1;
                }
            }
            else
            {
                if (-currSpeed < 0.5f)
                {
                    check = 1;
                }
            }

            if(check == 1)
            {
                Debug.Log("stopping");
                anim.runtimeAnimatorController = listNpcAnim.listAnim[0].NpcAnim;
                freezetime += Time.deltaTime;
                if (freezetime > 1f)
                {
                    anim.speed = 1;
                }
                else
                {
                    anim.speed = 0;
                }
            }
        }
        else
        {
            Debug.Log("Continue");
            freezetime = 0;
            anim.runtimeAnimatorController = listNpcAnim.listAnim[1].NpcAnim;
            anim.speed = 1;
            check = 0;
        }
    }
}
