using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnergyFlower : MonoBehaviour
{
    public List<AudioClip> energyFlowerAudio;
    public GameObject energyFlower;
    public int randomVoice;
    private AudioSource audioSources;
    void Start()
    {
        audioSources = GetComponent<AudioSource>();
        randomVoice = Random.Range(0, energyFlowerAudio.Count);
        energyFlower.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //碰到能量花就加能量
        if (collision.gameObject.CompareTag("Player"))
        {
            //audioSources.PlayOneShot(energyFlowerAudio[randomVoice]);
            GameDb.energy++;
            //energyFlower.SetActive(false);
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        //audioSources.PlayOneShot(energyFlowerAudio[randomVoice]);
        audioSources.Play();
    }
}
