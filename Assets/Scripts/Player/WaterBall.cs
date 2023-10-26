using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBall : MonoBehaviour
{

    void Update()
    {
        transform.position += new Vector3(0.2f, 0, 0);
       
        StartCoroutine("Disappear");
    }
    
     void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject)
        {
            Destroy(gameObject);
        }
    }
    
    public IEnumerator Disappear()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
