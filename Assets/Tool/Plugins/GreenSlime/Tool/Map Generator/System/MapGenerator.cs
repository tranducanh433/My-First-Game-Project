using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using GreenSlime.Editor;

#if UNITY_EDITOR
public class MapGenerator : EditorWindow
{
    [Header("Tile")]
    public Tilemap floor;
    public Tilemap wall;
    [Space]
    public Tile floorTile;
    public Tile wallTile;

    [Header("Setting")]
    public int width = 40;
    public int height = 40;

    public int fillRate = 40;
    public int smooth = 5;

    public string seed;
    public bool useRandomSeed = true;

    private bool openSeendSetting;
    private Vector2 scrollDown;

    int[,] map;

    [MenuItem("Tilemap Editor/Map Generation")]
    public static void OpenWindow()
    {
        EditorWindow window = GetWindow<MapGenerator>("Map Generator");
        window.minSize = new Vector2(400, 400);
        window.maxSize = new Vector2(400, 500);
    }

    void OnGUI()
    {
        scrollDown = GUILayout.BeginScrollView(scrollDown, false, true);
        MyEditor.VerticalInBox(Tilemap);
        MyEditor.VerticalInBox(MapSize);
        MyEditor.VerticalInBox(Setting);
        MyEditor.VerticalInBox(SeedSetting);
        MyEditor.Horizontal(MapButton);
        GUILayout.EndScrollView();
    }

    #region GUI
    void Tilemap()
    {
        MyEditor.Label("Tilemap");
        floor = MyEditor.TILEMAP(floor, "Floor Tilemap");
        wall = MyEditor.TILEMAP(wall, "Wall Tilemap");

        MyEditor.Space();

        floorTile = MyEditor.TILE(floorTile, "Floor Tile");
        wallTile = MyEditor.TILE(wallTile, "Wall Tile");

        MyEditor.Label("Decorate", false);
    }

    void MapSize()
    {
        MyEditor.Label("Map Size");
        width = MyEditor.INT(width, "Width");
        height = MyEditor.INT(height, "Height");
    }

    void Setting()
    {
        MyEditor.Label("Setting");

        fillRate = MyEditor.INT(fillRate, 0, 100, "Fill Rate");
        smooth = MyEditor.INT(smooth, "Smooth");
    }
    void SeedSetting()
    {
        openSeendSetting = MyEditor.Foldout(openSeendSetting, "Seed Setting");

        if (openSeendSetting)
        {
            seed = MyEditor.TextField(seed, "Seed");
            useRandomSeed = MyEditor.BOOL(useRandomSeed, "Use Random Seed");
        }
    }
    void MapButton()
    {
        MyEditor.Button(GenerationMap, "Generation Map");
        MyEditor.Button(RotateTilemap, "Rotate Map");
    }
    #endregion

    #region System

    public void GenerationMap()
    {
        if(floor == null || wall == null)
        {
            Debug.LogError("Floor tilemap and Wall tilemap can not be null. Please set a value for it");
            return;
        }

        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < smooth; i++)
        {
            SmoothMap();
        }

        PlaceTile();
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random psuedoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(x == 0 || x == width - 1 || y == 0 || y == height - 1) //Create Border
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (psuedoRandom.Next(0, 100) < fillRate) ? 1 : 0;
                }
            }
        }
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
                else if (neighbourWallTiles < 4)
                    map[x, y] = 0;
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
                if(neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    void PlaceTile()
    {
        floor.ClearAllTiles();
        wall.ClearAllTiles();

        Vector3Int currentFloorCell = floor.WorldToCell(floor.transform.position);
        Vector3Int currentWallCell = wall.WorldToCell(wall.transform.position);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //Floor
                Vector3Int tilePosition = new Vector3Int(currentFloorCell.x + x, currentFloorCell.y + y, 0);
                floor.SetTile(tilePosition, floorTile);


                if (map[x, y] == 1)
                {
                    //wall
                    Vector3Int wallTilePosition = new Vector3Int(currentWallCell.x + x, currentWallCell.y + y, 0);
                    wall.SetTile(wallTilePosition, wallTile);
                }
            }
        }
    }


    public void RotateTilemap()
    {
        Vector3Int currentFloorCell = floor.WorldToCell(floor.transform.position);
        Vector3Int currentWallCell = wall.WorldToCell(wall.transform.position);

        List<Tile> floorTiles = new List<Tile>();
        List<Tile> wallTiles = new List<Tile>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                floorTiles.Add(floor.GetTile<Tile>(new Vector3Int(currentFloorCell.x + x, currentFloorCell.y + y, 0)));
                wallTiles.Add(wall.GetTile<Tile>(new Vector3Int(currentWallCell.x + x, currentWallCell.y + y, 0)));
            }
        }
        int storage = height;
        height = width;
        width = storage;

        floor.ClearAllTiles();
        wall.ClearAllTiles();

        int i = 0;
        for (int y = height - 1; y > - 1; y--)
        {
            for (int x = 0; x < width; x++)
            {
                //wall
                Vector3Int wallTilePosition = new Vector3Int(currentWallCell.x + x, currentWallCell.y + y, 0);
                wall.SetTile(wallTilePosition, wallTiles[i]);
                //Floor
                Vector3Int floorTilePosition = new Vector3Int(currentFloorCell.x + x, currentFloorCell.y + y, 0);
                floor.SetTile(floorTilePosition, floorTiles[i]);

                i++;
            }
        }
    }
    #endregion
}
#endif
