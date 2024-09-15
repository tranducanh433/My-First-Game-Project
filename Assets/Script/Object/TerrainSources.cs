using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSources : Creature
{
    [Header("Setting")]
    public LootTable lootTable;
    public SpriteRenderer sr;

    [Header("Resources")]
    public GameObject itemDropPrefab;
    public Material whitePlashEffect;

    public override void TakeDamage(int value, Element attackElement)
    {
        base.TakeDamage(value, attackElement);

        StartCoroutine(TakeDamageEffectCO());
        TakeDamageEffect();

        if (currentHp <= 0)
        {
            DropItem();
            DestroyEffect();
        }
    }

    protected virtual void TakeDamageEffect() { }

    void DropItem()
    {
        if (lootTable != null)
        {
            int[] lootAmount = lootTable.GetLootArray();

            for (int i = 0; i < lootAmount.Length; i++)
            {
                for (int j = 0; j < lootAmount[i]; j++)
                {
                    GameObject item = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
                    ItemDrop m_itemDrop = item.GetComponent<ItemDrop>();
                    m_itemDrop.SetItem(lootTable.GetItem(i));
                }
            }
        }
    }

    protected virtual void DestroyEffect()
    {
        Destroy(gameObject);
    }
    IEnumerator TakeDamageEffectCO()
    {
        Material storageMaterial = sr.material;
        sr.material = whitePlashEffect;
        yield return new WaitForSeconds(0.2f);
        sr.material = storageMaterial;
    }

    private void Reset()
    {
        textDamage = Resources.Load("Prefab/Damage Text") as GameObject;
        itemDropPrefab = Resources.Load("Prefab/Item Drop") as GameObject;
        whitePlashEffect = Resources.Load("Material/Plash White") as Material;

        sr = GetComponent<SpriteRenderer>();
    }
}
