using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumming Item", menuName = "Item Data/Consumming Item")]
public class ConsummingItem : ItemBase
{
    [Header("Consumming Item Setting")]
    public BuffType buffType;
    public int valueGain;
    public KindOfValue kindOfValue;
    public float availableTime;
    public float timeBetweenActivate;
}

public enum BuffType
{
    gainHpImmediately
}
public enum KindOfValue
{
    normal,
    percent
}
