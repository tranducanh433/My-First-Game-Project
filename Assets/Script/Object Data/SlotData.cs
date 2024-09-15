using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
public class SlotData
{

    public ItemBase item;
    public int amount;

    int maxAmount
    {
        get
        {
            if (item != null)
                return item.stackAmount;
            else
                return 0;
        }
    }
    public Sprite itemImage
    {
        get
        {
            if (item != null)
                return item.itemImage;
            else
                return null;
        }
    }
    public string itemName
    {
        get
        {
            if (item != null)
                return item.itemName;
            else
                return null;
        }
    }
    public string itemDescription
    {
        get
        {
            if (item != null)
                return item.itemDescription;
            else
                return null;
        }
    }

    public bool IsFull()
    {
        if (amount < maxAmount)
            return false;
        else
            return true;
    }
    public void SetNewItem(ItemBase _item, int _amount = 1)
    {
        item = _item;
        amount = _amount;
    }
    public void SetNewItem(SlotData newItem)
    {
        item = newItem.item;
        amount = newItem.amount;
    }
    public void Clear()
    {
        item = null;
        amount = 0;
    }
    public void AddAmount(int value)
    {
        if(amount + value <= maxAmount)
        {
            amount += value;
        }
        else
        {
            amount = maxAmount;
        }
    }
    public int OddAmount(int value)
    {
        if ((amount + value) <= maxAmount)
            return 0;
        else
            return (amount + value) - maxAmount;
    }
    public void SwitchWith(SlotData slotData)
    {
        SlotData m_data = new SlotData();
        m_data.SetNewItem(slotData);

        slotData.SetNewItem(item, amount);
        this.SetNewItem(m_data.item, m_data.amount);
    }
}
