using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicBullet : MonoBehaviour
{
    public ParticleSystem particle;
    public GameObject hitEffect;
    int damage;
    public bool oneTime = true;
    public bool canDestroy = true;

    private Element m_element;
    private bool oneHitDone;
    Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && oneHitDone == false)
        {
            collision.GetComponent<Creature>().TakeDamage(damage, m_element);
            Instantiate(hitEffect, transform.position, Quaternion.identity);

            if (player != null)
                player.GainMP(2);

            if(canDestroy == true)
            {
                if(particle != null)
                    particle.Stop();

                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Destroy(gameObject, 1f);

                if(transform.childCount >= 1)
                    Destroy(transform.GetChild(0).gameObject);

                if (oneTime)
                {
                    oneHitDone = true;
                }
            }
        }
        if(collision.CompareTag("Breakable Object"))
        {
            collision.GetComponent<Creature>().TakeDamage(damage, m_element);
        }
        if(collision.CompareTag("Cant Break") && transform.childCount >= 1)
        {
            if (particle != null)
                particle.Stop();
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            if(transform.childCount > 0)
                Destroy(transform.GetChild(0).gameObject);
            Destroy(gameObject, 1f);
            oneHitDone = true;
        }
    }

    public void BulletData(Vector2 direction, float speed, int damage, Element element, Player m_player = null)
    {
        this.damage = damage;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
        m_element = element;

        player = m_player;
    }
    public void BulletData(float speed, int damage, Element element, Player m_player = null)
    {
        this.damage = damage;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        m_element = element;

        player = m_player;
    }
}
