using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarUI : MonoBehaviour
{
    public Slider slider;
    public Slider fillerEffect;
    public TextMeshProUGUI hpText;
    string maxValue;
    float currentValue;
    bool processing;
    
    public void SetMaxAndValue(int value)
    {
        slider.maxValue = value;
        slider.value = value;

        if(fillerEffect != null)
        {
            fillerEffect.maxValue = value;
            fillerEffect.value = value;
        }
        fillerEffect.maxValue = value;
        fillerEffect.value = value;
        currentValue = value;

        maxValue = value.ToString();

        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);

        if (hpText != null)
            hpText.text = value.ToString() + "/" + maxValue;
    }

    public void SetMaxValue(int value)
    {
        slider.maxValue = value;
        if (fillerEffect != null)
            fillerEffect.maxValue = value;

        maxValue = value.ToString();

        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);

        if (hpText != null)
            hpText.text = value.ToString() + "/" + maxValue;
    }
    public void SetValue(int value)
    {
        if (value != slider.value)
        {
            if (GetComponent<Animator>() != null)
                GetComponent<Animator>().SetTrigger("Zoom");
        }

        if(value > currentValue)
        {
            slider.value = value;
            if (fillerEffect != null)
                fillerEffect.value = value;
            currentValue = value;
        }
        else if(value < currentValue)
        {
            slider.value = value;
            currentValue = value;

            if (fillerEffect != null && processing == false)
                StartCoroutine(DecreaseCO());
        }

        if (hpText != null)
            hpText.text = value.ToString() + "/" + maxValue;

        //if(processing == false)

    }
    public void TextDisplay(string content)
    {
        hpText.text = content;
    }

    void ActivateEffect()
    {
        if (slider.value > currentValue)
            StartCoroutine(DecreaseCO());
        else if (slider.value < currentValue)
            StartCoroutine(IncreaseCO());
    }

    IEnumerator IncreaseCO(bool needToWait = true)
    {
        if(needToWait)
            yield return new WaitForSeconds(0.1f);

        processing = true;
        do
        {
            yield return null;
            slider.value += 0.02f;

            if (slider.value >= currentValue)
                slider.value = currentValue;
            else if(slider.value < currentValue)
            {
                StartCoroutine(DecreaseCO());
                processing = false;
                break;
            }
        }
        while (slider.value != currentValue);

        processing = false;
    }
    IEnumerator DecreaseCO(bool needToWait = true)
    {
        if (needToWait)
            yield return new WaitForSeconds(0.35f);

        processing = true;
        do
        {
            yield return null;
            fillerEffect.value -= fillerEffect.maxValue / 120f;

            if (fillerEffect.value <= currentValue)
                fillerEffect.value = currentValue;
            /*else if (fillerEffect.value > currentValue)
            {
                //StartCoroutine(IncreaseCO());
                processing = false;
                break;
            }*/
        }
        while (fillerEffect.value != currentValue);

        processing = false;
    }
}
