using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class btn : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource btnTouchVoice;
    public Slider slidSound;

    void Update()
    {
        btnTouchVoice.volume = slidSound.value / 10f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        btnTouchVoice.Play();
    }
}
