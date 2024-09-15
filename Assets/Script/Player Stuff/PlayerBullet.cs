using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public enum KindOfHit { stuck, destroy, cantDestroy}
    public int damage = 1;
    public bool oneTime = true;
    public KindOfHit kindOfHit;
    public GameObject effectToDestroy;
    private Player player;

    private bool oneHitDone;
    private Element m_element;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !oneHitDone)
        {
            collision.GetComponent<Enemy>().TakeDamage(damage, m_element);

            if(kindOfHit == KindOfHit.stuck)
            {
                transform.parent = collision.gameObject.transform;

                Destroy(GetComponent<Rigidbody2D>());
                Destroy(this);
                Destroy(effectToDestroy);
                Destroy(GetComponent<DestroyWithTime>());

            }
            else if(kindOfHit == KindOfHit.destroy)
            {
                Destroy(gameObject);
            }

            if (oneTime)
            {
                oneHitDone = true;
            }
            if(player != null)
            {
                player.GainMP(2);
            }
        }

        if (collision.CompareTag("Cant Break"))
        {
            Destroy(gameObject);
        }
    }

    public void BulletData(Vector2 direction, float speed, int damage, Element element, Player player = null)
    {
        this.damage = damage;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
        this.player = player;
        m_element = element;
    }
    public void BulletData(float speed, int damage, Element element, Player player = null)
    {
        this.damage = damage;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        this.player = player;
        m_element = element;
    }
}
