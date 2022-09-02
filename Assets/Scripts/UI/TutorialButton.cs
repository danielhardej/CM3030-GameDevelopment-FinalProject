using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class TutorialButton : Image
{
    Sprite defaultSprite;
    AudioSource audioSource;

    [Header("Custom Properties")]
    [SerializeField]
    public Sprite highlightedSprite;

    protected override void Start()
    {
        base.Start();

        defaultSprite = this.sprite;
        audioSource = GetComponent<AudioSource>();
    }

    public void SetHighlighted()
    {
        if(this.isActiveAndEnabled)
        {
            this.sprite = highlightedSprite;
            audioSource.Play();
        }
        
    }

    public void SetLowlighted()
    {
        if(this.isActiveAndEnabled)
        {
            this.sprite = defaultSprite;
        }
    }

}
