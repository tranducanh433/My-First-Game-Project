using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using GreenSlime.Editor;


#if UNITY_EDITOR
public class TileModified : EditorWindow
{
    public enum TilePosition {
        upLeft, up, upRight,
        left, defaultTile, right,
        downLeft, down, downRight,
        upLeftEdge, upRightEdge, downLeftEdge, downRightEdge,
        none
    }
    int width, height;
    bool modifiedEdge;
    Tilemap tilemapToModified;
    MyTilesRule tilesRule;

    Vector2 scrollDown;
    Vector3Int currentCell;

   [MenuItem("Tilemap Editor/Tile Modifide")]
    public static void OpenWindow()
    {
        EditorWindow window = GetWindow<TileModified>("Tile Modified");
        window.minSize = new Vector2(400, 400);
        window.maxSize = new Vector2(400, 500);
    }

    void OnGUI()
    {
        scrollDown = GUILayout.BeginScrollView(scrollDown, false, true);
        MyEditor.VerticalInBox(TilemapChoosed);
        modifiedEdge.TickBox("Modified Edge");

        MyEditor.Button(StartModified, "Start Modified");

        GUILayout.EndScrollView();
    }

    #region GUI Layout
    void TilemapChoosed()
    {
        tilemapToModified = MyEditor.TILEMAP(tilemapToModified, "Tilemap To Modified");
        tilesRule = (MyTilesRule)EditorGUILayout.ObjectField("Tile Rule", tilesRule, typeof(MyTilesRule), false);
    }
    #endregion

