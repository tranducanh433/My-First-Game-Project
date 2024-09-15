using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot Table", menuName = "Loot Table")]
public class LootTable : ScriptableObject
{
    public LootData[] loots = new LootData[1];

    public int[] GetLootArray()
    {
        List<int> amountLoot = new List<int>();

        for (int i = 0; i < loots.Length; i++)
        {
            amountLoot.Add(loots[i].RandomAmount());
        }

        return amountLoot.ToArray();
    }
    public ItemBase GetItem(int i)
    {
        return loots[i].loot;
    }
}
