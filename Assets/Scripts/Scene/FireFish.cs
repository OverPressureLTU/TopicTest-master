using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFish : MonoBehaviour
{
    public Transform left;
    public Transform right;
 
    private Rigidbody2D rb;
 
    private bool walkLeft = true; //是否往左走
 
    void Start()
    {
        this.transform.DetachChildren();
 
        rb = this.GetComponent<Rigidbody2D>(); //抓元件使用
    }
 
    void Update()
    {
        // 判斷
        if (left.localPosition.x > this.transform.localPosition.x)
        {
            //Debug.Log("超出左邊界!!");
            walkLeft = false;
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (right.localPosition.x < this.transform.localPosition.x)
        {
            // Debug.Log("超出右邊界!!");
            walkLeft = true;
            transform.localScale = new Vector3(-1, 1, 1);
        }
         
        //執行
        if (walkLeft)
        {
            rb.velocity = new Vector2(-5, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(5, rb.velocity.y);
        }
    }
}
