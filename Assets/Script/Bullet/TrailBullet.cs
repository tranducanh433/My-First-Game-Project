using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailBullet : MonoBehaviour
{
    GameObject trailObject;
    int damage;
    float trailPerSec = 0.25f;
    Element m_element;

    float cd;

    public void SetData(GameObject trailObject, int damage, Element element, float trailPerSec = 0.15f)
    {
        this.trailObject = trailObject;
        this.damage = damage;
        this.trailPerSec = trailPerSec;
        m_element = element;
    }

    void Update()
    {
        if(cd <= 0 && trailObject != null)
        {
            GameObject obj = Instantiate(trailObject, transform.position, Quaternion.identity);
            obj.GetComponent<SlashAttack>().SetData(damage, m_element);
            Destroy(obj, 1);

            cd = trailPerSec;
        }
        else
        {
            cd -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Cant Break"))
        {
            Destroy(gameObject);
        }
    }
}
