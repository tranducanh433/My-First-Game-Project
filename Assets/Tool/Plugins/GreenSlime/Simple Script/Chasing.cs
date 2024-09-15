using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasing : MonoBehaviour
{
    Transform current;
    Vector2 target;
    float moveSpeed;

    public void Move(Transform _transform, Vector2 _from, Vector2 _to, float speed, bool useDeltatime = true)
    {
        current = _transform;
        target = (_from - _to) + (Vector2)_transform.position;
        moveSpeed = speed;

        if (useDeltatime == false)
            moveSpeed /= Time.deltaTime;
    }

    void Update()
    {
        current.position = Vector2.MoveTowards(current.position, target, moveSpeed * Time.deltaTime);

        if ((Vector2)current.position == target)
            Destroy(this);
    }
}
