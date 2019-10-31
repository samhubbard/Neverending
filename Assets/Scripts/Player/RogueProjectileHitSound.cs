using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueProjectileHitSound : MonoBehaviour
{
    AudioSource audioSource;

    // Get the reference and play the sound for the DOT effect
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = 0.4f * GameController.sfxVolume;
        audioSource.Play();

        Destroy(gameObject, 2.0f);
    }
}
