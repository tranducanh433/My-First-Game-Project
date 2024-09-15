using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chest Information", menuName = "Chest Information")]
public class ChestInfor : ScriptableObject
{
    public ItemBase[] itemDrop;
    public int[] amountDrop;

    public (ItemBase, int) GetDropInfor(int index)
    {
        return (itemDrop[index], amountDrop[index]);
    }
}
