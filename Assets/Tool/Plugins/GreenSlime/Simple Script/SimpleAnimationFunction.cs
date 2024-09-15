using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimationFunction : MonoBehaviour
{
    public ParticleSystem particleSystemToPlay;
    public UIParticleSystem UIParticleSystemToPlay;

    public void SwitchThisGameObject2False()
    {
        gameObject.SetActive(false);
    }
    public void DestroyThisGameObject()
    {
        Destroy(gameObject);
    }
    public void PlayParticleSystem()
    {
        particleSystemToPlay.Play();
    }
    public void PlayUIParticleSystem()
    {
        UIParticleSystemToPlay.Play();
    }
}
