using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileUIController : MonoBehaviour, IDataPersistence
{
    [SerializeField] private Image profilePicImg;
    [SerializeField] private SO_itemList itemDatabase;

    public static ProfileUIController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void changeProfilePic(Sprite img)
    {
        //Debug.Log("Panggil dari ProfileUIController masuk chagne profilePic");
        //Debug.Log(img);
        //Debug.Log(profilePicImg);
        profilePicImg.sprite = img;
    }

    public void LoadData(GameData data)
    {
        string skinID = data.activeSkin;
        SO_item item = itemDatabase.GetItemByID(skinID);
        if (item != null && item is SO_Skin)
        {
            SO_Skin currentSkin = (SO_Skin)item;
            changeProfilePic(currentSkin.profilePic);

            if (ProfileManager.Instance != null)
            {
                ProfileManager.Instance.ChangeProfile(currentSkin);
            }
        }
        else
        {
            Debug.LogWarning("Can't load skin!");
        }
    }

    public void SaveData(ref GameData data)
    {
        // Do nothing
    }
}
