using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GreenSlime.Editor;

[CustomEditor(typeof(MyTilesRule))]
public class Editor_MyTilesRule : Editor
{
    public override void OnInspectorGUI()
    {
        MyTilesRule mtr = target as MyTilesRule;

        MyEditor.VerticalInBox(TileSetting);
        EditorUtility.SetDirty(target);

        void TileSetting()
        {
            MyEditor.Label("Tiles");
            MyEditor.Horizontal(TileLine1);
            MyEditor.Horizontal(TileLine2);
            MyEditor.Horizontal(TileLine3);
            MyEditor.Space();
            MyEditor.Horizontal(TileLine4);
            MyEditor.Horizontal(TileLine5);
        }

        void TileLine1()
        {
            mtr.upLeft = MyEditor.TILE(mtr.upLeft);
            mtr.up = MyEditor.TILE(mtr.up);
            mtr.upRight = MyEditor.TILE(mtr.upRight);
        }
        void TileLine2()
        {
            mtr.left = MyEditor.TILE(mtr.left);
            mtr.defaultTile = MyEditor.TILE(mtr.defaultTile);
            mtr.right = MyEditor.TILE(mtr.right);
        }
        void TileLine3()
        {
            mtr.downLeft = MyEditor.TILE(mtr.downLeft);
            mtr.down = MyEditor.TILE(mtr.down);
            mtr.downRight = MyEditor.TILE(mtr.downRight);
        }
        void TileLine4()
        {
            mtr.upLeftEdge = MyEditor.TILE(mtr.upLeftEdge);
            mtr.upRightEdge = MyEditor.TILE(mtr.upRightEdge);
        }
        void TileLine5()
        {
            mtr.downLeftEdge = MyEditor.TILE(mtr.downLeftEdge);
            mtr.downRightEdge = MyEditor.TILE(mtr.downRightEdge);
        }
    }
}
