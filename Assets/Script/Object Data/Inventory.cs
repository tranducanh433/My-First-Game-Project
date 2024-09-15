[System.Serializable]
public class Inventory
{
    public SlotData[] slotDatas;

    public int slotLength => slotDatas.Length;

    public int NumberOfItem(ItemBase item)
    {
        int amount = 0;

        for (int i = 0; i < slotDatas.Length; i++)
        {
            if(slotDatas[i].item == item)
            {
                amount += slotDatas[i].amount;
            }
        }
        return amount;
    }
    public void AddItem(ItemBase item)
    {
        for (int i = 0; i < slotLength; i++)
        {
            SlotData slotData = slotDatas[i];


            if (slotData.item == item && slotData.IsFull() == false)
            {
                slotData.AddAmount(1);
                break;
            }
            else if (slotData.item == null)
            {
                slotData.SetNewItem(item);
                break;
            }
        }
    }
    public void RemoveItem(ItemBase itemToRemove, int removeAmount)
    {
        int amountLeft = removeAmount;

        for (int i = 0; i < slotDatas.Length; i++)
        {
            if(slotDatas[i].item == itemToRemove)
            {
                if(slotDatas[i].amount >= amountLeft)
                {
                    slotDatas[i].amount -= removeAmount;
                    break;
                }
                else //If(slotDatas[i].amount < removeAmount)
                {
                    amountLeft -= slotDatas[i].amount;
                    slotDatas[i].Clear();
                }
            }
        }
    }
    public bool IsFull(ItemBase item)
    {
        for (int i = 0; i < slotLength; i++)
        {
            SlotData slotData = slotDatas[i];

            if (slotData.item == null)
            {
                return false;
            }
            else if (slotData.item == item && slotData.IsFull() == false)
            {
                return false;
            }
        }

        return true;
    }

    public bool HaveTheseItem(ItemBase[] items, int[] amounts)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if(NumberOfItem(items[i]) < amounts[i])
            {
                return false;
            }
        }

        return true;
    }
    public bool HaveEnoughMaterialToCreateThisItem(ItemBase itemToCraft)
    {
        ItemBase[] items = itemToCraft.GetMaterialArray();
        int[] amounts = itemToCraft.GetMaterialAmountArray();

        for (int i = 0; i < items.Length; i++)
        {
            if (NumberOfItem(items[i]) < amounts[i])
            {
                return false;
            }
        }

        return true;
    }

}
