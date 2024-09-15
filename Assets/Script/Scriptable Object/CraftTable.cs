using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Craft Table", menuName = "Craft Table")]
public class CraftTable : ScriptableObject
{
    public ItemBase[] itemCanCraft;

    public Sprite GetItemSprite(int index)
    {
        return itemCanCraft[index].itemImage;
    }

    public int MaterialLength(int index)
    {
        return itemCanCraft[index].materialLength;
    }
    public int ItemLength => itemCanCraft.Length;
    public void GetMaterial(int index, out ItemBase[] itemBases, out int[] amountNeed)
    {
        List<ItemBase> m_item = new List<ItemBase>();
        List<int> m_amount = new List<int>();

        for (int i = 0; i < itemCanCraft[index].materialLength; i++)
        {
            int amount = 0;
            ItemBase item = null;

            itemCanCraft[index].GetCraftMaterialInfor(i, out item, out amount);

            m_item.Add(item);
            m_amount.Add(amount);
        }

        itemBases = m_item.ToArray();
        amountNeed = m_amount.ToArray();
    }

    public void GetMaterialArray(ItemBase _item, out ItemBase[] itemBases, out int[] amountNeed)
    {
        List<ItemBase> m_item = new List<ItemBase>();
        List<int> m_amount = new List<int>();

        int index = ItemIndex(_item);

        for (int i = 0; i < itemCanCraft[index].materialLength; i++)
        {
            int amount = 0;
            ItemBase item = null;

            itemCanCraft[index].GetCraftMaterialInfor(i, out item, out amount);

            m_item.Add(item);
            m_amount.Add(amount);
        }

        itemBases = m_item.ToArray();
        amountNeed = m_amount.ToArray();
    }

    int ItemIndex(ItemBase itemToFind)
    {
        for (int i = 0; i < itemCanCraft.Length; i++)
        {
            if (itemCanCraft[i] == itemToFind)
                return i;
        }

        return -1;
    }
}
