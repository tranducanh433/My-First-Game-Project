using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassiveSkillSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image skillImage;
    public Image filler;
    public PassiveSkillSlot[] connectedSkill;
    bool isHolding;

    PassiveSkillBase m_passiveSkill;
    PassiveSkillController passiveSkillController;
    GameManager GM;

    private void Start()
    {
        GM = GameManager.instance;
        passiveSkillController = transform.GetComponentInParent<PassiveSkillController>();
    }

    private void Update()
    {
        if (isHolding)
        {
            filler.fillAmount += Time.deltaTime;

            if(filler.fillAmount >= 1)
            {
                GM.skillPoint--;
                filler.fillAmount = 0;
                passiveSkillController.UpdateData();
                isHolding = false;
            }
        }
    }
    public void UpdateData(PassiveSkillBase passiveSkillInfor)
    {
        m_passiveSkill = passiveSkillInfor;

        skillImage.sprite = passiveSkillInfor.skillImage;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GM.skillPoint > 0)
            isHolding = true;

        StopAllCoroutines();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        StartCoroutine(CancelUpgradeCO());
    }

    IEnumerator CancelUpgradeCO()
    {
        yield return new WaitForSeconds(0.5f);
        do
        {
            filler.fillAmount -= Time.deltaTime;
            yield return null;
        }
        while (filler.fillAmount > 0);

        filler.fillAmount = 0;
    }
}
