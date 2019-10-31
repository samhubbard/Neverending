using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightProjectileImpactSound : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
    	// the sound that plays when a projectile hits the enemy
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.4f;

        audioSource.Play();

        Destroy(gameObject, 2.0f);
    }
}
