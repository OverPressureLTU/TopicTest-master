using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneName : MonoBehaviour
{
    public Text txtSceneName;
    void Start()
    {
        txtSceneName.color = new Color(1, 1, 0, 1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.name == "GameObject" && other.gameObject.CompareTag("Player"))
        {
            txtSceneName.text = "神樹之泉";
        }
        if (gameObject.name == "GameObject (1)" && other.gameObject.CompareTag("Player"))
        {
            txtSceneName.text = "冰封入口";
        }
        if (gameObject.name == "GameObject (2)" && other.gameObject.CompareTag("Player"))
        {
            txtSceneName.text = "精靈隧道";
        }
        if (gameObject.name == "GameObject (3)" && other.gameObject.CompareTag("Player"))
        {
            txtSceneName.text = "靜謐洞穴";
        }
        if (gameObject.name == "GameObject (4)" && other.gameObject.CompareTag("Player"))
        {
            txtSceneName.text = "沙枯之地";
        }
        if (gameObject.name == "GameObject (5)" && other.gameObject.CompareTag("Player"))
        {
            txtSceneName.text = "熱氣峽谷";
        }
        if (gameObject.name == "GameObject (6)" && other.gameObject.CompareTag("Player"))
        {
            txtSceneName.text = "赤焰入口";
        }
    }
}
