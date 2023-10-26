using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrainMgr : MonoBehaviour
{
    public int index = 0;
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
    public Text txtGuide;
    public GameObject deadView;     //死亡視窗
    public GameObject settingView;  //設定視窗
    public GameObject typeChange;   //型態轉換畫面
    public AudioSource music;       //音樂大小
    public AudioSource sound;       //音效大小
    public Train_PlayerCtrl playerctrl;
   
    private bool isLeft= false;
    private bool isRight = false;
    private bool isJump = false;
    private bool isSettingViewOpen;
   
       void Start()
       {
           slidMusic.value = music.volume = GameDb.musicVolum;
           slidSound.value = GameDb.soundVolum * 10;
           slidStamina.value = 30;
           Time.timeScale = 1;
           GameDb.energy = 0 ;
           GameDb.hp = 1;
           playerctrl.enabled = false;
           txtGuide.text = "嗨！你好哇，小水滴！ 這裡是神樹內部的樹洞，我會在這裡教你怎麼使用能力！ 雖然你才剛出生而已，但是你被神樹大人創造出來就是為了拯救這裡...那麼事不宜遲！";
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

           if (Input.GetMouseButtonDown(0))
           {
               if (index ==0)
               {
                   index++;
                   return;
               }
               
               if (index ==1)
               {
                   txtGuide.text = "首先來教你怎麼走路吧！嗯...水滴會走路嗎？算了！鍵盤A、D鍵可以左右移動，你試試看吧？";
                   index++;
                   return;
               }
               if (index ==2)
               {
                   txtGuide.text = "按AD移動，空白鍵跳躍(0/1)";
                   index++;
                   playerctrl.enabled = true;                                                              
                   return;
               }
           }

           if (playerctrl.rb.position.x < 0)
           {
               isLeft = true;
           }
           if (playerctrl.rb.position.x > 0)
           {
               isRight = true;
           }

           if (Input.GetKeyDown(KeyCode.Space))
           {
               isJump = true;
           }
           
           if (index == 3 && isLeft && isRight && isJump)
           {
               txtGuide.text = "做得好！你已經學會移動了！";
               index++;
           }
           
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
           SceneManager.LoadScene("Train");
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
