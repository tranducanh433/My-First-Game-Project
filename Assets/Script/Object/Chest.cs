using MyExtension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public ChestInfor chestInformation;
    public GameObject itemDrop;
    public bool opened;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.CompareTag("Player") || collision.CompareTag("Immune")) && !opened)
        {
            GetComponent<Animator>().SetTrigger("Open");
        }
    }

    public void Open_Chest()
    {
        StartCoroutine(OpenChestCO());
        opened = true;
    }

    IEnumerator OpenChestCO()
    {
        for (int i = 0; i < chestInformation.itemDrop.Length; i++)
        {
            ItemBase item;
            int amount;
            (item, amount) = chestInformation.GetDropInfor(i);

            for (int j = 0; j < amount; j++)
            {
                yield return new WaitForSeconds(0.02f);
                GameObject _itemDrop = Instantiate(itemDrop, transform.position.Add(0, 0.5f), Quaternion.identity);
                _itemDrop.GetComponent<ItemDrop>().SetItem(item);
            }
        }
    }

    private void Reset()
    {
        itemDrop = Resources.Load("Prefab/Item Drop") as GameObject;
    }
}
