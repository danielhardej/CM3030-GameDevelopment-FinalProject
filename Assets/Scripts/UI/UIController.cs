/*
* Written by Safa Chawich.
* Following this tutorial https://youtu.be/lF26yGJbsQk
* This class manages the behaviour of the UI elements (swap sprite and play audio).
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    [SerializeField] private Image _img;
    [SerializeField] private Sprite _hover, _default;
    [SerializeField] private AudioClip _hoverClip, _pressClip;
    [SerializeField] private AudioSource _source;
    public void OnPointerDown(PointerEventData eventData)
    {
        _source.PlayOneShot(_pressClip);
        _img.sprite = _default;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _img.sprite = _hover;
        _source.PlayOneShot(_hoverClip);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _img.sprite = _default;
    }
}
