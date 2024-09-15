using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemGetContent : MonoBehaviour
{
    int amount = 1;
    public ItemBase currentItem;
    public Image itemImage;
    public TextMeshProUGUI text;
    [Header("Coin")]
    public Material defaultMaterial;
    public Sprite coinImage;

    public float speed;
    Vector2 m_target;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger("Get Item");
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_target, speed);
    }

    public void SetItem(ItemBase item, Vector2 target, float speed)
    {
        currentItem = item;

        itemImage.sprite = item.itemImage;
        text.text = item.itemName + " x " + amount;

        if (item is CoinItem)
        {
            itemImage.material = defaultMaterial;
            itemImage.sprite = coinImage;
        }

        this.speed = speed;
        m_target = target;
    }
    public void SetPosition(Vector2 pos)
    {
        m_target = pos;
    }
    public void AddAmount()
    {
        anim = GetComponent<Animator>();
        amount++;
        text.text = currentItem.itemName + " x " + amount;
        anim.SetTrigger("Get Item");
    }
}
