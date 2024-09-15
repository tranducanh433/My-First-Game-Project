using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyExtension;

public class Minimap_Room : MonoBehaviour
{
    public bool discovered;
    public void Show()
    {
        Image roomImage = GetComponent<Image>();
        Image otherIcon = transform.GetChild(0).GetComponent<Image>();
        Image playerIcon = transform.GetChild(1).GetComponent<Image>();

        roomImage.color = roomImage.color.ShowColor();

        if(otherIcon.sprite != null)
            otherIcon.color = otherIcon.color.ShowColor();

        playerIcon.color = playerIcon.color.ShowColor();

        discovered = true;
    }

    public void Exit()
    {
        Image playerIcon = transform.GetChild(1).GetComponent<Image>();
        playerIcon.color = playerIcon.color.HideColor() ;
    }
}
