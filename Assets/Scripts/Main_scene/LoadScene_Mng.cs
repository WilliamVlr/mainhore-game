using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Unity.VisualScripting;

public class LoadScene_Mng : MonoBehaviour
{
    public GameObject player;
    public Button RightBtn;
    public List<GameObject> pintuObjects;
    [HideInInspector]public List<Collider2D> pintuColliderObjects;

    private Collider2D col_player;

    private Image spr_RightBtn;
    private SO_LoadSceneInfo currentSceneInfo = null;

    public SO_ListSO _so_listSO;

    private void Awake()
    {
        
    }

    void Start()
    {
        pintuColliderObjects = new List<Collider2D>();
        for (int i = 0; i < pintuObjects.Count; i++)
        {
            pintuColliderObjects.Add(pintuObjects[i].GetComponent<Collider2D>());
        }
        col_player = player.GetComponent<Collider2D>();

        spr_RightBtn = RightBtn.GetComponent<Image>();
        RightBtn.gameObject.SetActive(false);
    }

    void Update()
    {
        bool isTouching = false;
        for(int i = 0; i < pintuColliderObjects.Count; i++)
        {
            if (pintuColliderObjects[i].IsTouching(col_player))
            {
                SetDoorScene(pintuColliderObjects[i], _so_listSO.listSO[i]);
                isTouching = true;
            }
        }
        if (!isTouching)
        {
            currentSceneInfo = null;
            RightBtn.gameObject.SetActive(false);
        }
    }

    void SetDoorScene(Collider2D doorCollider, SO_LoadSceneInfo sceneInfo)
    {
        //isPlayerTouchingDoor = true;
        currentSceneInfo = sceneInfo;
        RightBtn.gameObject.SetActive(true);
        spr_RightBtn.sprite = sceneInfo._sprite;
    }

    public void CheckButtonClick()
    {
        SceneManager.LoadSceneAsync(currentSceneInfo.sceneName);
    }
}
