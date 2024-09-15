using GreenSlime.Calculator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCrystal : TerrainSources
{
    public GameObject prizePrefab;

    protected override void DestroyEffect()
    {
        for (int i = 0; i < 15; i++)
        {
            float angle = Random.Range(0, 360);
            GameObject obj = Instantiate(prizePrefab, transform.position, Quaternion.Euler(0, 0, angle));
            Rigidbody2D objrb = obj.GetComponent<Rigidbody2D>();
            objrb.velocity = VectorValue.GetDirectionFromAngle(angle) * 5f;


        }
        base.DestroyEffect();
    }
}
