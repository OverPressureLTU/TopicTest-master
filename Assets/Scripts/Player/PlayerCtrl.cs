using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TerrainTools;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    #region public
    public float dashSpeed;                //衝刺速度
    public S1Mgr s1;
    public GameObject waterBall;           //水球
    public Slider slidSound;

    [Header("Jump")]
    public float jumpForce;                //跳躍高度

    [Header("Type")]
    public bool isWater;
    public bool isIce;
    public bool isSteam;
    
    [Header(("Audio"))]
    public List<AudioSource> waterAudio;
    public List<AudioSource> iceAudio;
    public List<AudioSource> steamAudio;
    public AudioSource waterDieVoice;      //水死亡聲
    public AudioSource waterToIceVoice;    //結冰聲
    public AudioSource waterToSteamVoice;  //蒸發聲
    #endregion

    #region private
    
    private CapsuleCollider2D capsuleCollider2D;
    private Rigidbody2D rb;
    private Animator animator;
    private float timeType;                 //型態持續時間
    private bool isTouchVine;               //碰到藤蔓
    private bool isHurt;                    //受傷
    private AudioSource waterMoveVoice;     //水移動聲
    private AudioSource waterJumpVoice;     //水跳躍聲
    private AudioSource waterInGroundVoice; //水落地聲
    private AudioSource waterHurtVoice;     //水受傷聲
    
    private AudioSource iceMoveVoice;       //冰移動聲
    private AudioSource iceToWaterVoice;    //冰融化聲

    private AudioSource steamMoveVoice;     //蒸氣移動聲
    private AudioSource steamToWaterVoice;  //凝結聲

    #endregion
    
    void Start()
    {
        capsuleCollider2D = gameObject.GetComponent<CapsuleCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        transform.localScale = new Vector3(1f, 1f, 1f);
        waterMoveVoice = waterAudio[0];
        waterJumpVoice = waterAudio[1];
        waterInGroundVoice = waterAudio[2];
        waterHurtVoice = waterAudio[3];
        waterDieVoice = waterAudio[4];
        
        waterToIceVoice = iceAudio[0];
        iceMoveVoice = iceAudio[1];
        iceToWaterVoice = iceAudio[2];

        waterToSteamVoice = steamAudio[0];
        steamMoveVoice = steamAudio[1];
        steamToWaterVoice = steamAudio[2];
        
        isTouchVine = false;
        isHurt = false;
        isWater = true;
    }

    void FixedUpdate()
    {
        if (isWater)
        {
            WaterType();
            animator.SetBool("SteamType", false);
            animator.SetBool("IceType", false);
            //animator.Play("Water1_Idle");
            s1.btnWater.interactable = false;
        }
        else
        {
            s1.btnWater.interactable = true;
        }
        if (isIce)
        {
            IceType();
            animator.SetBool("IceType", true);
            s1.btnIce.interactable = false;
        }
        else
        {
            s1.btnIce.interactable = true;
        }
        if (isSteam)
        {
            SteamType();
            animator.SetBool("SteamType", true);
            s1.btnSteam.interactable = false;
        }
        else
        {
            s1.btnSteam.interactable = true;
        }
    }

    void Update()
    {
        if (GameDb.hp > 5)
        {
            GameDb.hp = 5;
        }
        //讓所有音量變為設定的音量
        waterDieVoice.volume = waterHurtVoice.volume = waterInGroundVoice.volume = waterJumpVoice.volume
            = waterMoveVoice.volume = waterToIceVoice.volume = waterToSteamVoice.volume = iceMoveVoice.volume
            = iceToWaterVoice.volume = steamMoveVoice.volume = steamToWaterVoice.volume = slidSound.value / 10f;
    }

    #region 碰撞觸發
    void OnCollisionEnter2D(Collision2D col)
    {
        //水碰到tag冰變成冰
        if (isWater && col.gameObject.CompareTag("Ice"))
        {
            isIce = true;
            isWater = false;
            isSteam = false;
            waterToIceVoice.Play();
        }

        //水碰到tag火變蒸氣
        if (isWater && col.gameObject.CompareTag("Fire"))
        {
            isSteam = true;
            isWater = false;
            isIce = false;
            waterToSteamVoice.Play();
        }
        //碰到地板或陷阱可跳躍
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Trap"))
        {
            GameDb.canJump = true;
            //waterInGroundVoice.Play();
        }
        //碰到陷阱或敵人就變小
        if (isWater && (col.gameObject.CompareTag("Trap") || col.gameObject.CompareTag("Enemy")))
        {
            StartCoroutine("HurtCD");
            if (isHurt)
            {
                return;
            }
            GameDb.hp--;
            transform.localScale -= new Vector3(0.2f, 0.2f, 0f);
            waterHurtVoice.Play();
            isHurt = true;
        }
        //撞到任何東西都停止慣性
        if (col.gameObject)
        {
            rb.velocity = Vector2.zero;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //水型態碰到藤蔓可吸附在上面
        if (isWater && collision.gameObject.CompareTag("Vine"))     
        {
            transform.SetParent(collision.gameObject.transform);
            rb.velocity = Vector2.zero;
            isTouchVine = true;
            OnVine();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (isWater && collision.gameObject.CompareTag("Vine"))
        {
            isTouchVine = false;
        }
        if (collision.gameObject)
        {
            GameDb.canJump = false;
        }

        if (isWater && collision.gameObject.CompareTag("Trap"))
        {
            StopCoroutine("HurtCD");
            GameDb.touchSand = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //水碰到tag冰變成冰
        if (isWater && collision.gameObject.CompareTag("Ice"))
        {
            isIce = true;
            isWater = false;
            isSteam = false;
            timeType = 0;
            waterToIceVoice.Play();
        }

        //水碰到tag火變蒸氣
        if (isWater && collision.gameObject.CompareTag("Fire"))
        {
            isSteam = true;
            isWater = false;
            isIce = false;
            timeType = 0;
            waterToSteamVoice.Play();
        }
        //冰碰火或火碰冰會變水
        if ((isSteam && !collision.gameObject.CompareTag("Fire")) ||
            (isIce && collision.gameObject.CompareTag("Fire")))
        {
            if (isSteam && !collision.gameObject.CompareTag("Fire"))
            {
                steamToWaterVoice.Play();
            }
            if (isIce && collision.gameObject.CompareTag("Fire"))
            {
                iceToWaterVoice.Play();
            }
            isWater = true;
            isIce = false;
            isSteam = false;
            timeType = 0;
        }
        
        //水蒸氣碰到爆炸花就死掉
        if (isSteam && collision.gameObject.CompareTag("Explode"))
        {
            GameDb.hp = 0;
            waterDieVoice.Play();
        }

        if (collision.gameObject.CompareTag("Save") && Input.GetKeyDown(KeyCode.S))
        {
            //PlayerPrefs.Save();
        }

        if (collision.gameObject.CompareTag("WaterHole"))
        {
            //enterWaterVoice.Play();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject)
        {
            GameDb.canJump = false;
        }
    }
    #endregion

    #region Type
    public void WaterType() //水型態
    {
        //animator.Play("Water1_Idle");
        
        capsuleCollider2D.isTrigger = false;
        if (isTouchVine)
        {
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 1;
        }
        
        if (!Input.GetButton("Horizontal"))
        {
            waterMoveVoice.Play();
        }
        
        if (Input.GetKey(KeyCode.A))    //按A向左移動
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (s1.slidStamina.value < 10)
                {
                    return;
                }
                s1.slidStamina.value -= 10;
                //transform.Translate(transform.right * Time.deltaTime * -dashSpeed);
                rb.velocity = new Vector2(-5, rb.velocity.y);
            }
            transform.position -= new Vector3(0.1f, 0f, 0f);
            animator.SetBool("LeftWalking", true);
            //waterMoveVoice.Play();

            //animator.Play("Water1_Walk_Left");
        }
        else
        {
            animator.SetBool("LeftWalking", false);
            //waterMoveVoice.Play();
            //animator.Play("Water1_Idle");
        }

        if (Input.GetKey(KeyCode.D))
        {   
            if (Input.GetMouseButtonDown(1))
            {
                if (s1.slidStamina.value < 10)
                {
                    return;
                }
                s1.slidStamina.value -= 10;
                //transform.Translate(transform.right * Time.deltaTime * dashSpeed);
                rb.velocity = new Vector2(5, rb.velocity.y);
            }
            transform.position += new Vector3(0.1f, 0f, 0f);
            animator.SetBool("RightWalking", true);
            //animator.Play("Water1_Walk_Right");
        }
        else
        {
            animator.SetBool("RightWalking", false);
            //animator.Play("Water1_Idle");
        }
        //Attack();
        Jump();
        Jump();
    }

    public void IceType()   //冰型態
    {
        timeType += 1f * Time.deltaTime;
        if (timeType > 10f)     //如果時間超過10秒就變回水滴
        {
            timeType = 0f;
            isIce = false;
            isWater = true;
            animator.SetBool("IceType", false);
            iceToWaterVoice.Play();
        }
        
        rb.gravityScale = 1.5f;
        //移動
        if (Input.GetKey(KeyCode.A))    //按A向左移動
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (s1.slidStamina.value < 10)
                {
                    return;
                }
                s1.slidStamina.value -= 10;
                transform.Translate(transform.right * Time.deltaTime * -dashSpeed);
            }
            rb.velocity = new Vector2(-5, rb.velocity.y);
        }
        if (Input.GetKey(KeyCode.D))    //按D向右移動
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (s1.slidStamina.value < 10)
                {
                    return;
                }
                s1.slidStamina.value -= 10;
                transform.Translate(transform.right * Time.deltaTime * dashSpeed);
            }
            rb.velocity = new Vector2(5, rb.velocity.y);
        }
        Jump(); 
    }

    public void SteamType() //蒸氣型態
    {
        timeType += 1f * Time.deltaTime;
        if (timeType > 5f)
        {
            timeType = 0f;
            isSteam = false;
            isWater = true;
            animator.SetBool("SteamType", false);
            steamToWaterVoice.Play();
        }
        
        capsuleCollider2D.isTrigger = true;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(rb.velocity.x, 7);
        //移動
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(0.08f, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(0.08f, 0f, 0f);
        }
    }
    #endregion

    public IEnumerator HurtCD()
    {
        yield return new WaitForSeconds(1f);
        GameDb.hp--;
        waterHurtVoice.Play();
    }
    
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && GameDb.canJump)
        {
            rb.AddForce(transform.up * jumpForce);
            if (isWater)
            {
                waterJumpVoice.Play();
            }
        }

        if (/*rb.velocity.y > 0f &&*/ Input.GetButtonDown("Jump"))      //力向上時播放向上跳動畫
        {
            animator.SetBool("JumpingUp", true);
            //animator.Play("Water1_Jump");
        }
        
        //原始跳躍會緊接著降落，暴力播放會帶來更多的卡頓問題。
        else
        {
            animator.SetBool("JumpingUp", false);
            //animator.Play("Water1_Idle");
        }
        if (rb.velocity.y < -0.1f)      //力向下時播放播放降落動畫
        {
            animator.SetBool("JumpingFall", true);
            //animator.Play("Water1_Fall");
        }
        else
        {
            animator.SetBool("JumpingFall", false);
            //animator.Play("Water1_Idle");
        }
    }

    /*void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(waterBall, transform.position + new Vector3(1.5f, 0, 0), transform.rotation);
        }
    }*/
    void OnVine()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0f, 0.1f, 0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(0f, 0.1f, 0f);
        }
    }
}
