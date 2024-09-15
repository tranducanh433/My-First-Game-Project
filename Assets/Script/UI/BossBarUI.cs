using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBarUI : MonoBehaviour
{
    public static BossBarUI instance;

    public Slider slider;
    public Transform bossLifeContainer;

    int numberOfLife;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameObject.SetActive(false);
    }

    public void SetStartValue(int value, int numberOfPhase)
    {
        slider.maxValue = value;
        slider.value = value;

        numberOfLife = numberOfPhase;

        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);

        for (int i = 0; i < numberOfPhase - 1; i++)
        {
            bossLifeContainer.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void RemoveOneLife()
    {
        numberOfLife--;
        bossLifeContainer.GetChild(numberOfLife - 1).gameObject.SetActive(false);
    }
    public void SetMaxValue(int value)
    {
        slider.maxValue = value;
        slider.value = value;
    }
    public void SetValue(int value)
    {
        slider.value = value;
    }

    public void HideBossBar()
    {
        gameObject.SetActive(false);
    }
}
