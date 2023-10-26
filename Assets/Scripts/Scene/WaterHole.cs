using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterHole : MonoBehaviour
{
    public PlayerCtrl PlayerCtrl;
    public Train_PlayerCtrl trainPlayerCtrl;
    public GameObject player;
    public List<AudioSource> waterHoleAudio;
    public Slider slidSound;
    
    private AudioSource enterWaterVoice;
    private AudioSource swimVoice;
    private bool isTouchwater;

    void Start()
    {
        isTouchwater = false;
        enterWaterVoice = waterHoleAudio[0];
        swimVoice = waterHoleAudio[1];
    }

    void Update()
    {
        enterWaterVoice.volume = swimVoice.volume = slidSound.value / 10f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            GameDb.touchWaterhole = true;
            enterWaterVoice.Play();
            GameDb.canJump = true;
            if (isTouchwater)
            {
                return;
            }
            
            GameDb.hp++;
            player.transform.localScale += new Vector3(0.2f, 0.2f, 0);
            isTouchwater = true;
        }
    }
}
