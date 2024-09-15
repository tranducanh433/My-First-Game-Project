using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tiles Name", menuName = "Other ScriptableObject/Tilemap/Tiles Rule")]
public class MyTilesRule : ScriptableObject
{
    public Tile upLeft, up, upRight,
    left, defaultTile, right,
    downLeft, down, downRight;

    public Tile upLeftEdge, upRightEdge, downLeftEdge, downRightEdge;
}
