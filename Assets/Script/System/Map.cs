using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    Tilemap wallTilemap;

    private void Start()
    {
        wallTilemap = transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();    
    }

    public bool CanSummon(Vector2Int summonPos)
    {
        Vector3Int startCellPos = wallTilemap.WorldToCell((Vector3Int)summonPos);

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighborCell = new Vector3Int(startCellPos.x + x, startCellPos.y + y, 0);

                if (wallTilemap.GetTile<Tile>(neighborCell) != null)
                    return false;
            }
        }

        return true;
    }
}
