using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoaderObject : MonoBehaviour
{
    public SO_LoadSceneInfo sceneInfo;
    public Button sceneLoaderButton;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sceneLoaderButton.gameObject.SetActive(true);
            sceneLoaderButton.GetComponent<Image>().sprite = sceneInfo._sprite;
            sceneLoaderButton.onClick.RemoveAllListeners();
            sceneLoaderButton.onClick.AddListener(loadScene);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (sceneLoaderButton != null)
            {
                sceneLoaderButton.gameObject.SetActive(false);
                sceneLoaderButton.GetComponent<Image>().sprite = null;
                sceneLoaderButton.onClick.RemoveAllListeners();
            }
        }
    }

    public void loadScene()
    {
        SceneManager.LoadSceneAsync(sceneInfo.sceneName);
    }

}
