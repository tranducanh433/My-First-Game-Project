using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimpleAnimation : MonoBehaviour
{
    public enum MoveDirection
    {
        up, down, right, left
    }

    public IEnumerator TextRevealEffect(TextMeshProUGUI TMPro, string content, IEnumerator endEffect, float timeBetweenChar = 0.05f)
    {
        TMPro.ClearMesh();
        foreach(char letter in content)
        {
            yield return new WaitForSeconds(timeBetweenChar);
            TMPro.text += letter;
        }

        if(endEffect != null)
            StartCoroutine(endEffect);
    }
    public IEnumerator TextRevealEffect(TextMeshProUGUI TMPro, string content, Action endEffect, float timeBetweenChar = 0.05f)
    {
        TMPro.text = "";
        foreach (char letter in content)
        {
            yield return new WaitForSeconds(timeBetweenChar);
            TMPro.text += letter;
        }

        if(endEffect != null)
            endEffect();
    }

    public IEnumerator FadedEffect(SpriteRenderer spriteRenderer, float second, Action endEffect)
    {
        float aStep = (1f / second) * Time.deltaTime;
        Color color = spriteRenderer.color;

        do
        {
            yield return null;
            spriteRenderer.color = new Color(color.r, color.g, color.b, color.a - aStep);
        }
        while (spriteRenderer.color.a > 0);


        if (endEffect != null)
            endEffect();
    }
    public IEnumerator FadedEffect(SpriteRenderer spriteRenderer, float second, IEnumerator endEffect)
    {
        float aStep = (1f / second) * Time.deltaTime;
        Color color = spriteRenderer.color;

        do
        {
            yield return null;
            spriteRenderer.color = new Color(color.r, color.g, color.b, color.a - aStep);
        }
        while (spriteRenderer.color.a > 0);

        if (endEffect == null)
            StartCoroutine(endEffect);
    }

    public IEnumerator Move(Transform _transform, MoveDirection moveDirection, float speed, float timeEnd, IEnumerator endEffect)
    {
        float currentTime = 0;

        do
        {
            yield return null;

            switch (moveDirection)
            {
                case MoveDirection.up:
                    _transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + (Vector2.up * speed * 100), speed);
                    break;
                case MoveDirection.down:
                    _transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + (Vector2.down * speed * 100), speed);
                    break;
                case MoveDirection.right:
                    _transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + (Vector2.right * speed * 100), speed);
                    break;
                case MoveDirection.left:
                    _transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + (Vector2.left * speed * 100), speed);
                    break;
                default:
                    break;
            }

            currentTime += Time.deltaTime;
        }
        while (currentTime < timeEnd);

        StartCoroutine(endEffect);
    }

    public IEnumerator Move(Transform _transform, MoveDirection moveDirection, float speed, float timeEnd, Action endEffect)
    {
        float currentTime = 0;

        do
        {
            yield return null;

            switch (moveDirection)
            {
                case MoveDirection.up:
                    _transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + (Vector2.up * speed * 100), speed);
                    break;
                case MoveDirection.down:
                    _transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + (Vector2.down * speed * 100), speed);
                    break;
                case MoveDirection.right:
                    _transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + (Vector2.right * speed * 100), speed);
                    break;
                case MoveDirection.left:
                    _transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + (Vector2.left * speed * 100), speed);
                    break;
                default:
                    break;
            }

            currentTime += Time.deltaTime;
        }
        while (currentTime < timeEnd);


        yield return null;
        if (endEffect != null)
            endEffect();
    }

    public void DestroyThisGameObject()
    {
        Destroy(gameObject);
    }
    public void UnActiveGameObject()
    {
        gameObject.SetActive(false);
    }
}
