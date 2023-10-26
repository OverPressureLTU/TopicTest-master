using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public Transform barrelTip;
    public GameObject shell;
    private Vector2 lookDirection;
    private float lookAngle;

    public float fireRate = 1f;
    public float nextRound = 0f;

    //public AudioSource FireSound;
    

    void Update()
    {
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,lookAngle-90f);

        if (Input.GetMouseButton(0) && Time.time > nextRound)
        {
            //Debug.Log("On fire!");
            nextRound = Time.time + fireRate;
            Fire();
        }

    }

    void Fire()
    {
        GameObject fire = Instantiate(shell, barrelTip.position, barrelTip.rotation);
        fire.GetComponent<Rigidbody2D>().velocity = barrelTip.up * 15f;
        //FireSound.Play();
    }
}
