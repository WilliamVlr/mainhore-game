using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BoutiqueManager : MonoBehaviour
{
    [SerializeField] private GameObject DialogKasir;           // Gambar dialog kasir
    [SerializeField] private GameObject RandomizeButton;       // Tombol randomize
    [SerializeField] private GameObject[] curtains;            // Array tirai (3 tirai)
    [SerializeField] private GameObject[] costumeCharacters;   // Array karakter kostum (3 karakter)
    [SerializeField] private SO_itemList itemDatabase;

    private bool isRandomizing = false; // Untuk mencegah klik ganda saat proses randomisasi

    private void Start()
    {
        // Sembunyikan dialog, tombol, karakter, dan tutup semua tirai saat mulai
        DialogKasir.SetActive(false);
        RandomizeButton.SetActive(false);
        RandomizeCostumeInside();
        HideAllCharacters();
        CloseAllCurtains();

        // Tambahkan listener untuk tombol randomize
        RandomizeButton.GetComponent<Button>().onClick.AddListener(RandomizeCostume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player mendekati kasir. Menampilkan dialog dan tombol...");
            DialogKasir.SetActive(true);
            RandomizeButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player menjauh dari kasir. Menyembunyikan dialog dan tombol...");
            DialogKasir.SetActive(false);
            RandomizeButton.SetActive(false);
        }
    }

   private void RandomizeCostumeInside()
{
    Debug.Log("Randomizing characters inside tirai");

    // 1. Ambil semua SO_Skin dari itemDatabase
    List<SO_Skin> allSkins = new List<SO_Skin>();
    foreach (SO_item item in itemDatabase.availItems)
    {
        if (item is SO_Skin skin)
        {
            allSkins.Add(skin);
        }
    }

    // 2. Cek apakah skin cukup tersedia
    if (allSkins.Count < 3)
    {
        Debug.LogError("Jumlah skin kurang dari 3! Tambahkan skin di itemDatabase.");
        return;
    }

    // 3. Pilih 3 skin berbeda secara acak
    List<int> chosenIndices = new List<int>();
    while (chosenIndices.Count < 3)
    {
        int randomIndex = Random.Range(0, allSkins.Count);
        if (!chosenIndices.Contains(randomIndex))
        {
            chosenIndices.Add(randomIndex);
        }
    }

    // 4. Ganti SpriteRenderer.sprite di costumeCharacters[]
    for (int i = 0; i < costumeCharacters.Length; i++)
    {
        SO_Skin chosenSkin = allSkins[chosenIndices[i]];
        SpriteRenderer spriteRenderer = costumeCharacters[i].GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && chosenSkin.sprite != null)
        {
            spriteRenderer.sprite = chosenSkin.sprite; // ✅ Ganti sprite sesuai skin
            Debug.Log($"Karakter {i} menggunakan skin: {chosenSkin.name}");
        }
        else
        {
            Debug.LogWarning($"SpriteRenderer atau skinSprite tidak ditemukan di costumeCharacter index {i}");
        }
        costumeCharacters[i].SetActive(false);
    }
}
    private void RandomizeCostume()
    {
        if (!isRandomizing)
        {
            RandomizeCostumeInside();
            Debug.Log("Memulai proses randomisasi kostum...");
            isRandomizing = true;
            RandomizeButton.SetActive(false);
            DialogKasir.SetActive(false);
            StartCoroutine(RandomizeProcess());
        }
    }

    private IEnumerator RandomizeProcess()
    {
        yield return new WaitForSeconds(1f); // Simulasi waktu untuk animasi spotlight

        // Pilih karakter secara acak
        int chosenIndex = Random.Range(0, costumeCharacters.Length);
        Debug.Log("Karakter yang dipilih: " + chosenIndex);

        // Buka tirai dan tampilkan karakter
        yield return StartCoroutine(OpenCurtain(chosenIndex));
        ShowCharacter(chosenIndex);

        Debug.Log("Kostum telah ditampilkan. Pemain dapat meng-equip.");
        isRandomizing = false;
    }

    private IEnumerator OpenCurtain(int index)
    {
        Debug.Log("Membuka tirai nomor: " + index);
        Animator curtainAnimator = curtains[index].GetComponent<Animator>();
        if (curtainAnimator != null)
        {
            curtainAnimator.SetTrigger("Open"); // Trigger animasi buka tirai
            yield return new WaitForSeconds(1.5f); // Tunggu animasi selesai
        }
        else
        {
            Debug.LogWarning("Animator tidak ditemukan di tirai index " + index);
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
}
