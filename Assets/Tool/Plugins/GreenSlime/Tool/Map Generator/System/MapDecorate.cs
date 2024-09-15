using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using GreenSlime.Editor;
using GreenSlime.Calculator;


#if UNITY_EDITOR
public class MapDecorate : EditorWindow
{
    public enum DecorateBuild
    {
        random,
        building
    }

    Tilemap groundTilemap;
    Tilemap decorateTilemap;

    int decorateRate = 5;
    int numberOfBuilding = 4;
    int minTileOfBuilding = 15;
    int maxTileOfBuilding = 30;
    int smooth = 4;

    DecorateBuild decorateBuild;

    TileArray tileArray;
    TileArray buildingArray;

    int width;
    int height;

    int[,] map;
    List<Vector3Int> buildingPosition = new List<Vector3Int>();
    List<Vector3Int> currentBuildingPos = new List<Vector3Int>();

    [MenuItem("Tilemap Editor/Decorate Generator")]
    public static void OpenWindow()
    {
        GetWindow<MapDecorate>("Decorate Generator");
    }

    private void OnGUI()
    {
        MyEditor.VerticalInBox(DisplayTimemap);
        MyEditor.VerticalInBox(DisplayTile);
        MyEditor.VerticalInBox(Setting);
        MyEditor.Button(StartDecorate, "Start Decorate");
    }

    #region GUI Display
    void DisplayTimemap()
    {
        MyEditor.Label("Tilemap");
        groundTilemap = MyEditor.TILEMAP(groundTilemap, "Ground Tilemap");
        decorateTilemap = MyEditor.TILEMAP(decorateTilemap, "Decorate Tilemap");
    }
    void DisplayTile()
    {
        MyEditor.Label("Decorate Tiles");
        if(decorateBuild == DecorateBuild.random)
            tileArray = (TileArray)EditorGUILayout.ObjectField("Tile Array", tileArray, typeof(TileArray), false);
        else if(decorateBuild == DecorateBuild.building)
            buildingArray = (TileArray)EditorGUILayout.ObjectField("Building Array", buildingArray, typeof(TileArray), false);
    }

    void Setting()
    {
        MyEditor.Label("Setting");
        decorateBuild = (DecorateBuild)EditorGUILayout.EnumPopup("Decorate Build", decorateBuild);

        if(decorateBuild == DecorateBuild.building)
        {
            numberOfBuilding = MyEditor.INT(numberOfBuilding, "Number Of Building");
            minTileOfBuilding = MyEditor.INT(minTileOfBuilding, "Min Tile Of Building");
            maxTileOfBuilding = MyEditor.INT(maxTileOfBuilding, "Max Tile Of Building");
            smooth = MyEditor.INT(smooth, "Smooth");
        }
        else if(decorateBuild == DecorateBuild.random)
        {
            decorateRate = MyEditor.INT(decorateRate, 0, 100, "DecorateRate");

        }
    }
    #endregion

