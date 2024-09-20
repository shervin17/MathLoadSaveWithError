using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiomanager : MonoBehaviour
{
    [Header("------Audio Source------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;


    [Header("------Audio clip------")]
    public AudioClip background;


    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
}
