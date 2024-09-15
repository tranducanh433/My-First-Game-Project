using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentData
{
    public ItemBase[] weapons = new ItemBase[3];
    public ItemBase[] skillOrb = new ItemBase[3];
    public ItemBase[] consummingItem = new ItemBase[1];

    public int Length()
    {
        return weapons.Length + skillOrb.Length + consummingItem.Length;
    }

}
