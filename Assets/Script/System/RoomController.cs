using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreenSlime.Calculator;
using MyExtension;

public class RoomController : MonoBehaviour
{
    public Vector2[] m_summonPosition;
    public RoomInformation roomInformation;
    [Space]
    GameObject CMVcam;
    public GameObject plashEffect;
    Vector2 lastEnemy;

    private Vector2 minPos;
    private Vector2 maxPos;
    GameObject[] enemiesToSummon;
    GameObject[] creaturesToSummon;

    private Vector2[] colliderPoints;

    int ID;
    private bool isBeat;
    private bool playerIsIn;
    PolygonCollider2D polygonCollider2D;
    Map map;

    void Start()
    {
        ID = transform.ToSiblingIndex();
        map = GetComponent<Map>();
        CMVcam = transform.GetChild(1).gameObject;

        ReadColliderInformation();
        GetSummonData();
        //StartCoroutine(SummonCreatorCO());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(roomInformation != null)
                StartCoroutine(SummonCreatorCO());
        }
    }
    IEnumerator SummonCreatorCO()
    {
        yield return null;
        SummonCreatures();
    }

    void GetSummonData()
    {
        if(roomInformation != null)
        {
            enemiesToSummon = roomInformation.GetArrayOfEnemies(InGameTime.Day);
            creaturesToSummon = roomInformation.GetArrayOfCreatures(InGameTime.Day);
        }
    }

    void ReadColliderInformation()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        colliderPoints = polygonCollider2D.GetPath(0);

        for (int i = 0; i < colliderPoints.Length; i++)
        {
            //Get Min Position
            if(minPos == null)
            {
                minPos = colliderPoints[i];
            }
            else if(colliderPoints[i].x <= minPos.x && colliderPoints[i].y <= minPos.y)
            {
                minPos = colliderPoints[i];
            }

            //Get Max Position
            if (maxPos == null)
            {
                maxPos = colliderPoints[i];
            }
            else if (colliderPoints[i].x >= maxPos.x && colliderPoints[i].y >= maxPos.y)
            {
                maxPos = colliderPoints[i];
            }
        }

        //Fix the point to make sure that the enemy wont spawn inside the wall
        minPos = minPos + Vector2.one + (Vector2)transform.position;
        maxPos = maxPos - Vector2.one + (Vector2)transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Immune"))
        {
            GameObject minimapObj = GameObject.Find("Minimap");
            Minimap minimap = minimapObj.GetComponent<Minimap>();
            minimap.SetPlayerPosition(ID);

            if (isBeat == false && roomInformation != null)
            {
                SummonEnemies();
            }

            playerIsIn = true;
            CMVcam.SetActive(true);
            GameManager.instance.ChangeCurrentCam(CMVcam);
        }
    }
    void SummonEnemies()
    {
        for (int i = 0; i < enemiesToSummon.Length; i++)
        {
            if(m_summonPosition.Length == 0)
            {
                Vector2Int positionInTile = VectorValue.RandomVector(minPos, maxPos).ToVector2Int();

                do
                {
                    positionInTile = VectorValue.RandomVector(minPos, maxPos).ToVector2Int();
                }
                while (map.CanSummon(positionInTile) == false);

                Instantiate(enemiesToSummon[i], (Vector2)positionInTile, Quaternion.identity, transform);
            }
            else
            {
                if (i < m_summonPosition.Length)
                    Instantiate(enemiesToSummon[i], m_summonPosition[i], Quaternion.identity, transform);
            }
        }
    }
    void SummonCreatures()
    {
        if (roomInformation == null) return;
        for (int i = 0; i < creaturesToSummon.Length; i++)
        {
            Vector2Int positionInTile = VectorValue.RandomVector(minPos, maxPos).ToVector2Int();

            do
            {
                positionInTile = VectorValue.RandomVector(minPos, maxPos).ToVector2Int();
            }
            while (map.CanSummon(positionInTile) == false);

            Instantiate(creaturesToSummon[i], (Vector2)positionInTile, Quaternion.identity);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Immune"))
        {
            playerIsIn = false;
            CMVcam.SetActive(false);
            DestroyAllEnemy();
        }
    }
    void DestroyAllEnemy()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Enemy"))
                Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void OnTransformChildrenChanged()
    {
        if (playerIsIn && NoEnemyInThisRoom())
        {
            SummonEffect();
            isBeat = true;
            GameManager.instance.StageClear();
        }
        else if(playerIsIn)
        {
            lastEnemy = transform.GetChild(1).position;

        }
    }
    bool NoEnemyInThisRoom()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Enemy"))
            {
                return false;
            }
        }

        return true;
    }

    void SummonEffect()
    {
        GameObject effect = Instantiate(plashEffect, lastEnemy, Quaternion.identity);
        effect.GetComponent<PlashEffect>().SetZoomOutData(15, 30, Color.white);
    }
}
