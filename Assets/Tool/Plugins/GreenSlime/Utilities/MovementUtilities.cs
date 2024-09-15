using MyExtension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyMovement
{
    public static class Movement2D
    {
        public static Vector2 MoveAround(Vector2 current, Vector2 viot, float radius, float speed)
        {
            float angle = (current - viot).ToAngle();
            angle += speed;
            return viot + angle.ToDirection() * radius;
        }
    }
}
