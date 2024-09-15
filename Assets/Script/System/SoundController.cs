using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public void PlayTheSound(AudioClip soundToPlay)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = soundToPlay;
        audio.Play();
    }
}
