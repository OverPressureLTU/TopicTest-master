using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public GameObject leaf;
    public GameObject vine;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            Instantiate(leaf, transform.position + new Vector3(6f, 9f, 0), transform.rotation);
            Debug.Log("Leaf");
        }

        if (other.gameObject.CompareTag("Attack"))
        {
            Destroy(gameObject);
            Instantiate(vine, transform.position, transform.rotation);
            Debug.Log("Vine");
        }
    }
}
