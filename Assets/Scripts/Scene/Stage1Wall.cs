using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Wall : MonoBehaviour
{
    private BoxCollider2D collider2D;
    void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
        collider2D.isTrigger = false;
    }

    
    void Update()
    {
        if (GameDb.touchWaterhole && GameDb.touchSand)
        {
            collider2D.isTrigger = true;
        }
    }
}
