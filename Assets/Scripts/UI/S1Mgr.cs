using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class S1Mgr : MonoBehaviour
{
    //關卡索引
    public string levelIndex;
    public List<Image> hpList;      //血量列表
    public List<Image> energyList;  //能量列表
    public Slider slidStamina;      //耐力條
    public Slider slidMusic;
    public Slider slidSound;
    public Button btnReplay;        //重玩按鈕
    public Button btnExit;          //離開按鈕
    public Button btnMenu;          //主畫面按鈕
    public Button btnIce;           //冰型態按鈕
    public Button btnWater;         //水型態按鈕
    public Button btnSteam;         //蒸氣型態按鈕
    public GameObject deadView;     //死亡視窗
    public GameObject settingView;  //設定視窗
    public GameObject typeChange;   //型態轉換畫面
    public AudioSource music;       //音樂大小
    public AudioSource sound;       //音效大小
    public PlayerCtrl playerctrl;

    private bool isSettingViewOpen;

    void Start()
    {
        slidMusic.value = music.volume = GameDb.musicVolum;
        slidSound.value = GameDb.soundVolum * 10;
        slidStamina.value = 30;
        Time.timeScale = 1;
        GameDb.energy = 0 ;
        GameDb.hp = 1;
        deadView.SetActive(false);
        settingView.SetActive(false);
        typeChange.SetActive(false);
        btnReplay.onClick.AddListener(OnBtnReplayClick);
        btnExit.onClick.AddListener(OnBtnExitClick);
        btnMenu.onClick.AddListener(OnBtnMenuClick);
        btnIce.onClick.AddListener(OnBtnIceClick);
        btnWater.onClick.AddListener(OnBtnWaterClick);
        btnSteam.onClick.AddListener(OnBtnSteamClick);
        slidSound.onValueChanged.AddListener(OnslidSoundValueChange);
    }
    
    void Update()
    {
        music.volume = slidMusic.value;
        sound.volume = slidSound.value / 10f;

        for (int i = 0; i < 5; i++)
        {
            if (i > GameDb.hp - 1)
            {
                hpList[i].gameObject.SetActive(false);
            }
            else
            {
                hpList[i].gameObject.SetActive(true);
            }
        }

        for (int i = 0; i < 10; i++)
        {
            if (i > GameDb.energy - 1)
            {
                energyList[i].gameObject.SetActive(false);
            }
            else
            {
                energyList[i].gameObject.SetActive(true);
            }
        }
        //按Esc鍵開啟設定視窗
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isSettingViewOpen = !isSettingViewOpen;
            if (isSettingViewOpen)
            {
               settingView.SetActive(true);
            }
            else
            {
                settingView.SetActive(false);
            }
        }
        //如果耐力不是全滿，則每秒恢復5耐力
        if (slidStamina.value < 30)
        {
            slidStamina.value += 5 * Time.deltaTime;
        }
        if (GameDb.energy > 10)
        {
            GameDb.energy = 10;
        }
        
        if(GameDb.energy == 10 && Input.GetKey(KeyCode.Tab))
        {
            Time.timeScale = 0;
            typeChange.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            typeChange.SetActive(false);
        }
        if (GameDb.hp == 0)
        {
            Dead();
            playerctrl.waterDieVoice.Play();
        }
    }

    
 

    void OnBtnReplayClick()
    {
        SceneManager.LoadScene(levelIndex);
    }
    void OnBtnExitClick()
    {
        Application.Quit();
    }
    void OnBtnMenuClick()
    {
        SceneManager.LoadScene("S0");
    }
    void OnBtnIceClick()
    {
        GameDb.energy = 0;
        playerctrl.isIce = true;
        playerctrl.isWater = false;
        playerctrl.isSteam = false;
        playerctrl.waterToIceVoice.Play();
    }
    void OnBtnWaterClick()
    {
        GameDb.energy = 0;
        playerctrl.isWater = true;
        playerctrl.isIce = false;
        playerctrl.isSteam = false;
    }
    void OnBtnSteamClick()
    {
        GameDb.energy = 0;
        playerctrl.isSteam = true;
        playerctrl.isIce = false;
        playerctrl.isWater = false;
        playerctrl.waterToSteamVoice.Play();
    }

    public void Dead()
    {
        Time.timeScale = 0;
        deadView.SetActive(true);
    }
    
    void OnslidSoundValueChange(float value)
    {
        if (Input.GetMouseButtonDown(0))
        {
            sound.Play();
        }
    }
}
