using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileHitSound : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
    	// play the projectile hit sound
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.3f * GameController.sfxVolume;
        audioSource.Play();

        Destroy(gameObject, 2.0f);
    }
}
