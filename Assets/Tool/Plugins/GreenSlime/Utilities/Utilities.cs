using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreenSlime.Calculator
{
    public static class BoolValue
    {
        public static bool SwitchTrueFalse(bool currentValue)
        {
            if (currentValue)
                return false;
            else
                return true;
        }

        public static bool Random()
        {
            int r = UnityEngine.Random.Range(0, 2);

            if (r == 0)
                return false;
            else
                return true;
        }
        public static bool IsInRadius(Vector2 current, Vector2 target, float radius)
        {
            if (Vector2.Distance(current, target) <= radius)
                return true;
            else
                return false;
        }
    }
    public static class FloatValue
    {
        public static float AngleRotate2Target(Vector2 current, Vector2 target, float addAngle = 0)
        {
            Vector2 direction = target - current;
            direction.Normalize();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + addAngle;

            return angle;
        }
        public static float GetValueInParabol(float maxY, float x)
        {
            float a = maxY;
            float b = maxY * 2;

            float y =  - a * Mathf.Pow(x, 2) + b * x;
            return y;
        }
        public static float GetYFrom2Vector(Vector2 point1, Vector2 point2, float xValue)
        {
            //To Find m = (y2 -y1) / (x2 - x1)
            float y = point2.y - point1.y;
            float x = point2.x - point1.x;

            float m = y / x;

            //Get x and y from point1
            //b = y -m * x
            float b = point1.y - m * point1.x;

            //y = mx + b
            float yValue = m * xValue + b;
            return yValue;
        }
    }
    public static class VectorValue
    {
        public static Vector2 RandomVector(Vector2 minPos, Vector2 maxPos)
        {
            Vector2 newPos = new Vector2(Random.Range(minPos.x, maxPos.x),
                                        Random.Range(minPos.y, maxPos.y));

            return newPos;
        }
        public static Vector2Int RandomVector(Vector2Int minPos, Vector2Int maxPos)
        {
            Vector2Int newPos = new Vector2Int(Random.Range(minPos.x, maxPos.x),
                                                Random.Range(minPos.y, maxPos.y));

            return newPos;
        }
        public static Vector3Int RandomVector(Vector3Int minPos, Vector3Int maxPos)
        {
            Vector3Int newPos = new Vector3Int(Random.Range(minPos.x, maxPos.x),
                                                Random.Range(minPos.y, maxPos.y),
                                                  Random.Range(minPos.z, maxPos.z));

            return newPos;
        }
        public static Vector2 GetCenterPoint(Vector2 point1, Vector2 point2)
        {
            Vector2 centerPoint = new Vector2((point1.x + point2.x) / 2, 
                                                (point1.y + point2.y) / 2);
            return centerPoint;
        }
        public static Vector2 GetPointFrom2Vector(Vector2 point1, Vector2 point2, float numberToDivide)
        {
            Vector2 line = point2 - point1;
            Vector2 part = line * numberToDivide;
            Vector2 result = point1 + part;

            return result;
        }
        public static Vector2 GetDirectionFromTarget(Vector2 current, Vector2 target,float scale = 1, float addAngle = 0)
        {
            Vector2 direction = target - current;
            direction.Normalize();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + addAngle;

            return GetDirectionFromAngle(angle, scale);
        }
        public static Vector2 GetDirectionFromAngle(float angle, float distance = 1)
        {
            float _angle = angle * Mathf.Deg2Rad;
            Vector2 vectorValue;
            vectorValue.x = Mathf.Cos(_angle);
            vectorValue.y = Mathf.Sin(_angle);

            return vectorValue * distance;
        }
        public static Vector2 GetDirectionByParabola(Vector2 current, Vector2 target, float height, float gravityScale = 1)
        {
            float gravity = -Physics2D.gravity.magnitude * gravityScale;

            float distenceY = target.y - current.y;
            float distenceX = target.x - current.x;


            float velocityY = Mathf.Sqrt(-2 * gravity * height);
            float velocityX = distenceX / (Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (distenceY - height) / gravity));

            return new Vector2(velocityX, velocityY);
        }

        public static Vector2Int ToInt2(Vector2 v)
        {
            return new Vector2Int((int)v.x, (int)v.y);
        }
    }

    public class MyTransform
    {
        public static Vector2 LimitedDistance(Vector2 start, Vector2 current, float distance)
        {
            if(Vector2.Distance(start, current) <= distance)
            {
                return current;
            }
            else
            {
                return start + (current - start).normalized * distance;
            }
        } 
    }
}
