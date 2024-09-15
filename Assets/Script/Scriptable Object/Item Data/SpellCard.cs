using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell Card", menuName = "Item Data/Spell Card")]
public class SpellCard : ItemBase
{
    [Header("Spell Card Status")]
    public int damage;
    public Element element;

    [Header("Bullet Setting")]
    public GameObject bullet;
    public float bulletSpeed;
}
