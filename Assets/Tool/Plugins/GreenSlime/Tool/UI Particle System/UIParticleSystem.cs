using GreenSlime.Calculator;
using MyExtension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParticleSystem : MonoBehaviour
{
    [Header("Particle System")]
    public float duration;
    [Space]
    public float minLifeTime;
    public float maxLifeTime;
    [Space]
    public float minSpeed;
    public float maxSpeed;
    [Header("Emission")]
    public int rateOvertime;
    public int burst;
    [Header("Shape")]
    public float minX;
    public float maxX;

    Vector2 maxPos;
    Vector2 minPos;
    [Header("Color Over LifeTime")]
    public Gradient gradient;
    [Header("Texture Sheet Animation")]
    public Sprite[] sprites;


    public void Play()
    {
        minPos = new Vector2(transform.position.x + minX, transform.position.y);
        maxPos = new Vector2(transform.position.x + maxX, transform.position.y);

        StartCoroutine(PlayCO());
    }

    IEnumerator PlayCO()
    {
        float currentTime = 0;
        float timeStep = 0;
        float rateOffset = 0;
        float mutiTime = 1;

        if (rateOvertime < 60)
        {
            timeStep = 1f / rateOvertime;
        }
        else
        {
            timeStep = Time.deltaTime;
            rateOffset = rateOvertime % 60;
            mutiTime = rateOvertime / 60;
        }

        do
        {
            if(currentTime == 0)
            {
                for (int i = 0; i < burst; i++)
                {
                    CreateObject();
                }
            }

            for (int i = 0; i < mutiTime; i++)
            {
                CreateObject();
            }
            if(rateOffset > 0)
            {
                CreateObject();
                rateOffset--;
            }


            currentTime += timeStep;
            yield return new WaitForSeconds(timeStep);
        }
        while (currentTime < duration);
        
    }

    void CreateObject()
    {
        Transform obj = new GameObject("Particle").transform;
        obj.parent = transform;
        obj.position = VectorValue.RandomVector(minPos, maxPos);

        Vector2 dir = Random.Range(0f, 360f).ToDirection();
        float r_Speed = Random.Range(minSpeed, maxSpeed);
        float r_LifeTime = Random.Range(minLifeTime, maxLifeTime);

        Particle par_obj = obj.gameObject.AddComponent<Particle>();
        par_obj.SetValue(sprites, dir, r_Speed, gradient, r_LifeTime);
    }
}
