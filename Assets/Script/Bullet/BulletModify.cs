using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreenSlime.Calculator;

public class BulletModify : MonoBehaviour
{
    public GameObject bulletExplodeEffect;
    private Vector2 endPoint;
    private int damage;
    private int numberOfBullet;
    private GameObject objectToSummon;
    private GameObject bullet;
    private float summonBulletSpeed;
    private float thisBulletSpeed;
    BulletColor bulletColor;

    public void SetBulletData(Vector2 endPoint, float mainBulletSpeed, float _summonBulletspeed, int damage, int numberOfBullet, GameObject objectToSummon, GameObject _bullet, BulletColor bulletColor)
    {
        this.bulletColor = bulletColor;
        this.endPoint = endPoint;
        summonBulletSpeed = _summonBulletspeed;
        thisBulletSpeed = mainBulletSpeed;
        this.numberOfBullet = numberOfBullet;
        this.objectToSummon = objectToSummon;
        bullet = _bullet;
        this.damage = damage;
    }


    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, endPoint, thisBulletSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, endPoint) <= 0.05f && GetComponent<SpriteRenderer>().sprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = null;
            GameObject effect = Instantiate(bulletExplodeEffect, transform.position, Quaternion.identity, transform);
            effect.GetComponent<BulletCallBack>().SetCallBack(ActivateEffect);
        }
    }
    void ActivateEffect()
    {
        Transform _player = GameObject.Find("Player").transform;
        float angleStep = 360 / numberOfBullet;
        float angle = 0;

        for (int i = 0; i < numberOfBullet; i++)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
            GameObject _bullet = Instantiate(bullet, transform.position, Quaternion.identity);

            _bullet.GetComponent<EnemyBullet>().BulletData(transform.right, summonBulletSpeed, damage, bulletColor, Element.None);

            angle += angleStep;
        }

        if(objectToSummon != null)
            Instantiate(objectToSummon, transform.position, Quaternion.identity);

        if(Vector2.Distance(transform.position, _player.position) <= 1)
        {
            _player.GetComponent<Player>().TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
