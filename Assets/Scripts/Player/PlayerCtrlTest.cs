using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrlTest : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float moveInput;

    /*
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 100f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    */
    private Rigidbody2D rb;
    private bool isGrounded;
    
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private Animator Anime;
    
    // Start is called before the first frame update
    void Start()
    {
        Anime = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (Input.GetKey(KeyCode.A))
        {
            Anime.SetBool("moveLeft",true);
            //Anime.SetBool("moveRight",false);
        }
        else
        {
            Anime.SetBool("moveLeft",false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            //Anime.SetBool("moveLeft",false);
            Anime.SetBool("moveRight",true);
        }
        else
        {
            Anime.SetBool("moveRight",false);
        }
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        
    }

    private void Update()
    {
        /*
        if (isDashing)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        */        
        if ((Input.GetKeyDown(KeyCode.Space)) && isGrounded==true)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * speed);
            Anime.SetTrigger("takeOff");
        }

        if (isGrounded == true)
        {
            Anime.SetBool("falling",false);
        }
        else
        {
            Anime.SetBool("falling",true);
        }
    }
    /*
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        //tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        //tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    */
}
