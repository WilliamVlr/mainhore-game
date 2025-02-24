using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUIController : MonoBehaviour
{
    [Header("Buttons")]
    public Button musicButton;
    [SerializeField] private Sprite musicSprite;
    [SerializeField] private Sprite musicSprite_mute;
    public Button sfxButton;
    [SerializeField] private Sprite sfxSprite;
    [SerializeField] private Sprite sfxSprite_mute;

    [Header("Slider")]
    public Slider _musicSlider;
    public Slider _sfxSlider;

    public void ToggleMusic()
    {
        SoundManager.Instance.ToggleMusic();
        loadUI();
    }

    public void ToggleSFX()
    {
        SoundManager.Instance.ToggleSFX();
        loadUI();
    }

    public void AdjustMusicVolume()
    {
        SoundManager.Instance.AdjustMusicVolume(_musicSlider.value);
    }

    public void AdjustSFXVolume()
    {
        SoundManager.Instance.AdjustSFXVolume(_sfxSlider.value);
    }

    public void loadUI()
    {
        if (SoundManager.Instance._musicIsMuted)
        {
            musicButton.image.sprite = musicSprite_mute;
            _musicSlider.value = 0f;
        }
        else
        {
            musicButton.image.sprite = musicSprite;
            _musicSlider.value = SoundManager.Instance.getCurrentMusicVol();
        }

        if (SoundManager.Instance._sfxIsMuted)
        {
            sfxButton.image.sprite = sfxSprite_mute;
            _sfxSlider.value = 0f;
        }
        else
        {
            sfxButton.image.sprite = sfxSprite;
            _sfxSlider.value = SoundManager.Instance.getCurrentSfxVol();
        }
    }
}