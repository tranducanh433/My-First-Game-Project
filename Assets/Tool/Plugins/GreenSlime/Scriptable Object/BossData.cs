using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Name", menuName = "Creature Data/Boss Data")]
public class BossData : EnemyData
{
    public int[] HPEachPhase;
}
