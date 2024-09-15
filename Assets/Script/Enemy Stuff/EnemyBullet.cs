using GreenSlime.Calculator;
using MyExtension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletColor
{
    Red,
    Brown,
    Green,
    Purple
}
public class EnemyBullet : MonoBehaviour
{
    public int damage;
    public Material red, brown, purple, green;
    public GameObject hitEffect;

    SpriteRenderer sr;
    Element m_element;
    bool both;
    bool destroyWhenHit = true;
    float m_speed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamage(damage);

            if(destroyWhenHit)
                Destroy();
        }
        if(collision.CompareTag("Enemy") && both)
        {
            collision.GetComponent<Enemy>().TakeDamage(damage, m_element);
            Destroy();
        }

        if (collision.CompareTag("Cant Break"))
        {
            Destroy();
        }
    }

    public void BulletData(Vector2 direction, float speed, int damage, BulletColor bulletColor, Element element, bool dealToEnemyToo = false, bool destroyWhenHit = true, float destroyTime = 99)
    {
        m_element = element;
        this.destroyWhenHit = destroyWhenHit;

        sr = GetComponent<SpriteRenderer>();
        sr.material = GetMaterial(bulletColor);

        both = dealToEnemyToo;

        this.damage = damage;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
        m_speed = speed;

        Destroy(gameObject, destroyTime);
    }

    Material GetMaterial(BulletColor bulletColor)
    {
        switch (bulletColor)
        {
            case BulletColor.Red:
                return red;
            case BulletColor.Brown:
                return brown;
            case BulletColor.Green:
                return green;
            case BulletColor.Purple:
                return purple;
            default:
                return null;
        }
    }

    public void OnTrailEvent(GameObject bullet, float summonRadius, float summonTime, float survivalTime, int damage, BulletColor bulletColor, Element element, Vector2 dir = new Vector2(), float speed = 0)
    {
        BulletOnTrail onTrail = gameObject.AddComponent<BulletOnTrail>();
        onTrail.SetData(bullet, summonRadius, summonTime, survivalTime, damage, bulletColor, element, dir, speed);
    }

    public void MoveToPosEvent(Vector2 target, float speed, System.Action ongoal)
    {
        BulletMoveToPos bulletMoveToPos = gameObject.AddComponent<BulletMoveToPos>();
        bulletMoveToPos.SetData(target, speed, ongoal);
    }
    public void Event_Homing(Transform target, float speed, float rotateSpeed, float offset, HomingBullet.HomingType homingType)
    {
        HomingBullet homingBullet = gameObject.AddComponent<HomingBullet>();
        homingBullet.SetData(target, speed, rotateSpeed, offset, homingType);
    }

    public void ChangeDirection(Vector2 dir, float waitTime)
    {
        StartCoroutine(ChangeDirectionCO(dir, waitTime));
    }
    public void ChangeDirection2Target(Transform target, float waitTime)
    {
        StartCoroutine(ChangeDirectionCO(target, waitTime));
    }
    IEnumerator ChangeDirectionCO(Vector2 dir, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = dir * m_speed;
    }
    IEnumerator ChangeDirectionCO(Transform target, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Vector2 dir = FloatValue.AngleRotate2Target(transform.position, target.position).ToDirection();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = dir * m_speed;
    }
    void Destroy()
    {
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void Reset()
    {
        red = Resources.Load("Material/Bullet Outline/Red Bullet") as Material;
        brown = Resources.Load("Material/Bullet Outline/Brown Bullet") as Material;
        purple = Resources.Load("Material/Bullet Outline/Purple Bullet") as Material;
        green = Resources.Load("Material/Bullet Outline/Green Bullet") as Material;

        hitEffect = Resources.Load("Prefab/Effect/Hit Effect") as GameObject;
    }
}
