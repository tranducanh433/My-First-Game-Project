using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Item Data/Weapon")]
public class WeaponItem : ItemBase
{
    [Header("Weapon Status")]
    public int ATK;
    public int AMPR;
    public int SPD;
    public int ASPD;
    public int manaNeed;

    [Header("Bullet Setting")]
    public AttackType attackType;
    public Element element;
    public GameObject bullet;
    public float bulletSpeed;
}

public enum AttackType
{
    waveAttack,
    shootElementBall,
    spellBook,
    orb
}
