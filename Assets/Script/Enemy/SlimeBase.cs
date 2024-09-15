using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreenSlime.Calculator;

public class SlimeBase : Enemy
{
    [Header("Slime Base Setting")]
    public Transform shadow;
    public float jumpHeight = 1;
    public float shadowOffSet = 0.4f;

    protected bool isJumping;
    protected Vector2 startPoint;
    protected Vector2 fallPoint;
    protected int currentPoint;
    protected List<Vector2> jumpingPoints = new List<Vector2>();


    protected void JumpMovement(float speed)
    {
        if (isJumping)
        {
            //Parabol Moving
            if (currentPoint <= 4)
                MoveToTarget(transform, jumpingPoints[currentPoint], speed);
            else
                MoveToTarget(transform, jumpingPoints[currentPoint], speed * 3);

            SetShadowPosWentJump();

            //Change the point when near it
            if (Vector2.Distance(transform.position, jumpingPoints[currentPoint]) <= 0.01f && currentPoint < jumpingPoints.Count)
            {
                currentPoint++;
            }

            if (currentPoint == jumpingPoints.Count)    //Is on the ground
            {
                //Reset the status
                currentState = EnemyState.idle;
                isJumping = false;
                anim.SetBool("isJump", false);
                atkCD = attackSpeed;
                DealDamage();
                shadow.position = new Vector2(transform.position.x, transform.position.y + shadowOffSet);
            }
        }
    }
    void SetShadowPosWentJump()
    {
        //Control the position of shadow            
        float getYShadowPos;
        if (startPoint != fallPoint)
            getYShadowPos = FloatValue.GetYFrom2Vector(startPoint, fallPoint, transform.position.x);
        else
            getYShadowPos = fallPoint.y;

        shadow.position = new Vector2(transform.position.x, getYShadowPos + shadowOffSet);
    }



    #region Get Jump And Fall Point
    protected virtual void SetJumpAndFallPoint(float maxHeight, Transform target = null)
    {
        //Get the start point
        startPoint = transform.position;
        anim.SetBool("isJump", true);

        //Calcular Fall Point
        if (target == null)
        {
            if (canSeePlayer)
            {
                fallPoint = Vector2.MoveTowards(transform.position, player.position, 3);
                ClearPath();
            }
            else
            {
                if (paths == null)
                {
                    UpdatePath();
                }

                if (PathHaveValue() == false)
                    return;

                if (Vector2.Distance(paths[currentPath], transform.position) <= 0.25f && currentPath < paths.Length - 1)
                {
                    currentPath++;
                }


                fallPoint = Vector2.MoveTowards(transform.position, paths[currentPath], 3);
            }
        }
        else
        {
            fallPoint = target.position;
        }

        if (fallPoint.x == startPoint.x)
        {
            fallPoint = new Vector2(fallPoint.x + 0.001f, fallPoint.y);
        }

        //Set Value To Make Sure That This "If" Only Activate One Time
        currentPoint = 0;
        jumpingPoints.Clear();

        GetJumpingPoint(maxHeight);
    }
    protected virtual void SetJumpAndFallPoint(float maxHeight, Vector2 target)
    {

    }

    protected void GetJumpingPoint(float maxHeight)
    {
        float xValue = 2f / 7f;

        for (int i = 0; i < 7; i++)
        {
            //Get point from a line (from start point to fall point)
            float num = ((float)i + 1f) / 7f;
            Vector2 downPoint = VectorValue.GetPointFrom2Vector(transform.position, fallPoint, num);

            //Add Y from parabol
            float addY = FloatValue.GetValueInParabol(maxHeight, xValue);
            downPoint.y += addY;

            //Add to list
            jumpingPoints.Add(downPoint);

            xValue += 2f / 7f;
        }
    }
    #endregion

    #region Attack System


    protected virtual void DealDamage()  //Deal damage to player
    {
        if (Vector2.Distance(transform.position, player.position) <= 1)
        {
            player.GetComponent<Player>().TakeDamage(damage);
        }
    }
    #endregion

    #region Animation Function
    public void StartJump()
    {
        isJumping = true;
    }
    #endregion
}
