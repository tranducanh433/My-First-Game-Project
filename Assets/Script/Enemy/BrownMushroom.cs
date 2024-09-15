using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreenSlime.Calculator;

public class BrownMushroom : Enemy
{
    public enum AttackState { none, attack, stand}
    public Transform weaponViot;
    public Animator weaponAnim;
    public GameObject enemySlashEffect;

    public Transform hitPoint;

    private AttackState attackState = AttackState.none;
    private float angle;

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
            StateMachineController();
            ChasePlayer();
            AttackSystem();
            SwordAiming();

            if(! isAttacking)
                FlipImage(transform, player);
        }
    }

    void StateMachineController()
    {
        if (isInChaseRadius && !isInAttackRadius && attackState == AttackState.none)
        {
            currentState = EnemyState.chase;
            anim.SetBool("move", true);
        }
        if (isInAttackRadius && attackState == AttackState.none)
        {
            currentState = EnemyState.attack;
            anim.SetBool("move", false);
        }
        if(currentState == EnemyState.idle)
        {
            anim.SetBool("move", false);
        }
    }

    void SwordAiming()
    {
        if (!isAttacking)
        {
            angle = FloatValue.AngleRotate2Target(weaponViot.position, player.position);
            weaponViot.rotation = Quaternion.Euler(0, 0, angle);

            if(transform.position.x > player.position.x)
            {
                weaponAnim.SetBool("isLeft", true);
            }
            else
            {
                weaponAnim.SetBool("isLeft", false);
            }
        }
    }

    void ChasePlayer()
    {
        if(currentState == EnemyState.chase)
        {
            if (canSeePlayer)
            {
                MoveToTarget(transform, player.position, moveSpeed);
                ClearPath();
            }
            else
            {
                if (paths == null )
                {
                    UpdatePath();
                }
                
                if (PathHaveValue() == false) 
                    return;


                MoveToTarget(transform, paths[currentPath], moveSpeed);

                if(Vector2.Distance(paths[currentPath], transform.position) <= 0.25f && currentPath < paths.Length - 1)
                {
                    currentPath++;
                }
            }
        }
    }

    void AttackSystem()
    {
        if(currentState == EnemyState.attack)
        {
            AttackCountdown(Attack);

            if(!isInAttackRadius && attackState == AttackState.none)
            {
                currentState = EnemyState.chase;
            }
        }
    }

    void Attack()
    {
        isAttacking = true;
        StartCoroutine(AttackCO());
    }

    private IEnumerator AttackCO()
    {
        attackState = AttackState.stand;

        yield return new WaitForSeconds(0.5f);
        attackState = AttackState.attack;
        weaponAnim.SetBool("attack", true);
        float _angle = FloatValue.AngleRotate2Target(weaponViot.position, player.position);
        GameObject slash = Instantiate(enemySlashEffect, hitPoint.position, Quaternion.Euler(0, 0, _angle), transform);
        slash.GetComponent<EnemySlash>().SetData(damage);

        Vector2 direction = player.position - transform.position;
        direction.Normalize();
        rb.velocity = direction * moveSpeed * 3f;

        yield return new WaitForSeconds(0.25f);
        rb.velocity = Vector2.zero;
        isAttacking = false;
        weaponAnim.SetBool("attack", false);
        attackState = AttackState.none;
    }

    private void OnDrawGizmosSelected()
    {
        if (hitPoint == null)
            return;

        Gizmos.DrawWireCube(weaponViot.position, Vector3.one);
    }
}
