using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tiles Name", menuName = "Other ScriptableObject/Tilemap/Tile Array")]
public class TileArray : ScriptableObject
{
    public Tile[] tiles;
}
