using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class BoutiqueManager : MonoBehaviour
{
    [SerializeField] private GameObject DialogKasir;
    [SerializeField] private GameObject RandomizeButton;
    [SerializeField] private GameObject SimpanButton;
    [SerializeField] private GameObject UseClothButton;
    [SerializeField] private GameObject CoinIcon;
    [SerializeField] private TextMeshProUGUI CoinNominalText;
    [SerializeField] private GameObject[] curtains;
    [SerializeField] private GameObject[] costumeCharacters;
    [SerializeField] private SO_itemList itemDatabase;
    [SerializeField] private SpriteRenderer playerAvatarRenderer;

    private SO_Skin selectedSkin;
    private bool isRandomizing = false;
    private const int randomizationCost = 1000;
    private List<SO_Skin> allSkins;

    private void Start()
    {
        DialogKasir.SetActive(false);
        RandomizeButton.SetActive(false);
        SimpanButton.SetActive(false);
        UseClothButton.SetActive(false);
        CoinIcon.SetActive(false);
        CoinNominalText.gameObject.SetActive(false);

        RandomizeCostumeInside();
        HideAllCharacters();
        CloseAllCurtains();
        DisplayRandomizationCost();

        RandomizeButton.GetComponent<Button>().onClick.AddListener(RandomizeCostume);
        SimpanButton.GetComponent<Button>().onClick.AddListener(SaveSelectedCostume);
        UseClothButton.GetComponent<Button>().onClick.AddListener(UseSelectedCostume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogKasir.SetActive(true);
            RandomizeButton.SetActive(true);
            CoinIcon.SetActive(true);
            CoinNominalText.gameObject.SetActive(true);
            DisplayRandomizationCost();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogKasir.SetActive(false);
            RandomizeButton.SetActive(false);
            CoinIcon.SetActive(false);
            CoinNominalText.gameObject.SetActive(false);
        }
    }

    private void RandomizeCostumeInside()
    {
        allSkins = new List<SO_Skin>();
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

            if (spriteRenderer != null && chosenSkin.sprite != null)
                spriteRenderer.sprite = chosenSkin.sprite;

            costumeCharacters[i].SetActive(false);
        }
    }

    private void RandomizeCostume()
    {
        if (!isRandomizing)
        {
            if (CoinManager.Instance.canSubstractCoin(randomizationCost))
            {
                CoinManager.Instance.substractCoin(randomizationCost); // Koin langsung dikurangi
                DisplayRandomizationCost();
                isRandomizing = true;
                RandomizeButton.SetActive(false);
                DialogKasir.SetActive(false);
                CoinIcon.SetActive(false);
                CoinNominalText.gameObject.SetActive(false);
                StartCoroutine(RandomizeProcess());
            }
            else
            {
                Debug.Log("Koin tidak cukup untuk randomisasi!");
            }
        }
    }

    private IEnumerator RandomizeProcess()
    {
        yield return new WaitForSeconds(1f);
        int chosenIndex = Random.Range(0, costumeCharacters.Length);
        yield return StartCoroutine(OpenCurtain(chosenIndex));
        ShowCharacter(chosenIndex);
        SimpanButton.SetActive(true);
        UseClothButton.SetActive(true);
        selectedSkin = allSkins[chosenIndex];
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
    }

    private void HideAllCharacters()
    {
        foreach (GameObject character in costumeCharacters)
            character.SetActive(false);
    }

    private void SaveSelectedCostume()
    {
        if (selectedSkin != null)
        {
            InventoryManager.Instance.AddItem(selectedSkin);
            Debug.Log("Kostum disimpan ke inventory: " + selectedSkin.name);
        }
        SimpanButton.SetActive(false);
        UseClothButton.SetActive(false);
    }

    private void UseSelectedCostume()
    {
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
            Debug.Log("Kostum telah digunakan: " + selectedSkin.name);
        }
        SimpanButton.SetActive(false);
        UseClothButton.SetActive(false);
    }

    private void DisplayRandomizationCost()
    {
        CoinNominalText.text = randomizationCost.ToString();
    }
}
