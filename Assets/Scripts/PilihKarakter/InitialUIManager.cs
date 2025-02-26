using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InitialUIManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private TextMeshProUGUI welcomeTxt;
    [SerializeField] private Button confirmUsernameBtn;
    [SerializeField] private string[] listOfNames = { "Rizky", "Dewi", "Fajar", "Santi", "Budi", "Lestari", "Agung", "Tania", "Andre", "Rina" };
    [SerializeField] private string[] listOfSifat = { "Gemoy", "Kreatif", "Rajin", "Keren", "Kocak", "Pintar", "Cakep", "Hebat", "Ceria", "Asik" };

    public void confirmUsername()
    {
        welcomeTxt.transform.parent.gameObject.SetActive(true);
        welcomeTxt.text = "Hello, " + input.text;
        InitialDataManager.Instance.Username = input.text;
    }

    public void checkUsername(string usn)
    {
        if(usn == "")
        {
            confirmUsernameBtn.interactable = false;
        }
        else
        {
            confirmUsernameBtn.interactable = true;
        }
    }

    public bool isValidInput()
    {
        return input.text != "";
    }

    private string randomUsername()
    {
        string random = "";

        // Pilih nama secara acak dari listOfNames
        string randomName = listOfNames[Random.Range(0, listOfNames.Length)];

        // Pilih kata sifat secara acak dari listOfSifat
        string randomSifat = listOfSifat[Random.Range(0, listOfSifat.Length)];

        random = randomName + " " + randomSifat;

        // Gabungkan nama dan kata sifat
        return random;
    }

    public void generateRandomUsername()
    {
        string random = randomUsername();
        input.text = random;
    }

    public void OnConfirmChoices()
    {
        ConfirmationBehavior confirmationPanel = FindAnyObjectByType<ConfirmationBehavior>();

        if (confirmationPanel != null)
        {
            confirmationPanel.showConfirmDecorationPanel(
                () => InitialDataManager.Instance.createNewGame(),
                () => Debug.Log("Cancel selling")
            );
        }
        else
        {
            Debug.Log("Confirmation panel not found!");
        }
    }
}