using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    public static CraftingUI instance;

    public CraftItemSlot[] craftItemSlots;
    public CraftItemSlot displaySlot;
    public CraftItemSlot[] displayMaterial;

    CraftTable m_craftTable;

    ItemBase selectedItem;
    int[] m_amounts;
    ItemBase[] m_items;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowMaterialNeed(ItemBase item)
    {
        HideCraftInfor();

        selectedItem = item;
        displaySlot.ShowItem(item);

        m_craftTable.GetMaterialArray(item, out m_items, out m_amounts);

        for (int i = 0; i < m_items.Length; i++)
        {
            displayMaterial[i].ShowItem(m_items[i], m_amounts[i]);
        }
    }

    public void OpenCraftingUI(CraftTable craftTable)
    {
        selectedItem = null;
        m_craftTable = craftTable;
        gameObject.SetActive(true);

        HideCraftInfor();
        for (int i = 0; i < craftItemSlots.Length; i++)
        {
            craftItemSlots[i].Hide();
        }

        for (int i = 0; i < craftTable.ItemLength; i++)
        {
            craftItemSlots[i].ShowItem(craftTable.itemCanCraft[i]);
        }
    }

    void HideCraftInfor()
    {
        displaySlot.Hide();
        for (int i = 0; i < displayMaterial.Length; i++)
        {
            displayMaterial[i].Hide();
        }
    }
    void UpdateData()
    {
        for (int i = 0; i < displayMaterial.Length; i++)
        {
            displayMaterial[i].UpdateData();
        }
        for (int i = 0; i < craftItemSlots.Length; i++)
        {
            craftItemSlots[i].UpdateData();
        }
    }
    //BUTTON
    public void StartCraft()
    {
        Inventory playerInventory = GameManager.instance.inventory;

        if(playerInventory.HaveTheseItem(m_items, m_amounts))
        {
            for (int i = 0; i < m_items.Length; i++)
            {
                playerInventory.RemoveItem(m_items[i], m_amounts[i]);
            }

            GameManager.instance.AddItem(selectedItem);
            GetItemEvent.instance.NewItemEvent(selectedItem);
            UpdateData();
        }

    }
}
