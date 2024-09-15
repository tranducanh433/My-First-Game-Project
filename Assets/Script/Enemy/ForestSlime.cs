using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreenSlime.Calculator;
using MyExtension;

public class ForestSlime : SlimeBase
{
    public GameObject bulletPrefab;

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
            AttackPlayer();
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
    #region Attack System
    void AttackPlayer()
    {
        if (currentState == EnemyState.attack)
        {
            //CD to attack
            if (atkCD <= 0)
            {
                Attack();
            }
            else
            {
                //Return to CHASE when player go out of attack radius
                if (!isInAttackRadius && isJumping == false)
                {
                    currentState = EnemyState.chase;
                }
            }
        }

        atkCD -= Time.deltaTime;
    }
    void Attack()
    {
        //Get Jump and Fall Point
        if (isJumping == false)
        {
            SetJumpAndFallPoint(jumpHeight, player);
        }
    }
    #endregion

    protected override void OnDeath()
    {
        float angle = 0;
        float angleStep = 360 / 6;

        for (int i = 0; i < 6; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<EnemyBullet>().BulletData(angle.ToDirection(), 10f, damage, BulletColor.Green, Element.Plant);

            angle += angleStep;
        }
    }
}
