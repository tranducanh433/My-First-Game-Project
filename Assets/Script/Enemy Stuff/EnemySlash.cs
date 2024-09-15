using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlash : MonoBehaviour
{
    public Transform hitPoint;
    public Vector2 size;
    int damage;

    public void SetData(int damage)
    {
        this.damage = damage;
    }

    public void Deal_Damage()
    {
        Vector2 size = this.size;
        float angle = transform.rotation.z * Mathf.Rad2Deg;
        Collider2D[] hits = Physics2D.OverlapBoxAll(hitPoint.position, size, angle);

        foreach (Collider2D m_object in hits)
        {
            if (m_object.CompareTag("Player"))
            {
                m_object.GetComponent<Player>().TakeDamage(damage);
            }
        }
    }
}
