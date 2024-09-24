using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class clickbuttonFlip : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _img;
    [SerializeField] private AudioClip _compressedClip,_uncompressedClip; 
    [SerializeField] private AudioSource _source;

    public void OnPointerDown(PointerEventData eventData)
    {
      

        // Play the compressed audio clip
        _source.PlayOneShot(_compressedClip);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        

        // Play the uncompressed audio clip
        _source.PlayOneShot(_uncompressedClip);
    }


}
