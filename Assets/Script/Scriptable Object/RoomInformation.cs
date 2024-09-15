using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InGameTime
{
    Day,
    Night
}
[CreateAssetMenu(fileName = "Room Data", menuName = "Room Information")]
public class RoomInformation : ScriptableObject
{
    [Header("Enemy")]
    public GameObject[] enemyToSummon;
    public int numberAtDay;
    public int numberAtNight;

    [Header("Creatures")]
    public CreatureSpawn[] m_creatures;

    public GameObject[] GetArrayOfEnemies(InGameTime inGameTime)
    {
        List<GameObject> enemies = new List<GameObject>();

        if(inGameTime == InGameTime.Day)
        {
            for (int i = 0; i < numberAtDay; i++)
            {
                int r = Random.Range(0, enemyToSummon.Length);
                enemies.Add(enemyToSummon[r]);
            }
        }
        else if (inGameTime == InGameTime.Night)
        {
            for (int i = 0; i < numberAtNight; i++)
            {
                int r = Random.Range(0, enemyToSummon.Length);
                enemies.Add(enemyToSummon[r]);
            }
        }

        return enemies.ToArray();
    }

    public GameObject[] GetArrayOfCreatures(InGameTime inGameTime)
    {
        List<GameObject> creatures = new List<GameObject>();

        if (inGameTime == InGameTime.Day)
        {
            for (int i = 0; i < m_creatures.Length; i++)
            {
                int r_chance = Random.Range(0, 100);

                if (r_chance <= m_creatures[i].appearChanceAtDay)
                {
                    int r_amount = Random.Range(m_creatures[i].minAtDay, m_creatures[i].maxAtDay + 1);

                    for (int j = 0; j < r_amount; j++)
                    {
                        creatures.Add(m_creatures[i].creatures);
                    }
                }
            }
        }
        else if (inGameTime == InGameTime.Night)
        {
            for (int i = 0; i < m_creatures.Length; i++)
            {
                int r_chance = Random.Range(0, 100);

                if (r_chance <= m_creatures[i].appearChanceAtNight)
                {
                    int r_amount = Random.Range(m_creatures[i].minAtNight, m_creatures[i].maxAtNight + 1);

                    for (int j = 0; j < r_amount; j++)
                    {
                        creatures.Add(m_creatures[i].creatures);
                    }
                }
            }
        }

        return creatures.ToArray();
    }
}

[System.Serializable]
public class CreatureSpawn
{
    public GameObject creatures;

    [Header("Day")]
    [Range(0, 100)]
    public int appearChanceAtDay = 100;
    public int minAtDay;
    public int maxAtDay;

    [Header("Night")]
    [Range(0, 100)]
    public int appearChanceAtNight = 100;
    public int minAtNight;
    public int maxAtNight;
}
