using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageProjectileImpactSound : MonoBehaviour
{
    AudioSource audioSource;

    // Plays the sound effect
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = 0.4f * GameController.sfxVolume;
        audioSource.Play();
    }
}
