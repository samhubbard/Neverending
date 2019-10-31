using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryActivationSound : MonoBehaviour
{
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = audioSource.volume * GameController.sfxVolume;
        audioSource.Play();

        Destroy(gameObject, 2.0f);
    }
}
