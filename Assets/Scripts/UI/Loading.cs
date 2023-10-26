using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Slider slidLoading;
    public Text txtProcess;
    public int loadProgress;
    void Start()
    {
        StartCoroutine("LoadScene");
    }

    public IEnumerator LoadScene()
    {
        
        AsyncOperation async = SceneManager.LoadSceneAsync("S1");
        loadProgress = (int)async.progress * 100;
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            slidLoading.value = async.progress;
            txtProcess.text = slidLoading.value * 100 + " %";
            //Debug.Log("000");
            if (async.progress >=0.88f)
            {
                break;
            }
            yield return new WaitForEndOfFrame(); 
        }
        Debug.Log("強制100");
        loadProgress = 100;
        slidLoading.value = 100;
        txtProcess.text = slidLoading.value * 100 + " %";
        async.allowSceneActivation = true;
       
    }
}
