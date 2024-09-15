using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : MonoBehaviour
{
    public LayerMask objectCanHit;
    public Transform hitPoint;
    public GameObject hitEffect;
    public bool activateAtStart;
    int damage;
    Element m_element;
    Player player;

    private void Start()
    {
        if (activateAtStart)
            Deal_Damage();
    }

    public void SetData(int damage, Element element, Player m_player = null)
    {
        this.damage = damage;
        m_element = element;

        player = m_player;
    }

    public void Deal_Damage()
    {
        Vector2 transformSize = transform.localScale;
        Vector2 size = new Vector2(4 * transformSize.x, 3 * transformSize.y);
        float angle = transform.rotation.z * Mathf.Rad2Deg;
        Collider2D[] hits = Physics2D.OverlapBoxAll(hitPoint.position, size, angle, objectCanHit);

        foreach(Collider2D m_object in hits)
        {
            m_object.GetComponent<Creature>().TakeDamage(damage, m_element);

            Instantiate(hitEffect, m_object.transform.position, Quaternion.identity);

            if (player != null)
                player.GainMP(2);
        }
    }
}
