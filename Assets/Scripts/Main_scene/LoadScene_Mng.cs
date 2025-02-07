using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LoadScene_Mng : MonoBehaviour
{
    public GameObject player;
    public GameObject pintu1;
    public GameObject pintu2;
    public GameObject pintu3;
    public GameObject pintu4;
    public GameObject pintu5;
    public GameObject RightBtn;

    private Collider2D col_player;
    private Collider2D col_pintu1;
    private Collider2D col_pintu2;
    private Collider2D col_pintu3;
    private Collider2D col_pintu4;
    private Collider2D col_pintu5;
    private CircleCollider2D buttonCollider;

    private SpriteRenderer spr_RightBtn;
    private SO_LoadSceneInfo currentSceneInfo = null;

    public SO_ListSO _so_listSO;

    private bool isPlayerTouchingDoor = false;

    void Start()
    {
        col_player = player.GetComponent<Collider2D>();
        col_pintu1 = pintu1.GetComponent<Collider2D>();
        col_pintu2 = pintu2.GetComponent<Collider2D>();
        col_pintu3 = pintu3.GetComponent<Collider2D>();
        col_pintu4 = pintu4.GetComponent<Collider2D>();
        col_pintu5 = pintu5.GetComponent<Collider2D>();
        spr_RightBtn = RightBtn.GetComponent<SpriteRenderer>();

        // Ensure the button is hidden at start
        spr_RightBtn.enabled = false;

        // Ensure the button has a collider
        buttonCollider = RightBtn.GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        // Detect if player is touching any door
        if (col_pintu1.IsTouching(col_player))
        {
            SetDoorScene(col_pintu1, _so_listSO.listSO[0]);
        }
        else if (col_pintu2.IsTouching(col_player))
        {
            SetDoorScene(col_pintu2, _so_listSO.listSO[1]);
        }
        else if (col_pintu3.IsTouching(col_player))
        {
            SetDoorScene(col_pintu3, _so_listSO.listSO[2]);
        }
        else if (col_pintu4.IsTouching(col_player))
        {
            SetDoorScene(col_pintu4, _so_listSO.listSO[3]);
        }
        else if (col_pintu5.IsTouching(col_player))
        {
            SetDoorScene(col_pintu5, _so_listSO.listSO[4]);
        }
        else
        {
            // Player is not touching any door, reset
            isPlayerTouchingDoor = false;
            currentSceneInfo = null;
            spr_RightBtn.enabled = false;
        }

        // Detect Mouse Click on Button
        if (isPlayerTouchingDoor && Input.GetMouseButtonDown(0))
        {
            CheckButtonClick();
        }
    }

    void SetDoorScene(Collider2D doorCollider, SO_LoadSceneInfo sceneInfo)
    {
        isPlayerTouchingDoor = true;
        currentSceneInfo = sceneInfo;
        spr_RightBtn.enabled = true;
        spr_RightBtn.sprite = sceneInfo._sprite;
    }

    void CheckButtonClick()
    {
        if (currentSceneInfo != null)
        {
            // Perform a raycast to check if the button is clicked
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePosition2D = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);

            if (buttonCollider.OverlapPoint(mousePosition2D))
            {
                Debug.Log("Button Clicked! Loading Scene: " + currentSceneInfo.sceneName);
                SceneManager.LoadScene(currentSceneInfo.sceneName);
            }
        }
    }
}
