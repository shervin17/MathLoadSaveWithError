using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;
    public void ToggleMusic()
    {
        AudiomanagerFlip.Instance.ToggleMusic();
    }
    public void ToggleSFX()
    {
        AudiomanagerFlip.Instance.ToggleSFX();
    }

    public void MusicVoume()
    {
        AudiomanagerFlip.Instance.MusicVolume(_musicSlider.value);
    }
    public void SFXVoume()
    {
        AudiomanagerFlip.Instance.SFXVolume(_sfxSlider.value);
    }

}
