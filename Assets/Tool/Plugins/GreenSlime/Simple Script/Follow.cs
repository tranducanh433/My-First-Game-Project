using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public float speed;
    Vector2 m_target;
    public void SetTarget(Transform target, float speed)
    {
        this.speed = speed;
        this.target = target;
    }
    public void SetTarget(Vector2 target, float speed)
    {
        this.speed = speed;
        this.m_target = target;
    }

    void Update()
    {
        if (target != null)
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        else
            transform.position = Vector2.MoveTowards(transform.position, m_target, speed * Time.deltaTime);
    }
}