    #region System
    void StartModified()
    {
        BoundsInt mainBound = tilemapToModified.cellBounds;
        width = mainBound.size.x;
        height = mainBound.size.y;


        currentCell = tilemapToModified.WorldToCell(tilemapToModified.transform.position);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile tile = tilemapToModified.GetTile<Tile>(new Vector3Int(currentCell.x + x, currentCell.y + y, 0));
                //Debug.Log("X: " + x + ", Y: " + y + " " + tile.name);
                if(tile != null)
                {
                    TileNeighbourCheck(x, y);
                }
            }
        }
    }
    void TileNeighbourCheck(int x, int y)
    {
        List<Tile> neighbourTiles = new List<Tile>();
        for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
        {
            for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
            {
                Tile tile = tilemapToModified.GetTile<Tile>(new Vector3Int(currentCell.x + neighbourX, currentCell.y + neighbourY, 0));
                neighbourTiles.Add(tile);
            }
        }

        Tile TopLeftTile = neighbourTiles[2];
        Tile TopTile = neighbourTiles[5];
        Tile TopRightTile = neighbourTiles[8];

        Tile LeftTile = neighbourTiles[1];
        Tile RightTile = neighbourTiles[7];

        Tile DownLeftTile = neighbourTiles[0];
        Tile DownTile = neighbourTiles[3];
        Tile DownRightTile = neighbourTiles[6];

        /* 
         * 2 5 8
         * 1 4 7
         * 0 3 6
         */

        if (x == 0 && modifiedEdge == false)     //Left Edge
        {
            if(y == 0)
            {
                if (RightTile != null && TopTile == null)
                {
                    PlaceTile(TilePosition.down, x, y);
                }
                else if (RightTile == null && TopTile != null)
                {
                    PlaceTile(TilePosition.left, x, y);
                }
            }
            else if(y == height - 1)
            {
                if (RightTile != null && DownTile == null)
                {
                    PlaceTile(TilePosition.up, x, y);
                }
                else if (RightTile == null && DownTile != null)
                {
                    PlaceTile(TilePosition.left, x, y);
                }
            }
            else if (TopTile != null && DownTile != null && RightTile == null)
            {
                PlaceTile(TilePosition.left, x, y);
            }
            else if (TopTile != null && RightTile != null && TopRightTile == null)
            {
                PlaceTile(TilePosition.downLeft, x, y);
            }
            else if (DownTile != null && RightTile != null && DownRightTile == null)
            {
                PlaceTile(TilePosition.upLeft, x, y);
            }
            else if (TopTile != null && DownTile == null && RightTile == null)
            {
                PlaceTile(TilePosition.downRightEdge, x, y);
            }
            else if (DownTile != null && TopTile == null && RightTile == null)
            {
                PlaceTile(TilePosition.upRightEdge, x, y);
            }
            else if (RightTile != null && DownTile != null && TopTile == null)
            {
                PlaceTile(TilePosition.down, x, y);
            }
            else if (RightTile != null && TopTile != null && DownTile == null)
            {
                PlaceTile(TilePosition.up, x, y);
            }
        }
        else if(x == width - 1 && modifiedEdge == false)     //Right Edge
        {
            if (y == 0)
            {
                if (LeftTile != null && TopTile == null)
                {
                    PlaceTile(TilePosition.down, x, y);
                }
                else if (LeftTile == null && TopTile != null)
                {
                    PlaceTile(TilePosition.right, x, y);
                }
            }
            else if (y == height - 1)
            {
                if (LeftTile != null && DownTile == null)
                {
                    PlaceTile(TilePosition.up, x, y);
                }
                else if (LeftTile == null && DownTile != null)
                {
                    PlaceTile(TilePosition.right, x, y);
                }
            }
            else if (TopTile != null && DownTile != null && LeftTile == null)
            {
                PlaceTile(TilePosition.right, x, y);
            }
            else if (TopTile != null && LeftTile != null && TopLeftTile == null)
            {
                PlaceTile(TilePosition.downRight, x, y);
            }
            else if (DownTile != null && LeftTile != null && DownLeftTile == null)
            {
                PlaceTile(TilePosition.upRight, x, y);
            }
            else if (TopTile != null && DownTile == null && LeftTile == null)
            {
                PlaceTile(TilePosition.downLeftEdge, x, y);
            }
            else if (DownTile != null && TopTile == null && LeftTile == null)
            {
                PlaceTile(TilePosition.upLeftEdge, x, y);
            }
            else if (LeftTile != null && DownTile != null && TopTile == null)
            {
                PlaceTile(TilePosition.down, x, y);
            }
            else if (LeftTile != null && TopTile != null && DownTile == null)
            {
                PlaceTile(TilePosition.up, x, y);
            }
        }
        else if (y == 0 && modifiedEdge == false)
        {
            if (RightTile != null && LeftTile != null && TopTile == null)
            {
                PlaceTile(TilePosition.down, x, y);
            }
            else if (TopTile != null && LeftTile != null && TopLeftTile == null)
            {
                PlaceTile(TilePosition.downRight, x, y);
            }
            else if (TopTile != null && RightTile != null && TopRightTile == null)
            {
                PlaceTile(TilePosition.downLeft, x, y);
            }
            else if (RightTile != null && TopTile == null && LeftTile == null)
            {
                PlaceTile(TilePosition.upLeftEdge, x, y);
            }
            else if (LeftTile != null && TopTile == null && RightTile == null)
            {
                PlaceTile(TilePosition.upRightEdge, x, y);
            }
            else if (TopTile != null && LeftTile != null && RightTile == null)
            {
                PlaceTile(TilePosition.left, x, y);
            }
            else if (TopTile != null && RightTile != null && LeftTile == null)
            {
                PlaceTile(TilePosition.right, x, y);
            }
        }
        else if(y == height - 1 && modifiedEdge == false)
        {
            if (RightTile != null && LeftTile != null && DownTile == null)
            {
                PlaceTile(TilePosition.up, x, y);
            }
            else if (DownTile != null && LeftTile != null && DownLeftTile == null)
            {
                PlaceTile(TilePosition.upRight, x, y);
            }
            else if (DownTile != null && RightTile != null && DownRightTile == null)
            {
                PlaceTile(TilePosition.upLeft, x, y);
            }
            else if (RightTile != null && DownTile == null && LeftTile == null)
            {
                PlaceTile(TilePosition.downLeftEdge, x, y);
            }
            else if (LeftTile != null && DownTile == null && RightTile == null)
            {
                PlaceTile(TilePosition.downRightEdge, x, y);
            }
            else if(DownTile != null && LeftTile != null && RightTile == null)
            {
                PlaceTile(TilePosition.left, x, y);
            }
            else if (DownTile != null && RightTile != null && LeftTile == null)
            {
                PlaceTile(TilePosition.right, x, y);
            }
        }
        else if (ThisTileStayAlone(x, y))
        {
            PlaceTile(TilePosition.none, x, y);
        }
        else
        {
            if (DownTile != null && RightTile != null && DownRightTile == null)
            {
                PlaceTile(TilePosition.upLeft, x, y);
            }
            else if (LeftTile != null && DownTile != null && DownLeftTile == null)
            {
                PlaceTile(TilePosition.upRight, x, y);
            }
            else if (TopTile != null && RightTile != null && TopRightTile == null)
            {
                PlaceTile(TilePosition.downLeft, x, y);
            }
            else if (LeftTile != null && TopTile != null && TopLeftTile == null)
            {
                PlaceTile(TilePosition.downRight, x, y);
            }
            else if (LeftTile != null && RightTile != null && TopTile == null)
            {
                PlaceTile(TilePosition.down, x, y);
            }
            else if (DownTile != null && TopTile != null && RightTile == null)
            {
                PlaceTile(TilePosition.left, x, y);
            }
            else if (DownTile != null && TopTile != null && LeftTile == null)
            {
                PlaceTile(TilePosition.right, x, y);
            }
            else if (LeftTile != null && RightTile != null && DownTile == null)
            {
                PlaceTile(TilePosition.up, x, y);
            }
            else if (DownTile != null && RightTile != null && TopLeftTile == null)
            {
                PlaceTile(TilePosition.upLeftEdge, x, y);
            }
            else if (LeftTile != null && DownTile != null && TopRightTile == null)
            {
                PlaceTile(TilePosition.upRightEdge, x, y);
            }
            else if (TopTile != null && RightTile != null && DownLeftTile == null)
            {
                PlaceTile(TilePosition.downLeftEdge, x, y);
            }
            else if (LeftTile != null && TopTile != null && DownRightTile == null)
            {
                PlaceTile(TilePosition.downRightEdge, x, y);
            }
        }
    }

    bool ThisTileStayAlone(int x, int y)
    {
        int numberOfNeighbour = 0;
        List<Tile> neighbourTiles = new List<Tile>();

        Tile topTile = tilemapToModified.GetTile<Tile>(new Vector3Int(currentCell.x + x, currentCell.y + y + 1, 0));
        neighbourTiles.Add(topTile);

        Tile downTile = tilemapToModified.GetTile<Tile>(new Vector3Int(currentCell.x + x, currentCell.y + y - 1, 0));
        neighbourTiles.Add(downTile);

        Tile rightTile = tilemapToModified.GetTile<Tile>(new Vector3Int(currentCell.x + x + 1, currentCell.y + y, 0));
        neighbourTiles.Add(rightTile);

        Tile leftTile = tilemapToModified.GetTile<Tile>(new Vector3Int(currentCell.x + x - 1, currentCell.y + y, 0));
        neighbourTiles.Add(leftTile);

        foreach(Tile tile in neighbourTiles)
        {
            if(tile != null)
            {
                numberOfNeighbour++;
            }
        }

        if (numberOfNeighbour <= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void PlaceTile(TilePosition tilePosition, int x, int y)
    {
        Vector3Int tilePos = new Vector3Int(currentCell.x + x, currentCell.y + y, 0);
        switch (tilePosition)
        {
            case TilePosition.upLeft:
                tilemapToModified.SetTile(tilePos, tilesRule.upLeft);
                break;
            case TilePosition.up:
                tilemapToModified.SetTile(tilePos, tilesRule.up);
                break;
            case TilePosition.upRight:
                tilemapToModified.SetTile(tilePos, tilesRule.upRight);
                break;
            case TilePosition.left:
                tilemapToModified.SetTile(tilePos, tilesRule.left);
                break;
            case TilePosition.defaultTile:
                tilemapToModified.SetTile(tilePos, tilesRule.defaultTile);
                break;
            case TilePosition.right:
                tilemapToModified.SetTile(tilePos, tilesRule.right);
                break;
            case TilePosition.downLeft:
                tilemapToModified.SetTile(tilePos, tilesRule.downLeft);
                break;
            case TilePosition.down:
                tilemapToModified.SetTile(tilePos, tilesRule.down);
                break;
            case TilePosition.downRight:
                tilemapToModified.SetTile(tilePos, tilesRule.downRight);
                break;
            case TilePosition.upLeftEdge:
                tilemapToModified.SetTile(tilePos, tilesRule.upLeftEdge);
                break;
            case TilePosition.upRightEdge:
                tilemapToModified.SetTile(tilePos, tilesRule.upRightEdge);
                break;
            case TilePosition.downLeftEdge:
                tilemapToModified.SetTile(tilePos, tilesRule.downLeftEdge);
                break;
            case TilePosition.downRightEdge:
                tilemapToModified.SetTile(tilePos, tilesRule.downRightEdge);
                break;
            case TilePosition.none:
                tilemapToModified.SetTile(tilePos, null);
                break;
        }
    }
    #endregion
}
#endif
