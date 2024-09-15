using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Particle : MonoBehaviour
{
    Gradient gradient;
    Sprite[] sprites;
    float lifeTime;
    Image image;
    float cd;
    List<float> spriteStep = new List<float>();
    private void Start()
    {
        image = gameObject.AddComponent<Image>();
        image.color = gradient.Evaluate(1);
        image.sprite = sprites[0];
        image.SetNativeSize();
        cd = lifeTime;
    }

    void Update()
    {
        for (int i = spriteStep.Count - 1; i >= 0; i--)
        {
            if (cd <= spriteStep[i])
            {
                image.sprite = sprites[i];
                break;
            }
        }

        cd -= Time.deltaTime;
        float time = 1 - cd / lifeTime;
        image.color = gradient.Evaluate(time);

        if(cd <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetValue(Sprite[] _sprites, Vector2 dir, float _speed, Gradient _gradient, float lifeTime)
    {
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.velocity = dir * _speed * 100;
        gradient = _gradient;
        this.lifeTime = lifeTime;
        sprites = _sprites;

        float step = lifeTime / _sprites.Length;
        for (int i = 0; i < _sprites.Length; i++)
        {
            spriteStep.Add(lifeTime);
            lifeTime -= step;
        }
    }
}
