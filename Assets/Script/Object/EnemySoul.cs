using System.Collections;
using System.Collections.Generic;
using GreenSlime.Calculator;
using UnityEngine;

public class EnemySoul : MonoBehaviour
{
    ParticleSystem enemySoulEffect;
    public float moveSpeed = 20;
    bool startChase;
    Rigidbody2D rb;
    Transform player;
    float angle;
    bool end;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemySoulEffect = GetComponent<ParticleSystem>();
        StartCoroutine(ChasePlayer());
    }

    private void Update()
    {
        if (startChase && end == false)
        {
            angle = FloatValue.AngleRotate2Target(transform.position, player.position);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), 40 * Time.deltaTime);

            rb.velocity = transform.right * moveSpeed * 1.5f;

            if (Vector2.Distance(transform.position, player.position) <= 0.5f)
            {
                GameManager.instance.GainEnemySoul();
                enemySoulEffect.Stop();
                Destroy(gameObject, 1);
                end = true;
            }
        }
        else
        {
            rb.velocity = transform.right * moveSpeed;
        }
    }

    IEnumerator ChasePlayer()
    {
        yield return new WaitForSeconds(0.15f);
        player = GameObject.Find("Player").transform;
        startChase = true;
    }
}
