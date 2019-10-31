using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameKnightGeneralSound : MonoBehaviour
{
    public void StartPlayingSound()
    {
    	// setting it to loop and play
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().volume = GetComponent<AudioSource>().volume * GameController.sfxVolume;
        GetComponent<AudioSource>().Play();
    }

    public void StopPlayingSound()
    {
    	// STOP PLAYING THAT SOUND!
        GetComponent<AudioSource>().Stop();
    }
}
