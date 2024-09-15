using MyExtension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCardManager : MonoBehaviour
{
    public static SpellCardManager instance;

    public float spellCD = 20;
    public Image[] spellCardImage;
    public Image CDImage;
    float cd;

    List<SpellCard> spellCards = new List<SpellCard>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Update()
    {
        if(cd > 0)
        {
            cd -= Time.deltaTime;

            CDImage.fillAmount = cd / spellCD;
        }
    }

    public void ActivateSpellCard(out SpellCard spellCardUse)
    {
        if (cd <= 0)
        {
            if (spellCards.Count == 0 || spellCards == null)
            {
                spellCardUse = null;
                return;
            }

            spellCardUse = spellCards[0];
            spellCards.RemoveAt(0);
            GameManager.instance.RemoveSpellCard();
            cd = spellCD;
            UpdateDisplay();
        }
        else
        {
            spellCardUse = null;
        }
    }

    public bool CanActivateSpellCard()
    {
        return cd <= 0;
    }

    public void AddSpellCard()
    {
        List<SpellCard> equippedSpellCard = new List<SpellCard>();
        for (int i = 0; i < GameManager.instance.equippedSpellCards.Length; i++)
        {
            SlotData spellCardSlot = GameManager.instance.equippedSpellCards[i];

            if (spellCardSlot.item != null)
                equippedSpellCard.Add(spellCardSlot.item as SpellCard);
        }

        if (!equippedSpellCard.HaveValue())
            return;

        int r = Random.Range(0, equippedSpellCard.Count);

        spellCards.Add(equippedSpellCard[r]);
        GameManager.instance.AddSpellCard(equippedSpellCard[r]);
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < spellCardImage.Length; i++)
        {
            spellCardImage[i].sprite = null;
            spellCardImage[i].color = Color.clear;
        }

        for (int i = 0; i < spellCards.Count; i++)
        {
            spellCardImage[i].sprite = spellCards[i].itemImage;
            spellCardImage[i].color = Color.white;
        }
    }
}
