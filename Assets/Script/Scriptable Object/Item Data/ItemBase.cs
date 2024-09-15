using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    [TextArea]
    public string itemDescription;
    public int stackAmount = 99;

    [Header("Material Need")]
    public MaterialNeed[] materialNeeds;

    public int materialLength => materialNeeds.Length;

    public void GetCraftMaterialInfor(int index, out ItemBase item, out int amount)
    {
        item = materialNeeds[index].material;
        amount = materialNeeds[index].amountNeed;
    }
    public ItemBase[] GetMaterialArray()
    {
        List<ItemBase> items = new List<ItemBase>();

        for (int i = 0; i < materialLength; i++)
        {
            items.Add(materialNeeds[i].material);
        }

        return items.ToArray();
    }
    public int[] GetMaterialAmountArray()
    {
        List<int> amounts = new List<int>();

        for (int i = 0; i < materialLength; i++)
        {
            amounts.Add(materialNeeds[i].amountNeed);
        }

        return amounts.ToArray();
    }
}
