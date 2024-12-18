using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerCafe : MonoBehaviour
{
    //Order UI related
    public TextMeshProUGUI responseText;
    public GameObject orderPanel;

    //Order Related
    private List<SO_item> orderedItems;
    public SO_itemList itemList;
    private int maxOrder = 3;

    //Customer Sprites
    public SpriteRenderer customerSpriteRenderer;
    public SO_CustomerSetList customerSetList;
    public Transform customerTransform;
    private SO_CustomerSet chosenCustomerSpriteSet;//First = initial, Second = angry, Third = mad

    //Waiting Time and Order Flag
    private float waitTimeInterval;
    private float maxWaitTime = 5f;
    private float waitTimeMultiplier = 1.5f;
    private bool orderFinished = false;

    //Destroyable
    public delegate void CustomerDestroyed();
    public event CustomerDestroyed OnCustomerDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        //Random customer sprite set
        chosenCustomerSpriteSet = customerSetList.GetRandomSet();

        //Set customer y position (y - 1 because this adds up to the default value which is 1, 1, 1)
        transform.position = new Vector3(customerTransform.position.x, chosenCustomerSpriteSet.position.y, customerTransform.position.z);

        //Set first customer sprite
        customerSpriteRenderer.sprite = chosenCustomerSpriteSet.customerSpriteSet.First();

        //Generate Random Order
        generateRandomOrder();

        //Show Order
        showOrder();

        //Waiting logic
        StartCoroutine(WaitAndReact());
    }

    private void setWaitTimeInterval(float dur, float mult)
    {
        waitTimeInterval = dur * mult;
        if(waitTimeInterval > maxWaitTime) waitTimeInterval = maxWaitTime;
    }

    private void generateRandomOrder()
    {
        orderedItems = new List<SO_item>();
        var cafeItems = itemList.availItems.Where(item => item.place == Place.Cafe).ToList();

        if (cafeItems.Count > 0)
        {
            int numOrder = Random.Range(1, maxOrder + 1);

            for (int i = 0; i < numOrder; i++)
            {
                var randomItem = cafeItems[Random.Range(0, cafeItems.Count)];
                orderedItems.Add(randomItem);
            }

        }
        else
        {
            Debug.LogWarning("Tidak ada consumable item dengan place di cafe!");
        }

    }

    public void showOrder()
    {
        // Clear previous food items if any
        foreach (Transform child in orderPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Create UI elements for each ordered food item
        foreach (SO_Consumable item in orderedItems)
        {
            GameObject itemUI = new GameObject(item.name); // Create a new GameObject for the food item
            itemUI.transform.SetParent(orderPanel.transform); // Set parent to the dialog box

            // Add Image component to display food sprite
            Image itemImage = itemUI.AddComponent<Image>();
            itemImage.sprite = item.sprite;
            itemImage.SetNativeSize();

            // Adjust the scale to 1.5x for both X and Y
            RectTransform rectTransform = itemUI.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Draggable item = collision.gameObject.GetComponent<Draggable>();
        if(item != null)
        {
            CheckOrder(item);
            Destroy(item.gameObject);
        }
    }

    public void CheckOrder(Draggable item)
    {
        // Get the FoodItem component from the food GameObject
        SO_item itemSO = item.itemSO;
        if (itemSO != null)
        {
            //Debug.Log($"item: {item.name}, SOitem: {itemSO}");
            bool isInOrder = orderedItems.Contains(itemSO);

            // If the orderedFood list has more than one item
            if (orderedItems.Count > 1)
            {
                if (isInOrder)
                {
                    // Remove the food item from the orderedFood list
                    orderedItems.Remove(itemSO);
                    DestroyItemUIImage(itemSO.name); //Destroy ordered item image in orderBox
                    Destroy(item); // Destroy the draggable item GameObject
                }
                else
                {
                    customerSpriteRenderer.sprite = chosenCustomerSpriteSet.customerSpriteSet[2]; // Change to angry sprite
                    ShowResponseAndDestroy("Pesanan Salah!"); // Wrong order 
                }
            }
            // If the orderedItems list has only one item
            else if (orderedItems.Count == 1)
            {
                if (isInOrder)
                {
                    // Remove the food item from the orderedFood list
                    orderFinished = true;
                    orderedItems.Remove(itemSO);
                    DestroyItemUIImage(itemSO.name);
                    //Destroy(foodImageRenderer); // Destroy the food image renderer
                    Destroy(item); // Destroy the food GameObject
                    ShowResponseAndDestroy("Terima kasih!"); // Thank you message
                }
                else
                {
                    customerSpriteRenderer.sprite = chosenCustomerSpriteSet.customerSpriteSet[2];
                    ShowResponseAndDestroy("Pesanan Salah!"); // Wrong order message
                }
            }
        }
        else
        {
            Debug.LogWarning("The food GameObject does not have a SO_Item component.");
            customerSpriteRenderer.sprite = chosenCustomerSpriteSet.customerSpriteSet[2];
            ShowResponseAndDestroy("System Error!");
        }
    }

    private IEnumerator WaitAndReact()
    {
        
        for (int i = 1; i < chosenCustomerSpriteSet.customerSpriteSet.Count; i++)
        {
            setWaitTimeInterval(this.orderedItems.Count, waitTimeMultiplier);
            yield return new WaitForSeconds(waitTimeInterval);
            if (!orderFinished)
            {
                customerSpriteRenderer.sprite = chosenCustomerSpriteSet.customerSpriteSet[i];
            }
        }

        if (!orderFinished)
        {
            orderFinished = true;
            //Hapus semua imagenya atau nonaktifkan orderBox
            orderPanel.SetActive(false);

            //Show Response terlalu lama
            ShowResponseAndDestroy("Terlalu Lama!");
        }
    }

    public void ShowResponseAndDestroy(string message, float dur = 1f)
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        orderPanel.SetActive(false);
        responseText.text = message; // Set the dialog text
        StartCoroutine(DestroyCustomerAfterDelay(dur));
    }

    private void DestroyItemUIImage(string foodName)
    {
        bool itemDestroyed = false;
        foreach (Transform child in orderPanel.transform)
        {
            //Debug.Log($"Checking child: {child.name}, foodName: {foodName}");
            if (child.name == foodName)
            {
                //Debug.Log($"Destroying object: {child.name}");
                Destroy(child.gameObject);
                itemDestroyed = true;
                break;
            }
        }

        if (!itemDestroyed)
        {
            Debug.LogWarning($"No matching UI element found for foodName: {foodName}");
        }

        // Force layout rebuild
        LayoutRebuilder.ForceRebuildLayoutImmediate(orderPanel.GetComponent<RectTransform>());
    }

    private IEnumerator DestroyCustomerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified duration
        Destroy(gameObject); // Destroy the customer object after the dialog
    }

    private void OnDestroy()
    {
        // Trigger the OnDestroyed event when this customer is destroyed
        OnCustomerDestroyed?.Invoke();
    }
}
