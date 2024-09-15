using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PassiveSkillController : MonoBehaviour
{
    public TextMeshProUGUI skillPointText;
    public PassiveSkillSlot[] passiveSkillSlots;
    public PassiveSkillBase[] passiveSkill;

    Stat passiveSkillStat;
    GameManager GM;

    void Start()
    {
        GM = GameManager.instance;
        UpdateData();
        UpdateSkillInfor();

        passiveSkillStat = GM.GetPassiveStatData();
    }
    public void UpdateData()
    {
        skillPointText.text = GameManager.instance.skillPoint.ToString();

    }

    public void UpdateSkillInfor()
    {
        for (int i = 0; i < passiveSkillSlots.Length; i++)
        {
            passiveSkillSlots[i].UpdateData(passiveSkill[i]);
        }
    }

    public void UpdateStat()
    {
        
    }
}
