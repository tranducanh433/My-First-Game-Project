using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreenSlime.SoundUtilities;

public class ShootingLog : Enemy
{
    public GameObject brownBullet;
    public float bulletSpeed;

    public AudioClip shootSound;

    void Start()
    {
        BasicStart();
    }


    void Update()
    {
        CheckPlayer();

        if (currentState != EnemyState.death)
        {

            if (canSeePlayer)
            {
                StateMachineController();
                FlipImage(transform, player);
                AttackSystem();
            }
            else
            {
                currentState = EnemyState.idle;
            }
            
        }
    }
    void StateMachineController()
    {
        if (isInAttackRadius)
        {
            currentState = EnemyState.attack;
        }
    }

    void AttackSystem()
    {
        if(currentState == EnemyState.attack)
        {
            AttackCountdown(Attack);
        }
    }

    void Attack()
    {
        anim.SetTrigger("attack");
    }

    public void ShootProjectile()
    {
        GameObject bullet = Instantiate(brownBullet, transform.position, Quaternion.identity);
        Rigidbody2D rbBulet = bullet.GetComponent<Rigidbody2D>();


        Vector2 direction = player.position - transform.position;
        direction.Normalize();
        bullet.GetComponent<EnemyBullet>().BulletData(direction, bulletSpeed, damage, BulletColor.Brown, Element.Earth);

        Sound.PlaySound(audioSource, shootSound);
    }
}
