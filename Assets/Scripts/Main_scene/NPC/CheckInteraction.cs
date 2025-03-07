using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInteraction : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject button;
    private GameObject collidedplayer;
    public GameObject Collided => collidedplayer;
    private void Awake()
    {
        // Initially hide the button
        collidedplayer = null;
        button.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
        if (other.gameObject == player && !inventoryUI.IsOpen())
        {
            //Debug.Log("Player entered NPC area!");
            collidedplayer = this.gameObject;
            //Debug.Log(collidedplayer.transform.position.x);
            //collidedplayer.transform.Rotate(10, 10, 10);
            button.SetActive(true); // Show interaction button
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            //collidedplayer.SetActive(true);
            //Debug.Log("Player left NPC area!");
            collidedplayer = null;
            button.SetActive(false); // Hide interaction button
        }
    }

}
