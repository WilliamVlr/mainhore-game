using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene_Mng : MonoBehaviour
{
    public GameObject player;
    public GameObject pintu1;
    public GameObject pintu2;
    public GameObject pintu3;
    public GameObject RightBtn;

    Collider2D col_player;
    Collider2D col_pintu1;
    Collider2D col_pintu2;
    Collider2D col_pintu3;

    SpriteRenderer spr_RightBtn;

    public SO_ListSO _so_listSO;

    int checking = 0;

    // Start is called before the first frame update
    void Start()
    {
        col_player = player.GetComponent<Collider2D>();
        col_pintu1 = pintu1.GetComponent<Collider2D>();
        col_pintu2 = pintu2.GetComponent<Collider2D>();
        col_pintu3 = pintu3.GetComponent<Collider2D>();
        spr_RightBtn = RightBtn.GetComponent<SpriteRenderer>();
        spr_RightBtn.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        col_player = player.GetComponent<Collider2D>();

        if (col_pintu1.IsTouching(col_player) && checking == 0)
        {
            Debug.Log("Load Scene 1");
            colliderCheck(col_pintu1, _so_listSO.listSO[0]);
        }

        if (col_pintu2.IsTouching(col_player) && checking == 0)
        {
            Debug.Log("Load Scene 2");
            colliderCheck(col_pintu2, _so_listSO.listSO[1]);
        }

        if (col_pintu3.IsTouching(col_player) && checking == 0)
        {
            Debug.Log("Load Scene 3");
            colliderCheck(col_pintu3, _so_listSO.listSO[2]);
        }

        if (!col_pintu1.IsTouching(col_player) && !col_pintu2.IsTouching(col_player) && !col_pintu3.IsTouching(col_player))
        {
            checking = 0;
            spr_RightBtn.enabled = false;
        }
    }

    void colliderCheck(Collider2D coll, SO_LoadSceneInfo _loadSceneInfo)
    {
        spr_RightBtn.enabled = true;
        spr_RightBtn.sprite = _loadSceneInfo._sprite;
        checking++;
    }
}
