using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Name", menuName ="Creature Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;

    [Header("Status")]
    public Element element;
    public int HP = 3;
    public int ATK = 1;
    public float ATKSpeed = 1;
    public int DEF;
    public float moveSpeed = 3;
    public float chaseRarius = 9999;
    public float attackRadius = 2;

    [Header("Drop")]
    public int EXPPerCell;
    public int numberOfEXPCell;


    public LootTable lootTable;

    public int[] GetLootArray()
    {
        return lootTable.GetLootArray();
    }
    public ItemBase GetItem(int i)
    {
        return lootTable.loots[i].loot;
    }
}

public enum Element
{
    None,
    Fire,
    Water,
    Plant,
    Earth,
    Thunder
}
