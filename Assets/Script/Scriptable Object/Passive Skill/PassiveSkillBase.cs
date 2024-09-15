using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillBase : ScriptableObject
{
    public Sprite skillImage;
    public string skillName;
    [TextArea]
    public string description;
    public int maxLevel = 1;
}
