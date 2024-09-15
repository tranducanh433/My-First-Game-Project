using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreenSlime.SoundUtilities
{
    public static class Sound
    {
        public static void PlaySound(AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}