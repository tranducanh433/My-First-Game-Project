using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreenSlime.Calculator;

public class GaintRedSlime : SlimeBase
{
    [Header("Gaint Red Slime Setting")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    public Transform shootPoint;

    void Start()
    {
        BasicStart();
        EnablePathFinding();
    }

    void Update()
    {
        CheckPlayer();

        if (currentState != EnemyState.death)
        {
            //Flip Image
            FlipImage(transform, player);
            StateMachineController();
            JumpMovement(moveSpeed);
            ChasingPlayer();
        }
    }

    #region State Machine Controller
    void StateMachineController()
    {
        if (isInChaseRadius && !isInAttackRadius && currentState != EnemyState.attack)
        {
            currentState = EnemyState.chase;
        }
        if (isInAttackRadius && isJumping == false)
        {
            currentState = EnemyState.attack;
        }
    }
    #endregion

    #region Chasing System
    void ChasingPlayer()
    {
        if (currentState == EnemyState.chase)
        {
            if (atkCD <= 0 && isJumping == false)
            {
                //Get Jump and Fall Point
                SetJumpAndFallPoint(jumpHeight);
            }
            else
            {
                //Jump CD
                atkCD -= Time.deltaTime;
            }
        }
    }
    #endregion

    #region Deal Damage
    protected override void DealDamage()  //Deal damage to player
    {
        if (Vector2.Distance(transform.position, player.position) <= 1.5f)
        {
            player.GetComponent<Player>().TakeDamage(damage);
        }
        ShootBullet();
    }

    void ShootBullet()
    {
        float startAngle = FloatValue.AngleRotate2Target(transform.position, player.position) - 20f;
        shootPoint.rotation = Quaternion.Euler(0, 0, startAngle);

        for (int i = 0; i < 5; i++)
        {
            GameObject _bullet = Instantiate(bulletPrefab, transform.position, shootPoint.rotation);
            _bullet.GetComponent<EnemyBullet>().BulletData(shootPoint.right, bulletSpeed, damage, BulletColor.Red, Element.None);

            startAngle += 40f / 5f;
            shootPoint.rotation = Quaternion.Euler(0, 0, startAngle);
        }
    }
    #endregion
}
