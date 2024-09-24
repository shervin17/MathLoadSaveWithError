using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import for Button and Image

public class AudiomanagerFlip : MonoBehaviour
{
    public static AudiomanagerFlip Instance;
    public sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    public Button musicToggleButton;  // Reference to the music toggle button
    public Color activeColor = Color.white; // Color for active state
    public Color mutedColor = Color.gray;   // Color for muted state

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name)
    {
        sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Music not found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("SFX not found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;

        // Change the button's color based on the mute state
        if (musicSource.mute)
        {
            // Muted, darken or hide the button
            musicToggleButton.image.color = mutedColor;
        }
        else
        {
            // Not muted, set to active color
            musicToggleButton.image.color = activeColor;
        }
    }
    public void StopAllAudio()
    {
        // Stop the music and SFX sources
        musicSource.Stop();
        sfxSource.Stop();
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
