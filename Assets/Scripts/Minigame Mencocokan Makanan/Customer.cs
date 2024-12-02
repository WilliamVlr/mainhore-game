using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    //Order UI related
    public List<SO_Item> orderedItems;
    public TextMeshProUGUI finishOrderText;
    public GameObject orderBox;

    //Order Related
    public SO_itemList itemList;
    private int maxOrder = 3;

    //Customer Sprites
    public SpriteRenderer spriteRenderer;
    public List<Sprite> sprites;//First = initial, Second = angry, Third = mad

    //Waiting Time and Order Flag
    private float waitTimeInterval = 3f;
    private bool orderFinished = false;

    //Destroyable
    public delegate void CustomerDestroyed();
    public event CustomerDestroyed OnCustomerDestroyed;

    // Start is called before the first frame update
    private void Start()
    {
        //Set initial sprite
        spriteRenderer.sprite = sprites.First();

        //Generate Random Order
        generateRandomOrder();

        //Show Order
        showOrder();

        //Waiting logic
        StartCoroutine(WaitAndReact());

    }

    private void generateRandomOrder()
    {
        orderedItems = new List<SO_Item>();
        var cafeItems = itemList.items.Where(item => item.place == ItemLocation.Cafe).ToList();

        if(cafeItems.Count > 0 )
        {
            int numOrder = Random.Range(1, maxOrder+1);

            for(int i = 0; i < numOrder; i++)
            {
                var randomItem = cafeItems[Random.Range(0, cafeItems.Count)];
                orderedItems.Add(randomItem);
            }

        } else
        {
            Debug.LogWarning("Tidak ada item dengan place di cafe!");
        }
    }

    public void showOrder()
    {
        // Clear previous food items if any
        foreach (Transform child in orderBox.transform)
        {
            Destroy(child.gameObject);
        }

        // Create UI elements for each ordered food item
        foreach (SO_Item item in orderedItems)
        {
            GameObject itemUI = new GameObject(item.name); // Create a new GameObject for the food item
            itemUI.transform.SetParent(orderBox.transform); // Set parent to the dialog box

            // Add Image component to display food sprite
            Image itemImage = itemUI.AddComponent<Image>();
            itemImage.sprite = item.sprite; // Assuming FoodItem has a sprite field
            itemImage.SetNativeSize();

            // Adjust the scale to 1.5x for both X and Y
            RectTransform rectTransform = itemUI.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1.5f, 1.5f, 1f);
        }
    }

    public void CheckOrder(DraggableItem item)
    {
        // Get the FoodItem component from the food GameObject
        SO_Item itemSO = item.itemSO;
        if (itemSO != null)
        {
            Debug.Log($"item: {item.name}, SOitem: {itemSO}");
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
                    spriteRenderer.sprite = sprites[2]; // Change to angry sprite
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
                    spriteRenderer.sprite = sprites[2]; // Change to angry sprite
                    ShowResponseAndDestroy("Pesanan Salah!"); // Wrong order message
                }
            }
        }
        else
        {
            Debug.LogWarning("The food GameObject does not have a SO_Item component.");
            spriteRenderer.sprite = sprites[2];
            ShowResponseAndDestroy("Pesanan Salah!");
        }
    }

    private IEnumerator WaitAndReact()
    {
        for(int i = 1; i < sprites.Count; i++)
        {
            yield return new WaitForSeconds(waitTimeInterval);
            if (!orderFinished)
            {
                spriteRenderer.sprite = sprites[i];
            }
        }

        if (!orderFinished)
        {
            orderFinished = true;
            //Hapus semua imagenya atau nonaktifkan orderBox
            orderBox.SetActive(false);

            //Show Response terlalu lama
            ShowResponseAndDestroy("Terlalu Lama!");
        }
    }

    public void ShowResponseAndDestroy(string message, float dur = 2f)
    {
        orderBox.SetActive(false);
        finishOrderText.text = message; // Set the dialog text
        StartCoroutine(DestroyCustomerAfterDelay(dur));
    }

    private void DestroyItemUIImage(string foodName)
    {
        bool itemDestroyed = false;
        foreach (Transform child in orderBox.transform)
        {
            Debug.Log($"Checking child: {child.name}, foodName: {foodName}");
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
        LayoutRebuilder.ForceRebuildLayoutImmediate(orderBox.GetComponent<RectTransform>());
    }

    private IEnumerator DestroyCustomerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified duration
        //dialogText.text = ""; // Clear the dialog text
        //dialogBox.SetActive(false); // Hide the dialog box
        Destroy(gameObject); // Destroy the customer object after the dialog
    }

    private void OnDestroy()
    {
        // Trigger the OnDestroyed event when this customer is destroyed
        OnCustomerDestroyed?.Invoke();
    }
}
