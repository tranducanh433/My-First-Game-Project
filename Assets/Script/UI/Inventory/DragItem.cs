using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragItem : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI itemAmount;
    public Vector2 size;

    private void Start()
    {
        size = GetComponent<RectTransform>().sizeDelta;
    }

    private void Update()
    {
        FollowMouse();
    }

    public void SetDragItem(SlotData slotData)
    {
        FollowMouse();
        gameObject.SetActive(true);
        itemImage.sprite = slotData.itemImage;
        itemAmount.text = slotData.amount.ToString();
    }
    public void SetAmount(int amount)
    {
        itemAmount.text = amount.ToString();
    }
    public void Clear()
    {
        gameObject.SetActive(false);
    }

    void FollowMouse()
    {
        Vector2 mousePos = Input.mousePosition;
        transform.position = new Vector2(mousePos.x + (size.x / 2) + 5,
                                        mousePos.y - (size.y / 2) + 5);
    }
}
