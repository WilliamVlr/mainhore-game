using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarManager : MonoBehaviour, IDataPersistence
{
    public SO_Skin currentSkin;
    public SO_itemList itemDatabase;

    public SO_Skin changeSkin(SO_Skin newSkin)
    {
        SO_Skin oldSkin = currentSkin;
        currentSkin = newSkin;

        if(ProfileManager.Instance != null)
        {
            ProfileManager.Instance.ChangeProfile(newSkin);
        }

        if(ProfileUIController.Instance != null)
        {
            ProfileUIController.Instance.changeProfilePic(newSkin.profilePic);
        }

        return oldSkin;
    }

    public void LoadData(GameData data)
    {
        string skinID = data.activeSkin;
        SO_item item = itemDatabase.GetItemByID(skinID);
        if(item != null && item is SO_Skin)
        {
            currentSkin = (SO_Skin)item;

            //if (ProfileManager.Instance != null)
            //{
            //    ProfileManager.Instance.ChangeProfile(currentSkin);
            //}

            //if (ProfileUIController.Instance != null)
            //{
            //    ProfileUIController.Instance.changeProfilePic(currentSkin.profilePic);
            //}
        }
        else
        {
            Debug.LogWarning("Can't load skin!");
        }
    }

    public void SaveData(ref GameData data)
    {
        data.activeSkin = currentSkin.ID;
    }
}
