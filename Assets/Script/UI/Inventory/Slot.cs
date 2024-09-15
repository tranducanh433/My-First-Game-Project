using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using MyExtension;

public class Slot : MonoBehaviour, IPointerDownHandler
{
    public Image itemImage;
    public TextMeshProUGUI itemAmount;
    public bool canInteract = true;

    InventoryUI inventory;

    private void Start()
    {
        inventory = GetComponentInParent<InventoryUI>();
    }

    private SlotData m_slotData
    {
        get
        {
            int id = transform.ToSiblingIndex();
            return GameManager.instance.inventory.slotDatas[id];
        }
    }
    public void UpdateData()
    {
        if (m_slotData.item != null)
        {
            itemImage.sprite = m_slotData.itemImage;
            itemAmount.text = m_slotData.amount.ToString();

            if (m_slotData.itemImage != null)
            {
                itemImage.color = itemImage.color.ShowColor();
                if(m_slotData.item is WeaponItem)
                    itemAmount.color = itemAmount.color.HideColor();
                else
                    itemAmount.color = itemAmount.color.ShowColor();

            }
        }
        else
        {
            Clear();
        }
    }
   public void UpdateData(SlotData slot)
    {
        if (slot.item != null)
        {
            itemImage.sprite = slot.itemImage;
            itemAmount.text = slot.amount.ToString();

            if (slot.itemImage != null)
            {
                itemImage.color = itemImage.color.ShowColor();
                itemAmount.color = itemAmount.color.ShowColor();
            }
        }
        else
        {
            itemImage.color = itemImage.color.HideColor();
            itemAmount.color = itemAmount.color.HideColor();
        }
    }

    public void Clear()
    {
        m_slotData.Clear();

        itemImage.sprite = null;
        itemAmount.text = null;

        itemImage.color = itemImage.color.HideColor();
        itemAmount.color = itemAmount.color.HideColor();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canInteract)
        {
            if (inventory.itemDrag.item == null && m_slotData.item != null)
            {
                inventory.StartDrag(m_slotData);
                m_slotData.Clear();
                UpdateData();
            }
            else
            {
                ItemBase dragItem = inventory.itemDrag.item;

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

        if(m_slotData.OddAmount(dragItemAmount) == 0)
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
