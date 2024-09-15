using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMoveToPos : MonoBehaviour
{
    Vector2 m_target;
    float m_speed;
    System.Action OnGoal;

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_target, m_speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, m_target) <= 0.001f)
        {
            OnGoal?.Invoke();

            Destroy(this);
        }
    }

    public void SetData(Vector2 target, float speed, System.Action ongoal)
    {
        m_target = target;
        m_speed = speed;
        OnGoal = ongoal;
    }
}
