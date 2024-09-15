using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftItemSlot : MonoBehaviour, IPointerClickHandler
{
    public Image itemImage;
    public TextMeshProUGUI amountText;
    public GameObject darkBackground;
    ItemBase m_item;
    int m_amount;

    public void ShowItem(ItemBase item, int amount = 0)
    {
        Inventory playerInventory = GameManager.instance.inventory;
        gameObject.SetActive(true);

        m_item = item;
        m_amount = amount;
        itemImage.sprite = item.itemImage;

        if(amount > 0)
        {
            amountText.text = amount.ToString();
            if (playerInventory.NumberOfItem(item) < amount)
                amountText.color = Color.red;
            else
                amountText.color = Color.white;
        }
        else
        {
            amountText.color = Color.clear;

            if (playerInventory.HaveEnoughMaterialToCreateThisItem(m_item))
            {
                darkBackground.SetActive(false);
            }
            else
            {
                darkBackground.SetActive(true);
            }
        }
    }

    public void UpdateData()
    {
        if(m_item != null)
        {
            Inventory playerInventory = GameManager.instance.inventory;

            itemImage.sprite = m_item.itemImage;

            if (m_amount > 0)
            {
                amountText.text = m_amount.ToString();
                if (playerInventory.NumberOfItem(m_item) < m_amount)
                    amountText.color = Color.red;
                else
                    amountText.color = Color.white;
            }
            else
            {
                amountText.color = Color.clear;

                if (playerInventory.HaveEnoughMaterialToCreateThisItem(m_item))
                {
                    darkBackground.SetActive(false);
                }
                else
                {
                    darkBackground.SetActive(true);
                }
            }
        }
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(m_item.materialLength > 0)
        {
            CraftingUI.instance.ShowMaterialNeed(m_item);
        }
            
    }
}
