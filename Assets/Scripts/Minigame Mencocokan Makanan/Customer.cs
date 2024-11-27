using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Customer : MonoBehaviour
{
    //Order UI related
    public List<SO_Consumable> orderedItems;
    public TextMeshProUGUI finishOrderText;
    public GameObject orderBox;

    //Customer Sprites
    public SpriteRenderer spriteRenderer;
    public Sprite initialSprite;//First
    public Sprite angrySprite;//Second
    public Sprite madSprite;//Third

    //Waiting Time and Order Flag
    private float waitTimeInterval = 3f;
    private bool orderFinished = false;

    //Destroyable
    public delegate void CustomerDestroyed();
    public event CustomerDestroyed OnCustomerDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
