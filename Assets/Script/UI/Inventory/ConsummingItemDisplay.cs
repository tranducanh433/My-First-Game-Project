using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MyExtension;

public class ConsummingItemDisplay : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI numberOfItemText;
    public Image CDImage;

    Image backGroundImage;
    private readonly float TIME_NEED_TO_WAIT = 20;
    float timeLeft;

    GameManager GM;

    private void Start()
    {
        GM = GameManager.instance;
        backGroundImage = GetComponent<Image>();

        UpdateData();
    }

    private void Update()
    {
        PlayerInput();
        Cooldown();
    }

    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && timeLeft <= 0 && GM.consummingItem.amount > 0)
        {
            UseItem();
            timeLeft = TIME_NEED_TO_WAIT;
        }
    }
    void UseItem()
    {
        ConsummingItem consummingItem = GM.consummingItem.item as ConsummingItem;

        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.GainHP(consummingItem.valueGain);

        GM.consummingItem.amount--;
        UpdateData();
    }
    void Cooldown()
    {
        if(timeLeft > 0)
        {
            CDImage.fillAmount = Mathf.InverseLerp(0f, TIME_NEED_TO_WAIT, timeLeft);

            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0)
                timeLeft = 0;
        }
    }

    public void UpdateData()
    {
        ConsummingItem consummingItem = GM.consummingItem.item as ConsummingItem;
        string numberOfItem = GM.consummingItem.amount.ToString();

        if(consummingItem != null)
        {
            itemImage.sprite = consummingItem.itemImage;
            numberOfItemText.text = numberOfItem;

            itemImage.color = itemImage.color.ShowColor();
            numberOfItemText.color = numberOfItemText.color.ShowColor();
            backGroundImage.color = numberOfItemText.color.ShowColor();
        }
        else
        {
            itemImage.color = itemImage.color.HideColor();
            numberOfItemText.color = numberOfItemText.color.HideColor();
            backGroundImage.color = numberOfItemText.color.HideColor();
        }
    }
}
