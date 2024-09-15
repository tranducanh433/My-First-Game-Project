using JetBrains.Annotations;
using MyExtension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Image takeDamageImage;
    public Image healImage;
    public GameObject gameOverPanel;
    public GameObject mainUI;
    public InventoryUI inventoryUI;
    public GameObject passiveSkillUI;
    public CraftingUI craftingUI;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Update()
    {
        InventoryUIManager();
    }

    void InventoryUIManager()
    {
        /*if (Input.GetKeyDown(KeyCode.I))
        {
            if (mainUI.activeSelf == false)
            {
                OpenMainUI();
                OpenInventoryUI();
            }
            else
            {
                CloseMainUI();
                inventoryUI.CloseInventory();
            }

            GameObject.Find("Player").GetComponent<Player>().UpdatePlayerData();
        }*/
    }

    void OpenMainUI()
    {
        mainUI.SetActive(true);
    }
    void CloseMainUI()
    {
        StartCoroutine(CloseMainUICO());
    }
    IEnumerator CloseMainUICO()
    {
        yield return new WaitForSeconds(0.6f);
        mainUI.SetActive(false);
    }

    public void TakeDamageEffect()
    {
        StartCoroutine(TakeDamageEffectCo());
    }
    private IEnumerator TakeDamageEffectCo()
    {
        float a = 0;

        for (int i = 0; i < 15; i++)
        {
            yield return null;
            a += 0.01f;
            takeDamageImage.color = new Color(1, 1, 1, a);
        }
        for (int i = 0; i < 15; i++)
        {
            yield return null;
            a -= 0.01f;
            takeDamageImage.color = new Color(1, 1, 1, a);
        }
    }

    public void HealEffect()
    {
        StopCoroutine(HealEffectCo());
        StartCoroutine(HealEffectCo());
    }

    private IEnumerator HealEffectCo()
    {
        float a = 0;

        do
        {
            yield return null;
            a += 0.01f;
            healImage.color = new Color(1, 1, 1, a);
        }
        while (a < 0.2);
        do
        {
            yield return null;
            a -= 0.01f;
            healImage.color = new Color(1, 1, 1, a);
        }
        while (a > 0);
    }

    public IEnumerator OpenGameOverPanel()
    {
        yield return new WaitForSeconds(0.35f);
        gameOverPanel.SetActive(true);
        GameManager.instance.PauseGame();
    }

    public void OpenCraftUI(CraftTable craftTable)
    {
        if (craftingUI.gameObject.activeSelf == false)
            craftingUI.OpenCraftingUI(craftTable);
        else
            craftingUI.gameObject.SetActive(false);
    }

    //BUTTON
    public void OpenInventoryUI()
    {
        inventoryUI.OpenInventory();
        passiveSkillUI.SetActive(false);
    }
    public void OpenPassiveSkillUI()
    {
        inventoryUI.CloseInventory();
        passiveSkillUI.SetActive(true);
    }
}
