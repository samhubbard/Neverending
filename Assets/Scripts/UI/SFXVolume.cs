using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXVolume : MonoBehaviour
{

    float sfxVolume = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("sfxVolume"))
        {
            PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
            GameController.sfxVolume = sfxVolume;
            GetComponent<Slider>().value = sfxVolume;
        }
        else
        {
            sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
            GameController.sfxVolume = sfxVolume;
            GetComponent<Slider>().value = sfxVolume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameController.sfxVolume = sfxVolume;
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }
}
