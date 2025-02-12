using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUIController : MonoBehaviour
{
    [Header("Buttons")]
    public Button musicButton;
    public Button sfxButton;

    [Header("Slider")]
    public Slider _musicSlider;
    public Slider _sfxSlider;

    public void ToggleMusic()
    {
        SoundManager.Instance.ToggleMusic();
        musicButton.interactable = !musicButton.interactable;
    }

    public void ToggleSFX()
    {
        SoundManager.Instance.ToggleSFX();
        sfxButton.interactable = !sfxButton.interactable;
    }

    public void AdjustMusicVolume()
    {
        SoundManager.Instance.AdjustMusicVolume(_musicSlider.value);
    }

    public void AdjustSFXVolume()
    {
        SoundManager.Instance.AdjustSFXVolume(_sfxSlider.value);
    }
}
