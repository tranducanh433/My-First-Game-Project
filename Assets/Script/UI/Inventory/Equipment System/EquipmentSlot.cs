using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using MyExtension;

public class EquipmentSlot : MonoBehaviour, IPointerDownHandler
{
    public enum SlotType { Weapon, ConsummingItem, SpellCard}
    public SlotType slotType;
    public Sprite slotIcon;
    public Image itemImage;
    public TextMeshProUGUI itemAmount;

    InventoryUI inventory;
    private SlotData m_slotData
    {
        get
        {
            int id = transform.ToSiblingIndex();
            if (slotType == SlotType.Weapon)
                return GameManager.instance.equippedWeapons[id];
            else if (slotType == SlotType.SpellCard)
                return GameManager.instance.equippedSpellCards[id];
            else /*if (slotType == SlotType.ConsummingItem)*/
                return GameManager.instance.consummingItem;
        }
    }

    private void Start()
    {
        inventory = GetComponentInParent<InventoryUI>();
    }

    public void UpdateData()
    {
        if (m_slotData.item != null)
        {
            itemImage.sprite = m_slotData.itemImage;
            itemAmount.text = m_slotData.amount.ToString();

            if (m_slotData.itemImage != null)
            {
                itemImage.color = Color.white;

                if(m_slotData.item is ConsummingItem)
                    itemAmount.color = Color.white;
            }
        }
        else
        {
            Clear();
        }
    }
    public void Clear()
    {
        m_slotData.Clear();

        itemImage.sprite = slotIcon;
        itemAmount.text = null;

        itemImage.color = new Color(0, 0, 0, 0.5f);
        itemAmount.color = itemAmount.color.HideColor();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ItemBase dragItem = inventory.itemDrag.item;
        if (dragItem == null && m_slotData.item != null)
        {
            inventory.StartDrag(m_slotData);
            m_slotData.Clear();
            UpdateData();
        }
        else if (SameType(dragItem))
        {

            if (m_slotData.item == null)
            {
                SetNewItem(inventory.itemDrag);
            }
            else if (m_slotData.item == dragItem)
            {
                AddAmount(inventory.itemDrag);
            }
            else if (m_slotData.item != dragItem)
            {
                SwitchItem(inventory.itemDrag);
            }
        }
    }

    public bool SameType(ItemBase itemToPutIn)
    {
        if (itemToPutIn is WeaponItem && slotType == SlotType.Weapon)
            return true;
        else if (itemToPutIn is ConsummingItem && slotType == SlotType.ConsummingItem)
            return true;
        else if (itemToPutIn is SpellCard && slotType == SlotType.SpellCard)
            return true;
        else
            return false;
    }

    void SetNewItem(SlotData newItem)
    {
        m_slotData.SetNewItem(newItem);
        inventory.ClearDragItem();
        UpdateData();
    }
    void AddAmount(SlotData itemToAdd)
    {
        int dragItemAmount = itemToAdd.amount;

        if (m_slotData.OddAmount(dragItemAmount) == 0)
        {
            inventory.ClearDragItem();
        }
        else
        {
            int oddAmount = m_slotData.OddAmount(dragItemAmount);
            inventory.SetDragItemAmount(oddAmount);
        }

        m_slotData.AddAmount(dragItemAmount);
        UpdateData();
    }
    void SwitchItem(SlotData itemToASwitch)
    {
        m_slotData.SwitchWith(itemToASwitch);
        inventory.StartDrag(itemToASwitch);
        UpdateData();
    }
}
