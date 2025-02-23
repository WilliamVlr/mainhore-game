using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTrigger : MonoBehaviour
{
    public Button sceneLoaderButton;    
    public Sprite buyImg;
    public TextMeshProUGUI hargaText;
    public SO_Furniture currentItem;
    SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (sceneLoaderButton != null)
        {
            sceneLoaderButton.gameObject.SetActive(false);
        }
    }

    public void SetItem(SO_Furniture furnitureData)
    {
        currentItem = furnitureData;
        if(spriteRenderer != null){
            spriteRenderer.sprite = furnitureData.sprite;
            transform.localScale = new Vector2(currentItem.scale_inBackground, currentItem.scale_inBackground);
        }
        hargaText.text = furnitureData.price.ToString();
    }

    // Trigger saat karakter memasuki area collider item
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            sceneLoaderButton.gameObject.SetActive(true);
            sceneLoaderButton.GetComponent<Image>().sprite = buyImg;
            sceneLoaderButton.onClick.RemoveAllListeners();
            sceneLoaderButton.onClick.AddListener(OnBuyClicked);
        }
    }

    // Trigger saat karakter keluar dari area collider item
    private void OnTriggerExit2D(Collider2D other)
    {
        if (sceneLoaderButton != null)
            {
                sceneLoaderButton.gameObject.SetActive(false);
                sceneLoaderButton.GetComponent<Image>().sprite = null;
                sceneLoaderButton.onClick.RemoveAllListeners();
            }
    }

    private void OnBuyClicked(){
        ConfirmationBehavior confirmationPanel = FindAnyObjectByType<ConfirmationBehavior>();

        if (confirmationPanel != null)
        {
            confirmationPanel.showConfirmBuyingPanel(
                currentItem,
                () => confirmBuy(),
                () => Debug.Log("Cancel Buy")
            );
        }
        else
        {
            Debug.Log("Confirmation panel not found!");
        }
    }

    private void confirmBuy()
    {
        
        //TODO - kurangi coin player
         int itemPrice = currentItem.price;
         if (CoinManager.Instance.canSubstractCoin(itemPrice))
         {
            CoinManager.Instance.substractCoin(itemPrice); 
            //TODO - ganti text harga dengan sold out
            if (hargaText != null)
            {
                hargaText.text = "Sold Out"; 
            }
            InventoryManager.Instance.AddItem(currentItem);
            DataPersistenceManager.Instance.saveGame();
            //TODO - Destroy object furniturenya
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("Not enough coins to purchase the item.");
            NotifPanelBehavior notifPanel = FindAnyObjectByType<NotifPanelBehavior>();
            if (notifPanel != null)
            {
                notifPanel.showCanvas();
            }
            else
            {
                Debug.Log("Notif panel is not found");
            }
        }
    }
}
