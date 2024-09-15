using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectManager : MonoBehaviour
{
    public ParticleSystem chargingEffect, dashEffect, activateSkillEffect;

    public void PlayChargingEffect()
    {
        chargingEffect.Play();
    }

    public void PlayDashEffect()
    {
        dashEffect.Play();
    }

    public void PlayActivateSkillEffect()
    {
        activateSkillEffect.Play();
    }

    public void StopAllEffect()
    {
        chargingEffect.Stop();
    }
}
