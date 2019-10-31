using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAOETotem : MonoBehaviour
{
    // timer variables
    public float totalTotemActiveTime = 15f;
    private float timer;

    public float timeBetweenAOE = 1.5f;
    private float AOETimer;
    
    private Transform firingPoint;
    public GameObject AOE;
    AudioSource audioSource;


    // Setup the object
    void Start()
    {
        firingPoint = gameObject.transform.Find("Spawn Point").transform;
        timer = totalTotemActiveTime;
        AOETimer = timeBetweenAOE;
        audioSource = gameObject.transform.parent.GetComponent<AudioSource>();
    }

    // Check the timers, either perform an attack or, if the total timer has run up, destroy the object
    void Update()
    {
        if (timer <= 0)
        {
            // time's up

            string messageToSend = "Attack Totem Expired.";
            DisplayMessage.MessageToQueue(messageToSend);

            Destroy(gameObject.transform.parent.gameObject, 2.0f);
            Destroy(gameObject);
        }
        else
        {
            timer -= Time.deltaTime;
        }

        if (AOETimer <= 0)
        {
            Fire();
            AOETimer = timeBetweenAOE;
        }
        else
        {
            AOETimer -= Time.deltaTime;
        }
    }

    // instantiate an explosion that will do damage, play the audio and destroy the explosion
    private void Fire()
    {
        GameObject explode = Instantiate(AOE, firingPoint.position, Quaternion.identity);
        audioSource.volume = audioSource.volume * GameController.sfxVolume;
        audioSource.Play();
        Destroy(explode, 1.0f);
    }
}
