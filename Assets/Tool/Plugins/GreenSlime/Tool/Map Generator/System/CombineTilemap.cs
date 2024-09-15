using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GreenSlime.Editor;
using GreenSlime.Calculator;
using UnityEngine.Tilemaps;



#if UNITY_EDITOR
public class CombineTilemap : EditorWindow
{
    List<Tilemap> mainTileMap = new List<Tilemap>(1);
    List<Tilemap> tilemapToCombine = new List<Tilemap>(1);

    Vector2Int offset;

    [MenuItem("Tilemap Editor/Combine Tilemap")]
    public static void OpenWindow()
    {
        GetWindow<CombineTilemap>("Combine Tilemap");
    }

    private void OnGUI()
    {
        MyEditor.Vertical(DisplayTilemapInput);
        MyEditor.Horizontal(Button);
        MyEditor.Space();
        MyEditor.Button(Combine, "Combine");
    }

    void DisplayTilemapInput()
    {
        for (int i = 0; i < mainTileMap.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            mainTileMap[i] = MyEditor.TILEMAP(mainTileMap[i], "Main Tilemap " + i);
            tilemapToCombine[i] = MyEditor.TILEMAP(tilemapToCombine[i], "Tilemap To Combine " + i);
            EditorGUILayout.EndHorizontal();
        }
    }
    void Button()
    {
        MyEditor.Button(Add, "Add");
        if (mainTileMap.Count > 1)
            MyEditor.Button(Remove, "Remove");
    }

    void Add()
    {
        mainTileMap.Add(null);
        tilemapToCombine.Add(null);
    }
    void Remove()
    {
        mainTileMap.RemoveAt(mainTileMap.Count - 1);
        tilemapToCombine.RemoveAt(tilemapToCombine.Count - 1);
    }

    void Combine()
    {
        for (int i = 0; i < mainTileMap.Count; i++)
        {
            BoundsInt bounds = tilemapToCombine[i].cellBounds;
            TileBase[] allTiles = tilemapToCombine[i].GetTilesBlock(bounds);
            tilemapToCombine[i].ClearAllTiles();

            offset = VectorValue.ToInt2(tilemapToCombine[i].transform.position - mainTileMap[i].transform.position);

            for (int x = bounds.position.x; x < bounds.size.x + bounds.position.x; x++)
            {
                for (int y = bounds.position.y; y < bounds.size.y + bounds.position.y; y++)
                {
                    TileBase tile = allTiles[(x - bounds.position.x) + (y - bounds.position.y) * bounds.size.x];
                    Vector3Int tilePosition = new Vector3Int(offset.x + x, offset.y + y, 0);

                    if(tile != null)
                        mainTileMap[i].SetTile(tilePosition, tile);
                }
            }

        }
    }
}
#endif
