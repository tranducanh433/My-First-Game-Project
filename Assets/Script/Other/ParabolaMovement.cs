using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreenSlime.Calculator;

public class ParabolaMovement : MonoBehaviour
{
    public Transform shadow;

    Vector2 m_startPoint;
    Vector2 m_endPoint;
    float m_height;
    float moveSpeed;
    Action OnGoal;

    private List<Vector2> jumpingPoints = new List<Vector2>();
    private int currentPoint;

    public void SetParabolaData(Vector2 startPoint, Vector2 endPoint, float height, float speed)
    {
        m_startPoint = startPoint;
        m_endPoint = endPoint;
        m_height = height;
        moveSpeed = speed;
    }

    private void Start()
    {
        SetJumpPoint();
    }

    void SetJumpPoint()
    {
        //7 is number up point
        float xValue = 2f / 7f;

        for (int i = 0; i < 7; i++)
        {
            //Get point from a line (from start point to fall point)
            float num = ((float)i + 1f) / 7f;
            Vector2 downPoint = VectorValue.GetPointFrom2Vector(m_startPoint, m_endPoint, num);

            //Add Y from parabol
            float addY = FloatValue.GetValueInParabol(m_height, xValue);
            downPoint.y += addY;

            //Add to list
            jumpingPoints.Add(downPoint);

            xValue += 2f / 7f;
        }
    }

    void Update()
    {
        //Parabol Moving
        if (currentPoint == 3 || currentPoint == 4) //Near the ground so move faster
            transform.position = Vector2.MoveTowards(transform.position, jumpingPoints[currentPoint], moveSpeed * .65f * Time.deltaTime);
        else
            transform.position = Vector2.MoveTowards(transform.position, jumpingPoints[currentPoint], moveSpeed * Time.deltaTime);


        //Control the position of shadow
        float getYShadowPos;
        if (m_startPoint != m_endPoint)
            getYShadowPos = FloatValue.GetYFrom2Vector(m_startPoint, m_endPoint, transform.position.x);
        else
            getYShadowPos = m_endPoint.y;

        shadow.position = new Vector2(transform.position.x, getYShadowPos - 0.1f);

        //Change the point when near it
        if ((Vector2)transform.position == jumpingPoints[currentPoint] && currentPoint < jumpingPoints.Count)
        {
            currentPoint++;
        }
        //Is on the ground
        if (currentPoint == jumpingPoints.Count)
        {
            if (OnGoal != null)
                OnGoal();
            else
                Destroy(gameObject);
        }
    }
}
