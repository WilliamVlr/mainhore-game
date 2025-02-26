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

public class SoundManager : MonoBehaviour, IDataPersistence
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    private float lastMusicVol;
    private float lastSfxVol;

    [Header("Audio List Optional")]
    public Sound[] musicList;
    public Sound[] sfxList;

    public static SoundManager Instance;
    public bool _musicIsMuted;
    public bool _sfxIsMuted;

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
        _musicIsMuted = musicSource.mute;
        _sfxIsMuted = sfxSource.mute;
        lastMusicVol = musicSource.volume;
        lastSfxVol = sfxSource.volume;
        PlayMusicInList("Start");
    }

    public float getCurrentMusicVol()
    {
        return musicSource.volume;
    }
    public float getCurrentSfxVol()
    {
        return sfxSource.volume;
    }

    public void StopSFX()
    {
        sfxSource?.Stop();
    }

    public void LoopSFX()
    {
        sfxSource.loop = true;
    }

    public void UnloopSFX()
    {
        sfxSource.loop = false;
    }

    private void loadUITrigger()
    {
        SoundUIController soundUI = FindAnyObjectByType<SoundUIController>();
        if (soundUI != null)
        {
            soundUI.loadUI();
        }
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
        SaveLastVol(_musicIsMuted, musicSource.volume, ref lastMusicVol);
        musicSource.mute = !musicSource.mute;
        _musicIsMuted = musicSource.mute;
        if (!_musicIsMuted) LoadLastVol(musicSource, lastMusicVol);
    }

    public void ToggleSFX()
    {
        SaveLastVol(_sfxIsMuted, sfxSource.volume, ref lastSfxVol);
        sfxSource.mute = !sfxSource.mute;
        _sfxIsMuted = sfxSource.mute;
        if (!_sfxIsMuted) LoadLastVol(sfxSource, lastSfxVol);
    }

    private void SaveLastVol(bool isMuted, float vol, ref float lastVol)
    {
        if (!isMuted)
        {
            lastVol = vol;
        }
    }

    private void LoadLastVol(AudioSource source, float lastVol)
    {
        source.volume = lastVol;
        loadUITrigger();
    }

    public void AdjustMusicVolume(float vol)
    {
        musicSource.volume = vol;
        loadUITrigger();
    }

    public void AdjustSFXVolume(float vol)
    {
        sfxSource.volume = vol;
        loadUITrigger();
    }

    public void LoadData(GameData data)
    {
        loadUITrigger();
    }

    public void SaveData(ref GameData data)
    {
        //
    }
}