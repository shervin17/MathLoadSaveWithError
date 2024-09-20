using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class clickbutton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _img;
    [SerializeField] private Sprite _default, _pressed;
    [SerializeField] private AudioClip _compressedClip,_uncompressedClip; 
    [SerializeField] private AudioSource _source;

    public void OnPointerDown(PointerEventData eventData)
    {
        // Change the image to the pressed sprite
        _img.sprite = _pressed;

        // Play the compressed audio clip
        _source.PlayOneShot(_compressedClip);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Change the image back to the default sprite
        _img.sprite = _default;

        // Play the uncompressed audio clip
        _source.PlayOneShot(_uncompressedClip);
    }


}
