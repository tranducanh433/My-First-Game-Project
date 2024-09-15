using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolTest : MonoBehaviour
{
    public Rigidbody2D player;
    public Transform target;
    public float height = 5;

    void Start()
    {
        player.gravityScale = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }
    }
    void Launch()
    {
        player.gravityScale = 1;
        player.velocity = Direction();

    }
    Vector2 Direction()
    {
        float distenceY = target.position.y - player.position.y;
        float distenceX = target.position.x - player.position.x;

        float velocityY = Mathf.Sqrt(-2 * -9.81f * height);
        float velocityX = distenceX / (Mathf.Sqrt(-2 * height / -9.81f) + Mathf.Sqrt(2 * (distenceY - height) / -9.81f));

        return new Vector2(velocityX, velocityY);
    }
}
