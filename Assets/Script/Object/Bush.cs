using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : TerrainSources
{
    public ParticleSystem leafFalling;
    public Animator anim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            leafFalling.Play();
            anim.SetTrigger("Interact");
        }
    }

    protected override void TakeDamageEffect()
    {
        leafFalling.Play();
        anim.SetTrigger("Interact");
    }
}
