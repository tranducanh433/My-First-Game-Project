using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponContainer : MonoBehaviour
{
    public Image[] slots;

    WeaponItem[] equippedWeapons = new WeaponItem[3];

    GameManager GM;

    private void Start()
    {
        GM = GameManager.instance;
        ItemSelect();
        UpdateData();
    }

    private void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            NextWeapon();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            PreviousWeapon();

        }

        ItemSelect();
    }

    private void NextWeapon()
    {
            int loopTime = 0;
            do
            {
                loopTime++;

                SwitchUpWeapon();
            }
            while (equippedWeapons[0] == null && loopTime < 5);
    }
    private void PreviousWeapon()
    {
            int loopTime = 0;
            do
            {
                loopTime++;

                SwitchDownWeapon();
            }
            while (equippedWeapons[0] == null && loopTime < 5);
    }
    private int GetWeaponLength()
    {
        int numberOfEquippedWeapon = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            if (equippedWeapons != null)
                numberOfEquippedWeapon++;
        }

        return numberOfEquippedWeapon;
    }
    private int GetElementThatHaveWeapon()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (equippedWeapons != null)
                return i;
        }

        return 0;
    }

    void SwitchUpWeapon()
    {
        WeaponItem weapon = equippedWeapons[2];
        equippedWeapons[2] = equippedWeapons[1];
        equippedWeapons[1] = equippedWeapons[0];
        equippedWeapons[0] = weapon;
    }
    void SwitchDownWeapon()
    {
        WeaponItem weapon = equippedWeapons[0];
        equippedWeapons[0] = equippedWeapons[1];
        equippedWeapons[1] = equippedWeapons[2];
        equippedWeapons[2] = weapon;
    }

    public void ItemSelect()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(equippedWeapons[i] != null)
            {
                slots[i].sprite = equippedWeapons[i].itemImage;
                slots[i].color = Color.white;
            }
            else
            {
                slots[i].color = Color.clear;
            }
        }

        GM.SetSelectedWeapon(equippedWeapons[0]);
        GameObject.Find("Player").GetComponent<Player>().UpdatePlayerData();
    }

    public void UpdateData()
    {
        for (int i = 0; i < equippedWeapons.Length; i++)
        {
            equippedWeapons[i] = GM.equippedWeapons[i].item as WeaponItem;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].sprite = equippedWeapons[i]?.itemImage;
        }
        GameObject.Find("Player").GetComponent<Player>().UpdatePlayerData();
    }
}
