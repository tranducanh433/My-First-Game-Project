using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreenSlime.Calculator;

public class EXP : MonoBehaviour
{
    public ParticleSystem expEffect;
    public float moveSpeed = 20;
    public int EXPAmount;
    bool startChase;
    Rigidbody2D rb;
    Transform player;
    float angle;
    bool end;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
                GameManager.instance.GainEXP(EXPAmount);
                expEffect.Stop();
                GetComponent<SpriteRenderer>().sprite = null;
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
