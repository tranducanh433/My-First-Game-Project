using GreenSlime.Calculator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyExtension;
using System;

public class HealingItem : MonoBehaviour
{
    public float moveSpeed = 10;
    bool startChase;
    Rigidbody2D rb;
    Transform player;
    float angle;
    Vector3 targetRotate;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(ChasePlayer());
    }

    private void Update()
    {
        if (startChase)
        {
            angle = FloatValue.AngleRotate2Target(transform.position, player.position);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), 10 * Time.deltaTime);

            rb.velocity = transform.right * moveSpeed;

            if (Vector2.Distance(transform.position, player.position) <= 0.5f)
            {
                player.GetComponent<Player>().GainHP(1);
                Destroy(gameObject);
            }
        }
    }

    IEnumerator ChasePlayer()
    {
        yield return new WaitForSeconds(0.25f);
        rb.velocity = Vector2.zero;
        player = GameObject.Find("Player").transform;
        startChase = true;
    }
}
