using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip[] backgroundMusic;
    AudioSource backgroundMusicSource;
    float musicVolume = 1f;

    private void Start()
    {
        backgroundMusicSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
     	
     	// get a random background music track and play it   
        backgroundMusicSource.clip = backgroundMusic[Random.Range(0, backgroundMusic.Length)];
        backgroundMusicSource.loop = true;
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            backgroundMusicSource.volume = 1.0f;
            PlayerPrefs.SetFloat("musicVolume", 1.0f);
        }
        else
        {
            musicVolume = PlayerPrefs.GetFloat("musicVolume");
            backgroundMusicSource.volume = musicVolume;
        }
        backgroundMusicSource.Play();
    }

    private void Update() 
    {
        backgroundMusicSource.volume = musicVolume;
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }
}