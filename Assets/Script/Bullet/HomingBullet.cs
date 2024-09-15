using GreenSlime.Calculator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    Transform m_target;
    float m_speed;
    float m_rotateSpeed;
    float targetAngle;
    float m_offset;
    HomingType m_homingType;

    bool startHoming;
    Rigidbody2D rb;

    public enum HomingType { Continuous, RotateToTarget}

    public void SetData(Transform target, float speed, float rotateSpeed, float offset, HomingType homingType)
    {
        m_target = target;
        m_speed = speed;
        m_rotateSpeed = rotateSpeed;
        m_offset = offset;
        m_homingType = homingType;

        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(StartHomingCo());
    }

    void Update()
    {
        if (startHoming)
        {
            targetAngle = FloatValue.AngleRotate2Target(transform.position, m_target.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), m_rotateSpeed * Time.deltaTime);

            rb.velocity = transform.right * m_speed;

            if (m_homingType == HomingType.RotateToTarget)
            {
                if (Mathf.DeltaAngle(transform.localEulerAngles.z, targetAngle) <= m_offset)
                {
                    Destroy(this);
                }
            }
        }

    }

    IEnumerator StartHomingCo()
    {
        yield return new WaitForSeconds(0.15f);
        startHoming = true;
    }
}
