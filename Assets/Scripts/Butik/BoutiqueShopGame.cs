using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class BoutiqueManager : MonoBehaviour
{
    [SerializeField] private GameObject DialogKasir;
    [SerializeField] private Button RandomizeButton;
    [SerializeField] private GameObject SimpanButton;
    [SerializeField] private GameObject UseClothButton;
    [SerializeField] private GameObject[] curtains;
    [SerializeField] private GameObject[] costumeCharacters;
    [SerializeField] private SO_itemList itemDatabase;
    [SerializeField] private SpriteRenderer playerAvatarRenderer;
    [SerializeField] private Sprite gachaBtnSprite;
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Light2D spotLight;
    [SerializeField] private Light2D finalSpotlight;
    [SerializeField] private CanvasBehavior joystickCanvas;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CanvasBehavior resultCanvas;
    [SerializeField] private TextMeshProUGUI karakterName;
    [SerializeField] private Image karakterImg;

    private SO_Skin selectedSkin;
    private bool isRandomizing = false;
    private const int randomizationCost = 1000;
    private List<SO_Skin> allSkins;
    private SO_Skin[] costumerCharacters_data;

    private void Start()
    {
        DialogKasir.SetActive(false);
        RandomizeButton.gameObject.SetActive(false);

        RandomizeCostumeInside();
        HideAllCharacters();
        CloseAllCurtains();

        SimpanButton.GetComponent<Button>().onClick.AddListener(SaveSelectedCostume);
        UseClothButton.GetComponent<Button>().onClick.AddListener(UseSelectedCostume);

        SoundManager.Instance.PlayMusicInList("Butik");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogKasir.SetActive(true);
            RandomizeButton.GetComponent<Image>().sprite = gachaBtnSprite;
            RandomizeButton.gameObject.SetActive(true);
            RandomizeButton.onClick.AddListener(OnRandomizeClicked);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (RandomizeButton != null)
        {
            DialogKasir.SetActive(false);
            RandomizeButton.GetComponent<Image>().sprite = null;
            RandomizeButton.gameObject.SetActive(false);
            RandomizeButton.onClick.RemoveAllListeners();
        }
    }

    private void RandomizeCostumeInside()
    {
        allSkins = new List<SO_Skin>();
        costumerCharacters_data = new SO_Skin[3];
        foreach (SO_item item in itemDatabase.availItems)
        {
            if (item is SO_Skin skin)
                allSkins.Add(skin);
        }

        if (allSkins.Count < 3)
        {
            Debug.LogError("Jumlah skin kurang dari 3!");
            return;
        }

        List<int> chosenIndices = new List<int>();
        while (chosenIndices.Count < 3)
        {
            int randomIndex = Random.Range(0, allSkins.Count);
            if (!chosenIndices.Contains(randomIndex))
                chosenIndices.Add(randomIndex);
        }

        for (int i = 0; i < costumeCharacters.Length; i++)
        {
            SO_Skin chosenSkin = allSkins[chosenIndices[i]];
            SpriteRenderer spriteRenderer = costumeCharacters[i].GetComponentInChildren<SpriteRenderer>();
            costumerCharacters_data[i] = chosenSkin;

            if (spriteRenderer != null && chosenSkin.sprite != null)
            {
                spriteRenderer.sprite = chosenSkin.sprite;
                Debug.Log("Sprite should be changed");
            }

            costumeCharacters[i].SetActive(false);
        }
    }

    public void OnRandomizeClicked()
    {
        Debug.Log("Randomize button clicked");
        ConfirmationBehavior confirmationPanel = FindAnyObjectByType<ConfirmationBehavior>();
        RandomizeCostumeInside();

        if (confirmationPanel != null)
        {
            confirmationPanel.showConfirmDecorationPanel(
                () => RandomizeCostume(),
                () => Debug.Log("Cancel Buy")
            );
        }
        else
        {
            Debug.Log("Confirmation panel not found!");
        }
    }

    private void RandomizeCostume()
    {
        if (InventoryManager.Instance.isFull())
        {
            NotifPanelBehavior notifPanel = FindAnyObjectByType<NotifPanelBehavior>();
            if (notifPanel != null)
            {
                notifPanel.showInvFull();
            }
            else
            {
                Debug.Log("Notif panel is not found");
            }
            return;
        }
        if (!isRandomizing)
        {
            if (CoinManager.Instance.canSubstractCoin(randomizationCost))
            {
                joystickCanvas.hideCanvas();
                mainCamera.transform.position = new Vector3(-6, 0, -10);
                globalLight.intensity = 0.03f;
                CoinManager.Instance.substractCoin(randomizationCost); // Koin langsung dikurangi
                SoundManager.Instance.PlaySFXInList("Coin berkurang");
                isRandomizing = true;
                RandomizeButton.gameObject.SetActive(false);
                DialogKasir.SetActive(false);
                StartCoroutine(RandomizeProcess());
            }
            else
            {
                Debug.Log("Koin tidak cukup untuk randomisasi!");
                NotifPanelBehavior notifPanel = FindAnyObjectByType<NotifPanelBehavior>();
                if (notifPanel != null)
                {
                    notifPanel.showCoinInsuff();
                }
                else
                {
                    Debug.Log("Notif panel is not found");
                }
            }
        }
    }

    private IEnumerator RandomizeProcess()
    {
        // Play the gacha animation from the Animator attached to spotLight
        Animator spotLightAnimator = spotLight.GetComponent<Animator>();
        if (spotLightAnimator != null)
        {
            spotLight.intensity = 1;
            spotLightAnimator.SetTrigger("Gacha");
            SoundManager.Instance.PlaySFXInList("gacha");

            // Wait for animation to finish
            yield return new WaitForSeconds(spotLightAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        yield return new WaitForSeconds(1.5f); // Small delay for smooth transition

        int chosenIndex = Random.Range(0, costumeCharacters.Length);
        yield return StartCoroutine(OpenCurtain(chosenIndex));
        ShowCharacter(chosenIndex);
        selectedSkin = costumerCharacters_data[chosenIndex];
        karakterName.text = selectedSkin.itemName;
        karakterImg.sprite = selectedSkin.sprite;
        yield return new WaitForSeconds(0.5f);
        resultCanvas.showCanvas();
        isRandomizing = false;
    }

    private IEnumerator OpenCurtain(int index)
    {
        Animator curtainAnimator = curtains[index].GetComponent<Animator>();
        if (curtainAnimator != null)
        {
            curtainAnimator.SetTrigger("Open");
            yield return new WaitForSeconds(1.5f);
        }
    }

    private void CloseAllCurtains()
    {
        foreach (GameObject curtain in curtains)
        {
            Animator curtainAnimator = curtain.GetComponent<Animator>();
            if (curtainAnimator != null)
                curtainAnimator.SetTrigger("Close");
        }
    }

    private void ShowCharacter(int index)
    {
        HideAllCharacters();
        costumeCharacters[index].SetActive(true);
        if (index == 0)
        {
            finalSpotlight.transform.localPosition = new Vector2(-4.2f, -0.3f);
        } else if (index == 1)
        {
            finalSpotlight.transform.localPosition = new Vector2(-1f, -0.3f);
        } else
        {
            finalSpotlight.transform.localPosition = new Vector2(2.7f, -0.3f);
        }
        finalSpotlight.intensity = 1;
    }

    private void HideAllCharacters()
    {
        foreach (GameObject character in costumeCharacters)
            character.SetActive(false);
    }

    private void SaveSelectedCostume()
    {
        globalLight.intensity = 1;
        spotLight.intensity = 0;
        finalSpotlight.intensity = 0;
        joystickCanvas.showCanvas();
        ResetCameraPos();
        if (selectedSkin != null)
        {
            InventoryManager.Instance.AddItem(selectedSkin);
            Debug.Log("Kostum disimpan ke inventory: " + selectedSkin.name);
        }
        resultCanvas.hideCanvas();
    }

    private void UseSelectedCostume()
    {
        globalLight.intensity = 1;
        spotLight.intensity = 0;
        finalSpotlight.intensity = 0;
        joystickCanvas.showCanvas();
        ResetCameraPos();
        if (selectedSkin != null && selectedSkin.sprite != null && playerAvatarRenderer != null)
        {
            AvatarManager avatarMng = FindAnyObjectByType<AvatarManager>();
            if (avatarMng != null)
            {
                SO_Skin oldSkin = avatarMng.changeSkin(selectedSkin);
                InventoryManager.Instance.AddItem(oldSkin);
            }
            else
            {
                Debug.LogWarning("Avatar Manager not found!");
            }
            //Debug.Log("Kostum telah digunakan: " + selectedSkin.name);
        }
        resultCanvas.hideCanvas();
    }

    private void ResetCameraPos()
    {
        mainCamera.transform.position = new Vector3(0, 0, -10);
    }
}
