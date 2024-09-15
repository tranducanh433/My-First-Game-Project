using GreenSlime.Calculator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornPlant : TerrainSources
{
    public GameObject bulletPrefab;

    protected override void DestroyEffect()
    {
        float angle = 0;
        for (int i = 0; i < 8; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle));
            Vector2 dir = VectorValue.GetDirectionFromAngle(angle);
            bullet.GetComponent<EnemyBullet>().BulletData(dir, 15, 5, BulletColor.Green, Element.Plant, true);

            angle += (360 / 8);
        }
        base.DestroyEffect();
    }
}
