using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public TextMeshPro damageText;

    private Vector2 startPoint;

    public void TextContent(Vector2 current, int damage, Color textColor)
    {
        startPoint = new Vector2(Random.Range(current.x - 0.3f, current.x + 0.3f),
                                current.y);
        transform.position = startPoint;

        damageText.text = damage.ToString();
        damageText.color = textColor;
    }

    private void OnTransformChildrenChanged()
    {
        Destroy(gameObject);
    }
}
