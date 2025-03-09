using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public CanvasBehavior PausePanel;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        PausePanel.showCanvas();
        SoundManager.Instance.StopMusic();
        Time.timeScale = 0;
    }

    public void Continue()
    {
        PausePanel.hideCanvas();
        SoundManager.Instance.ContinueMusic();
        Time.timeScale = 1;
    }
}
