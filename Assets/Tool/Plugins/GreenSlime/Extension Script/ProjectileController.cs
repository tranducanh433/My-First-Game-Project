using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectileController
{
    public class Bullet : MonoBehaviour
    {
        public static GameObject[] Spread(GameObject bulletPrefab, Transform shootPos, int numberOfBullet, float startAtAngle)
        {
            List<GameObject> bullets = new List<GameObject>();
            float angleStep = 360 / numberOfBullet;
            float currentAngle = startAtAngle;

            for (int i = 0; i < numberOfBullet; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, shootPos.position, Quaternion.Euler(0, 0, currentAngle));
                bullets.Add(bullet);

                currentAngle += angleStep;
            }

            return bullets.ToArray();
        }
        public static GameObject[] SpreadRandom(GameObject bulletPrefab, Transform shootPos, int numberOfBullet, float minAngle, float maxAngle)
        {
            List<GameObject> bullets = new List<GameObject>();

            for (int i = 0; i < numberOfBullet; i++)
            {
                float angle = Random.Range(minAngle, maxAngle);
                GameObject bullet = Instantiate(bulletPrefab, shootPos.position, Quaternion.Euler(0, 0, angle));
                bullets.Add(bullet);
            }

            return bullets.ToArray();
        }

    }
}