using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio List Optional")]
    public Sound[] musicList;
    public Sound[] sfxList;

    public static SoundManager Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        PlayMusicInList("Start");
    }

    public void PlayMusicInList(string name)
    {
        Sound s = Array.Find(musicList, (x) => x.name == name);

        if(s != null)
        {
            PlayMusic(s.clip);
        }
    }

    public void PlaySFXInList(string name)
    {
        Sound s = Array.Find(sfxList, (x) => x.name == name);

        if (s != null)
        {
            PlaySFX(s.clip);
        }
    }

    public void PlayMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void StopMusic()
    {
        musicSource?.Stop();
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void AdjustMusicVolume(float vol)
    {
        musicSource.volume = vol;
    }

    public void AdjustSFXVolume(float vol)
    {
        sfxSource.volume = vol;
    }

}
