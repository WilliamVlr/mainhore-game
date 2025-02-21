using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BoutiqueManager : MonoBehaviour
{
    [SerializeField] private GameObject DialogKasir;
    [SerializeField] private GameObject RandomizeButton;
    [SerializeField] private GameObject SimpanButton;
    [SerializeField] private GameObject UseClothButton;
    [SerializeField] private GameObject[] curtains;
    [SerializeField] private GameObject[] costumeCharacters;
    [SerializeField] private SO_itemList itemDatabase;
    [SerializeField] private GameObject PlayerAvatar;

    private bool isRandomizing = false;
    private SO_Skin selectedSkin;

    private void Start()
    {
        DialogKasir.SetActive(false);
        RandomizeButton.SetActive(false);
        SimpanButton.SetActive(false);
        UseClothButton.SetActive(false);
        RandomizeCostumeInside();
        HideAllCharacters();
        CloseAllCurtains();

        RandomizeButton.GetComponent<Button>().onClick.AddListener(RandomizeCostume);
        SimpanButton.GetComponent<Button>().onClick.AddListener(SaveCostumeToInventory);
        UseClothButton.GetComponent<Button>().onClick.AddListener(EquipSelectedCostume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogKasir.SetActive(true);
            RandomizeButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogKasir.SetActive(false);
            RandomizeButton.SetActive(false);
        }
    }

    private void RandomizeCostumeInside()
    {
        List<SO_Skin> allSkins = new List<SO_Skin>();
        foreach (SO_item item in itemDatabase.availItems)
        {
            if (item is SO_Skin skin)
            {
                allSkins.Add(skin);
            }
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
            {
                chosenIndices.Add(randomIndex);
            }
        }

        for (int i = 0; i < costumeCharacters.Length; i++)
        {
            SO_Skin chosenSkin = allSkins[chosenIndices[i]];
            SpriteRenderer spriteRenderer = costumeCharacters[i].GetComponent<SpriteRenderer>();

            if (spriteRenderer != null && chosenSkin.sprite != null)
            {
                spriteRenderer.sprite = chosenSkin.sprite;
            }
            costumeCharacters[i].SetActive(false);
        }
    }

    private void RandomizeCostume()
    {
        if (!isRandomizing)
        {
            RandomizeCostumeInside();
            isRandomizing = true;
            RandomizeButton.SetActive(false);
            DialogKasir.SetActive(false);
            SimpanButton.SetActive(false);
            UseClothButton.SetActive(false);
            StartCoroutine(RandomizeProcess());
        }
    }

    private IEnumerator RandomizeProcess()
    {
        yield return new WaitForSeconds(1f);

        int chosenIndex = Random.Range(0, costumeCharacters.Length);
        selectedSkin = itemDatabase.availItems[chosenIndex] as SO_Skin;

        yield return StartCoroutine(OpenCurtain(chosenIndex));
        ShowCharacter(chosenIndex);

        SimpanButton.SetActive(true);
        UseClothButton.SetActive(true);
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
            {
                curtainAnimator.SetTrigger("Close");
            }
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
        {
            character.SetActive(false);
        }
    }

    private void SaveCostumeToInventory()
    {
        if (selectedSkin != null && !itemDatabase.availItems.Contains(selectedSkin))
        {
            itemDatabase.availItems.Add(selectedSkin);
            Debug.Log($"Kostum {selectedSkin.name} telah disimpan ke inventory.");
            SimpanButton.SetActive(false);
            UseClothButton.SetActive(false);
        }
    }

    private void EquipSelectedCostume()
    {
        if (selectedSkin != null && PlayerAvatar != null)
        {
            SpriteRenderer avatarRenderer = PlayerAvatar.GetComponent<SpriteRenderer>();
            if (avatarRenderer != null)
            {
                avatarRenderer.sprite = selectedSkin.sprite;
                Debug.Log($"Avatar berhasil mengenakan kostum: {selectedSkin.name}");
                SimpanButton.SetActive(false);
                UseClothButton.SetActive(false);
            }
        }
    }
}
