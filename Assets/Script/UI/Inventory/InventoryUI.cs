using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Slot[] slots;
    public EquipmentSlot[] equipmentSlots;
    public TextMeshProUGUI coinText;
    public WeaponContainer weaponContainer;
    public ConsummingItemDisplay consummingItemDisplay;

    [Header("Other")]
    public int column = 5;
    public int row = 4;

    [HideInInspector] public DragItem dragItemUI;
    [HideInInspector] public SlotData itemDrag;

    GameManager GM;

    void Start()
    {
        if(GM == null)
            GM = GameManager.instance;

        UpdateDisplay();
        UpdateInventoryData();
        UpdateEquipmentSlot();
    }

    public void UpdateInventoryData()
    {
        if (GM == null)
            GM = GameManager.instance;

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].UpdateData();
        }
    }
    public void UpdateEquipmentSlot()
    {
        if (GM == null)
            GM = GameManager.instance;

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].UpdateData();
        }

        UpdateDisplay();
    }
    public void UpdateStatusData()
    {
        if (GM == null)
            GM = GameManager.instance;

        coinText.text = GM.coin.ToString();
    }

    public void StartDrag(SlotData itemToDrag)
    {
        itemDrag.SetNewItem(itemToDrag);
        dragItemUI.SetDragItem(itemToDrag);

        UpdateDisplay();
    }
    public void SetDragItemAmount(int value)
    {
        dragItemUI.SetAmount(value);
        itemDrag.amount = value;

        UpdateDisplay();
    }
    public void ClearDragItem()
    {
        dragItemUI.Clear();
        itemDrag.Clear();

        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        weaponContainer.UpdateData();
        consummingItemDisplay.UpdateData();
    }

    // Open and close inventory
    public void OpenInventory()
    {
        gameObject.SetActive(true);
        StartCoroutine(SlotAppearCO());
    }

    IEnumerator SlotAppearCO()
    {
        List<Animator> slotAnims = new List<Animator>();

        for (int i = 0; i < slots.Length; i++)
        {
            slotAnims.Add(slots[i].GetComponent<Animator>());
        }

        slotAnims[0].SetBool("Appear", true);

        int numberOfSlotWillAppear = 2;
        int slotIndex;

        for (int i = 1; i < column; i++)
        {
            yield return new WaitForSeconds(0.025f);
            slotIndex = i;

            for (int j = 0; j < numberOfSlotWillAppear; j++)
            {
                slotAnims[slotIndex].SetBool("Appear", true);
                slotIndex += (column - 1);
            }

            if (numberOfSlotWillAppear < row)
                numberOfSlotWillAppear++;
        }


        for (int i = 1; i < row; i++)
        {
            yield return new WaitForSeconds(0.025f);
            slotIndex = (column - 1) + column * i;
            numberOfSlotWillAppear = row - i;

            for (int j = 0; j < numberOfSlotWillAppear; j++)
            {
                slotAnims[slotIndex].SetBool("Appear", true);
                slotIndex += (column - 1);
            }
        }
    }

    public void CloseInventory()
    {
        StartCoroutine(SlotDisappearCO());
    }
    IEnumerator SlotDisappearCO()
    {
        List<Animator> slotAnims = new List<Animator>();

        for (int i = 0; i < slots.Length; i++)
        {
            slotAnims.Add(slots[i].GetComponent<Animator>());
        }

        slotAnims[0].SetBool("Appear", false);

        int numberOfSlotWillAppear = 2;
        int slotIndex;

        for (int i = 1; i < column; i++)
        {
            yield return new WaitForSeconds(0.025f);
            slotIndex = i;

            for (int j = 0; j < numberOfSlotWillAppear; j++)
            {
                slotAnims[slotIndex].SetBool("Appear", false);
                slotIndex += (column - 1);
            }

            if (numberOfSlotWillAppear < row)
                numberOfSlotWillAppear++;
        }


        for (int i = 1; i < row; i++)
        {
            yield return new WaitForSeconds(0.025f);
            slotIndex = (column - 1) + column * i;
            numberOfSlotWillAppear = row - i;

            for (int j = 0; j < numberOfSlotWillAppear; j++)
            {
                slotAnims[slotIndex].SetBool("Appear", false);
                slotIndex += (column - 1);
            }
        }
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
