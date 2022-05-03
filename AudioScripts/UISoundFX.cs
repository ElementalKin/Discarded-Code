using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISoundFX : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public SoundControls SoundOnHover;
    public SoundControls SoundOnClick;

    public bool onClickDisable = false;
    public bool onHoverDisable = false;

    public bool OnEnableClick = false;

    [SerializeField] AudioCollection audCollection;

    private void OnEnable()
    {
        if (OnEnableClick == true)
        {
            //Debug.Log("Pointer Down UI");
            AudioManager.instance.UISounds(SoundOnClick);
            //FindObjectOfType<AudioManager>().instanceAudioManager();
            //FindObjectOfType<AudioManager>().PlayUISound(SoundOnClick);
        }
    }

    // This one is the CLICK
    public void OnPointerDown(PointerEventData eventData)
    {
        if (onClickDisable == false)
        {
            //Debug.Log("Pointer Down UI");
            AudioManager.instance.UISounds(SoundOnClick);
            //FindObjectOfType<AudioManager>().instanceAudioManager();
            //FindObjectOfType<AudioManager>().PlayUISound(SoundOnClick);
        }
    }

    // This one is the JUST HOVERED OVER
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (onHoverDisable == false)
        {
            //Debug.Log("Pointer Hover UI");
            AudioManager.instance.UISounds(SoundOnHover);
            //FindObjectOfType<AudioManager>().instanceAudioManager();
            //FindObjectOfType<AudioManager>().PlayUISound(SoundOnHover);
        }
    }
}