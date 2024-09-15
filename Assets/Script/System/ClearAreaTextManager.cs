using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClearAreaTextManager : MonoBehaviour
{
    public TextMeshProUGUI tmpro;

    public void ShowAreaClearText()
    {
        gameObject.SetActive(true);
    }
}
