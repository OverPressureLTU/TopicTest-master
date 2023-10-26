using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class S0Mgr : MonoBehaviour
{
    public Button btnStart;         //開始遊戲按鈕
    public Button btnLoad;          //載入遊戲按鈕
    public Button btnSetting;       //設定按鈕
    public Button btnExit;          //離開遊戲按鈕
    public Button btnCloseLoad;     //關閉載入視窗按鈕
    public Button btnCloseSetting;  //關閉設定視窗按鈕
    public Slider slidMusic;        //音樂大小滑桿
    public Slider slidSound;        //音效大小滑桿
    public GameObject laodView;     //載入視窗
    public GameObject settingView;  //設定視窗
    public AudioSource music;       //音樂大小
    public AudioSource sound;       //音效大小
    public AudioSource btnClickVoice;
    
    void Start()
    {
        laodView.SetActive(false);
        settingView.SetActive(false);
        slidMusic.value = music.volume = GameDb.musicVolum;
        slidSound.value = GameDb.soundVolum *10;
        btnStart.onClick.AddListener(OnBtnStartClick);
        btnLoad.onClick.AddListener(OnBtnLoadClick);
        btnSetting.onClick.AddListener(OnBtnSettingClick);
        btnExit.onClick.AddListener(OnBtnExitClick);
        btnCloseLoad.onClick.AddListener(OnBtnCloseClick);
        btnCloseSetting.onClick.AddListener(OnBtnCloseClick);
        slidSound.onValueChanged.AddListener(OnSlidSoundValueChange);
        //slidMusic.value = 0.5f;
    }
    void Update()
    {
        music.volume = slidMusic.value;
        sound.volume = btnClickVoice.volume = slidSound.value / 10f;
    }

    void OnSlidSoundValueChange(float value)
    {
        if (Input.GetMouseButtonDown(0))
        {
            sound.Play();
        }
    }
    void OnBtnStartClick()      //點擊開始按鈕
    {
        btnClickVoice.Play();
        SceneManager.LoadScene("Loading");
    }
    void OnBtnLoadClick()       //點擊載入按鈕
    {
        btnClickVoice.Play();
        laodView.SetActive(true);
    }
    void OnBtnSettingClick()    //點擊設定按鈕
    {
        btnClickVoice.Play();
        settingView.SetActive(true);
    }
    void OnBtnExitClick()       //點擊離開按鈕
    {
        btnClickVoice.Play();
        Application.Quit();
    }
    void OnBtnCloseClick()      //點擊關閉按鈕
    {
        btnClickVoice.Play();
        laodView.SetActive(false);
        settingView.SetActive(false);
    }
}
