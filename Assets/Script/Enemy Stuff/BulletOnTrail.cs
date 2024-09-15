using GreenSlime.Calculator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOnTrail : MonoBehaviour
{
    Element m_element;

    BulletColor m_bulletColor;
    GameObject m_bullet;
    GameObject bulletAppearEffect;
    int m_damage;
    float m_summonRadius;
    float m_summonTime;
    float m_survivalTime;
    Vector2 m_direction;
    float m_speed;

    float cd;
    List<Vector2> posList = new List<Vector2>();

    public void SetData(GameObject bullet, float summonRadius, float summonTime, float survivalTime, int damage, BulletColor bulletColor, Element element, Vector2 dir = new Vector2(), float speed = 0)
    {
        bulletAppearEffect = Resources.Load("Prefab/Effect/Bullet Appear") as GameObject;
        m_bullet = bullet;
        m_summonRadius = summonRadius;
        m_summonTime = summonTime;
        cd = summonTime;
        m_survivalTime = survivalTime;
        m_damage = damage;

        m_bulletColor = bulletColor;
        m_element = element;

        m_direction = dir;
        m_speed = speed;
    }

    private void Update()
    {
        if(cd <= 0)
        {
            SetPositionOfBullet();
            cd = m_summonTime;
        }
        else
        {
            cd -= Time.deltaTime;
        }
    }

    [System.Obsolete]
    void SetPositionOfBullet()
    {
        Vector2 minPos = new Vector2(transform.position.x - m_summonRadius, transform.position.y - m_summonRadius);
        Vector2 maxPos = new Vector2(transform.position.x + m_summonRadius, transform.position.y + m_summonRadius);
        Vector2 pos = VectorValue.RandomVector(minPos, maxPos);
        //posList.Add(pos);

        GameObject bullet = Instantiate(m_bullet, pos, Quaternion.identity);
        bullet.GetComponent<EnemyBullet>().BulletData(m_direction, m_speed, m_damage, m_bulletColor, m_element);
        Destroy(bullet, m_survivalTime);

        //GameObject effect = Instantiate(SummonBullet, pos, Quaternion.identity);
        //effect.GetComponent<BulletCallBack>().SetCallBack(SummonBullet);
    }

    void SummonBullet()
    {
        if (posList == null) return;

        GameObject bullet = Instantiate(m_bullet, posList[0], Quaternion.identity);
        bullet.GetComponent<EnemyBullet>().BulletData(m_direction, m_speed, m_damage, m_bulletColor, m_element);
        Destroy(bullet, m_survivalTime);
        posList.RemoveAt(0);
    }

    private void Reset()
    {
        bulletAppearEffect = Resources.Load("Prefab/Effect/Bullet Appear") as GameObject;   
    }
}
