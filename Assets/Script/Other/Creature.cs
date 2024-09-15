using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public int currentHp;
    public GameObject textDamage;
    public Element creatureElement;

    public virtual void TakeDamage(int value, Element attackElement)
    {
        Color textColor = Color.white;
        int damage = DamageCaculator(value, attackElement, out textColor);
        int finalDamage = Random.Range(damage - 1, damage + 2);

        currentHp -= finalDamage;

        GameObject text = Instantiate(textDamage, transform.position, Quaternion.identity);
        text.GetComponent<DamageText>().TextContent(transform.position, finalDamage, textColor);
    }
    protected int DamageCaculator(int damage, Element attackElement, out Color textColor)
    {
        if (creatureElement == Element.Earth)
        {
            if (attackElement == Element.Fire || attackElement == Element.Thunder)
            {
                textColor = new Color(0.5f, 0.5f, 0.5f);
                return (int)((float)damage * 0.75f);
            }
            if (attackElement == Element.Plant || attackElement == Element.Water)
            {
                textColor = new Color(1, 0.33f, 0.2f);
                return (int)((float)damage * 1.25f);
            }
        }
        if (creatureElement == Element.Fire)
        {
            if (attackElement == Element.Plant || attackElement == Element.Thunder)
            {
                textColor = new Color(0.5f, 0.5f, 0.5f);
                return (int)((float)damage * 0.75f);
            }
            if (attackElement == Element.Earth || attackElement == Element.Water)
            {
                textColor = new Color(1, 0.33f, 0.2f);
                return (int)((float)damage * 1.25f);
            }
        }
        if (creatureElement == Element.Plant)
        {
            if (attackElement == Element.Earth || attackElement == Element.Water)
            {
                textColor = new Color(0.5f, 0.5f, 0.5f);
                return (int)((float)damage * 0.75f);
            }
            if (attackElement == Element.Fire || attackElement == Element.Thunder)
            {
                textColor = new Color(1, 0.33f, 0.2f);
                return (int)((float)damage * 1.25f);
            }
        }
        if (creatureElement == Element.Thunder)
        {
            if (attackElement == Element.Plant || attackElement == Element.Water)
            {
                textColor = new Color(0.5f, 0.5f, 0.5f);
                return (int)((float)damage * 0.75f);
            }
            if (attackElement == Element.Earth || attackElement == Element.Fire)
            {
                textColor = new Color(1, 0.33f, 0.2f);
                return (int)((float)damage * 1.25f);
            }
        }
        if (creatureElement == Element.Water)
        {
            if (attackElement == Element.Fire || attackElement == Element.Earth)
            {
                textColor = new Color(0.5f, 0.5f, 0.5f);
                return (int)((float)damage * 0.75f);
            }
            if (attackElement == Element.Thunder || attackElement == Element.Plant)
            {
                textColor = new Color(1, 0.33f, 0.2f);
                return (int)((float)damage * 1.25f);
            }
        }
        //if(attackElement == enemyElement)
        textColor = Color.white;
        return damage;
    }
}
