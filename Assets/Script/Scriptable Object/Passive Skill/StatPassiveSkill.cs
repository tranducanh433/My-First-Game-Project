using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stat Skill", menuName = "Passive Skill/Gain Stat Passive Skill")]
public class StatPassiveSkill : PassiveSkillBase
{
    [Header("Gain Stat")]
    public Stat stat;
}