    #region System
    void StartDecorate()
    {
        BoundsInt mainBound = groundTilemap.cellBounds;
        width = mainBound.size.x;
        height = mainBound.size.y;

        map = new int[width, height];

        if(decorateBuild == DecorateBuild.random)
        {
            RandomDecorate();
        }
        else if(decorateBuild == DecorateBuild.building)
        {
            buildingPosition.Clear();
            GetRandomPoint();
            BuildTheBuilding();
            for (int i = 0; i < smooth; i++)
            {
                SmoothMap();
            }
        }

        PlaceTile();
    }
    void RandomDecorate()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = (Random.Range(0, 100) < decorateRate) ? 1 : 0;
            }
        }
    }

    void GetRandomPoint()
    {
        Vector3Int _buildingPosition = VectorValue.RandomVector(Vector3Int.zero, new Vector3Int(width, height, 0));
        buildingPosition.Add(_buildingPosition);
        map[_buildingPosition.x, _buildingPosition.y] = 1;
    }
    void BuildTheBuilding()
    {
        for (int i = 0; i < numberOfBuilding; i++)
        {
            currentBuildingPos.Clear();
            currentBuildingPos.Add(buildingPosition[i]);
            int amountOfTiles = Random.Range(minTileOfBuilding, maxTileOfBuilding + 1);

            for (int j = 0; j < amountOfTiles; j++)
            {
                NeighbourGrid(currentBuildingPos.ToArray());
            }

            GetRandomPoint();
        }
    }

    void NeighbourGrid(Vector3Int[] buildingGrid)
    {
        List<Vector3Int> neighbour = new List<Vector3Int>();
        for (int i = 0; i < buildingGrid.Length; i++)
        {
            Vector3Int up = new Vector3Int(buildingGrid[i].x, buildingGrid[i].y + 1, 0);
            Vector3Int right = new Vector3Int(buildingGrid[i].x + 1, buildingGrid[i].y, 0);
            Vector3Int down = new Vector3Int(buildingGrid[i].x, buildingGrid[i].y - 1, 0);
            Vector3Int left = new Vector3Int(buildingGrid[i].x - 1, buildingGrid[i].y, 0);

            if (decorateTilemap.GetTile(up) == null && neighbour.Contains(up) == false && InMapRange(up))
                neighbour.Add(up);
            if (decorateTilemap.GetTile(right) == null && neighbour.Contains(right) == false && InMapRange(right))
                neighbour.Add(right);
            if (decorateTilemap.GetTile(down) == null && neighbour.Contains(down) == false && InMapRange(down))
                neighbour.Add(down);
            if (decorateTilemap.GetTile(left) == null && neighbour.Contains(left) == false && InMapRange(left))
                neighbour.Add(left);
        }
        if(neighbour.Count > 0)
        {
            int choosingTile = Random.Range(0, neighbour.Count);
            currentBuildingPos.Add(neighbour[choosingTile]);
            map[neighbour[choosingTile].x, neighbour[choosingTile].y] = 1;
        }

    }

    bool InMapRange(Vector3Int m_pos)
    {
        if(m_pos.x >= width || m_pos.x < 0 || m_pos.y >= height || m_pos.y < 0)
        {
            return false;
        }
        return true;
    }
    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundWallCount(x, y);

                if (neighbourWallTiles > 4)
                    map[x, y] = 1;
                /*else if (neighbourWallTiles < 4)
                    map[x, y] = 0;*/
            }
        }
    }

    int GetSurroundWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                    else
                        wallCount++;
                }
            }
        }

        return wallCount;
    }

    void PlaceTile()
    {
        decorateTilemap.ClearAllTiles();
        Vector3Int currentDecorateCell = decorateTilemap.WorldToCell(decorateTilemap.transform.position);

        Vector3Int currentGroundCell = groundTilemap.WorldToCell(groundTilemap.transform.position);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1 && groundTilemap.GetTile(new Vector3Int(currentGroundCell.x + x, currentGroundCell.y + y, 0)) != null)
                {
                    if (decorateBuild == DecorateBuild.random)
                    {
                        int r = Random.Range(0, tileArray.tiles.Length);
                        Vector3Int decoratePosition = new Vector3Int(currentDecorateCell.x + x, currentDecorateCell.y + y, 0);
                        decorateTilemap.SetTile(decoratePosition, tileArray.tiles[r]);
                    }
                    else if(decorateBuild == DecorateBuild.building)
                    {
                        int r = Random.Range(0, buildingArray.tiles.Length);
                        Vector3Int decoratePosition = new Vector3Int(currentDecorateCell.x + x, currentDecorateCell.y + y, 0);
                        decorateTilemap.SetTile(decoratePosition, buildingArray.tiles[r]);
                    }
                }
            }
        }
    }

    #endregion
}
#endif

/*public interface B
{
    public static string aasd()
    {
        return "";
    }
}*/
