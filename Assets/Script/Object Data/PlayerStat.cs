using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    public int m_BaseATK;
    public int m_BaseHP;
    public int m_BaseMP;
    public int m_BaseSPD;
    public int m_BaseASPD;
    public int m_BaseAMPR;


    List<Stat> playerStat = new List<Stat>();

    public int ATK
    {
        get
        {
            int atk = m_BaseATK;
            for (int i = 0; i < playerStat.Count; i++)
            {
                atk += playerStat[i].ATK;
            }
            return atk;
        }
    }

    public int MaxHP
    {
        get
        {
            int hp = m_BaseHP;
            for (int i = 0; i < playerStat.Count; i++)
            {
                hp += playerStat[i].HP;
            }
            return hp;
        }
    }
    public int MaxMP
    {
        get
        {
            int mp = m_BaseMP;
            for (int i = 0; i < playerStat.Count; i++)
            {
                mp += playerStat[i].MP;
            }
            return mp;
        }
    }

    public float MoveSpeedForDisplay
    {
        get
        {
            int spd = m_BaseSPD;
            for (int i = 0; i < playerStat.Count; i++)
            {
                spd += playerStat[i].SPD;
            }
            return spd;
        }
    }
    public float MoveSpeed
    {
        get
        {
            int spd = m_BaseSPD;
            for (int i = 0; i < playerStat.Count; i++)
            {
                spd += playerStat[i].SPD;
            }
            return (float)spd / 10;
        }
    }
    public float AttackSpeed
    {
        get
        {
            int aspd = m_BaseASPD;
            for (int i = 0; i < playerStat.Count; i++)
            {
                aspd += playerStat[i].ASPD;

                if(aspd >= 100)
                {
                    aspd = 100;
                    break;
                }
            }
            return 1.1f - ((float)aspd / 100);
        }
    }
    public float AttackSpeedForDisplay
    {
        get
        {
            int aspd = m_BaseASPD;
            for (int i = 0; i < playerStat.Count; i++)
            {
                aspd += playerStat[i].ASPD;

                if (aspd >= 100)
                {
                    aspd = 100;
                    break;
                }
            }
            return aspd;
        }
    }
    public PlayerStat(int baseAtk, int baseHP, int baseMP, int baseSPD, int baseASPD, int baseAMPR)
    {
        m_BaseATK = baseAtk;
        m_BaseHP = baseHP;
        m_BaseMP = baseMP;
        m_BaseSPD = baseSPD;
        m_BaseASPD = baseASPD;
        m_BaseAMPR = baseAMPR;
    }

    public void ReplaceEquipment(Stat oldWeaponStat, Stat newWeaponStat)
    {
        if(oldWeaponStat != null)
            playerStat.Remove(oldWeaponStat);

        if(newWeaponStat != null)
            playerStat.Add(newWeaponStat);
    }

    public void AddNewStat(Stat statToAdd)
    {
        playerStat.Add(statToAdd);
    }
}
