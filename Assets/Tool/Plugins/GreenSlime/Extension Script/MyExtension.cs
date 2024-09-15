using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MyExtension
{
    public static class MyExtension
    {
        public static Color ShowColor(this Color color)
        {
            return new Color(color.r, color.g, color.b, 1);
        }
        public static Color HideColor(this Color color)
        {
            return new Color(color.r, color.g, color.b, 0);
        }

        public static int ToSiblingIndex(this Transform transform)
        {
            Transform parent = transform.parent;

            if (parent == null)
            {
                Debug.LogError("This Object Don't Have Any Parent");
            }

            for (int i = 0; i < parent.childCount; i++)
            {
                if(parent.GetChild(i) == transform)
                {
                    return i;
                }
            }

            return -1;
        }

        public static Vector2 ToDirection(this float angle)
        {
            float _angle = angle * Mathf.Deg2Rad;
            Vector2 vectorValue;
            vectorValue.x = Mathf.Cos(_angle);
            vectorValue.y = Mathf.Sin(_angle);

            return vectorValue;
        }
        public static Vector2 Add(this Vector2 vectorToAdd, float x, float y)
        {
            return new Vector2(vectorToAdd.x + x, vectorToAdd.y + y);
        }
        public static Vector2 Add(this Vector3 vectorToAdd, float x, float y)
        {
            return new Vector2(vectorToAdd.x + x, vectorToAdd.y + y);
        }
        public static float ToAngle(this Vector2 vector)
        {
            float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

            return angle;
        }
        public static float ToAngle(this Vector3 vector)
        {
            float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

            return angle;
        }

        public static float Limited(this float value, float min, float max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            else
                return value;
        }
        public static void SwitchActive(this GameObject gameObject)
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        }

        public static Vector2Int ToVector2Int(this Vector2 vector)
        {
            return new Vector2Int((int)vector.x, (int)vector.y);
        }
        public static Vector2 ToVector2(this Vector2Int vector)
        {
            return new Vector2(vector.x, vector.y);
        }
        public static void Add_X(this ref Vector2 vector, float addValue)
        {
            vector = new Vector2(vector.x + addValue, vector.y);
        }
        public static void Add_Y(this ref Vector2 vector, float addValue)
        {
            vector = new Vector2(vector.x, vector.y + addValue);
        }

        public static bool HaveValue<T>(this T[] array)
        {
            return array != null && array.Length > 0;
        }
        public static bool HaveValue<T>(this List<T> array)
        {
            return array != null && array.Count > 0;
        }

        public static List<T> ToList<T>(this T[] array)
        {
            List<T> ts = new List<T>();
            for (int i = 0; i < array.Length; i++)
            {
                ts.Add(array[i]);
            }

            return ts;
        }
    }
}