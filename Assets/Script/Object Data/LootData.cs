using UnityEngine;

[System.Serializable]
public class LootData
{
    public ItemBase loot;
    public int minAmount;
    public int maxAmount;
    [Range(0, 100)]
    public int dropChange;

    public int RandomAmount()
    {
        if (dropChange == 0)
            return Random.Range(minAmount, maxAmount + 1);
        else
            return (Random.Range(0, 100) < dropChange) ? 1 : 0;
    }
}
