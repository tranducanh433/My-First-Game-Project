using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreenSlime.Calculator;

public class CameraPoint : MonoBehaviour
{
    public Transform player;

    Vector2 point;
    Vector2 mousePos;

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        point = (((Vector2)player.position + mousePos) / 2 + (Vector2)player.position) / 2;
        transform.position = MyTransform.LimitedDistance(player.position, point, 3);

    }
}
