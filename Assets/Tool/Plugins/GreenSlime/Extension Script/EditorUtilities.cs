using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

namespace GreenSlime.Editor
{


#if UNITY_EDITOR
    public static class MyEditor
    {
        //Design
        public static void Label(string label, bool bold = true)
        {
            if(bold)
                EditorGUILayout.LabelField(new GUIContent(label), EditorStyles.boldLabel);
            else
                EditorGUILayout.LabelField(new GUIContent(label));
        }
        public static void Space(int amount = 5)
        {
            EditorGUILayout.Space(amount);
        }
        public static bool Foldout(bool value, string label)
        {
            return EditorGUILayout.Foldout(value, new GUIContent(label));
        }
        public static Vector2 ScollDown(Vector2 scrollPos)
        {
            return EditorGUILayout.BeginScrollView(scrollPos);
        }
        public static void Button(Action _func, string content)
        {
            if (GUILayout.Button(content))
                _func();
        }
        public static void TickBox(this ref bool boolVar, string label)
        {
            boolVar = GUILayout.Toggle(boolVar, label);
        }
        //Display Design
        public static void Horizontal(Action _func)
        {
            GUILayout.BeginHorizontal();
            _func();
            GUILayout.EndHorizontal();
        }
        public static void Vertical(Action _func)
        {
            GUILayout.BeginVertical();
            _func();
            GUILayout.EndVertical();
        }
        public static void VerticalInBox(Action _func)
        {
            GUILayout.BeginVertical("box");
            _func();
            GUILayout.EndVertical();
        }
        public static void Margin(Action _func, int space = 5)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(space);
            GUILayout.BeginVertical();
            _func();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        //Varaible
        public static int INT(int value, string label)
        {
            return EditorGUILayout.IntField(new GUIContent(label), value);
        }
        public static int INT(int defaultValue, int minValue, int maxValue, string label)
        {
            return EditorGUILayout.IntSlider(new GUIContent(label), defaultValue, minValue, maxValue);
        }
        public static bool BOOL(bool value, string label)
        {
            return EditorGUILayout.Toggle(new GUIContent(label), value);
        }
        public static string TextField(string value, string label)
        {
            return EditorGUILayout.TextField(new GUIContent(label), value);
        }
        public static GameObject GAMEOBJECT(GameObject _object, string label)
        {
            return (GameObject)EditorGUILayout.ObjectField(new GUIContent(label), _object, typeof(GameObject), true);
        }

        public static Tilemap TILEMAP(Tilemap tilemap, string label)
        {
            return (Tilemap)EditorGUILayout.ObjectField(new GUIContent(label), tilemap, typeof(Tilemap), true);
        }
        public static Tile TILE(Tile tile, string label)
        {
            return (Tile)EditorGUILayout.ObjectField(new GUIContent(label), tile, typeof(Tile), true);
        }
        public static Tile TILE(Tile tile)
        {
            return (Tile)EditorGUILayout.ObjectField(tile, typeof(Tile), false);
        }

    }
#endif
}