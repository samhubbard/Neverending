using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1.0f);
            GetComponent<Slider>().value = 1.0f;
        }
        else
        {
            GetComponent<Slider>().value = PlayerPrefs.GetFloat("musicVolume");
        }
    }
}
