using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OrderInLayerController : MonoBehaviour
{
    [Header("Main Body")]
    public int addLayer;

    [Header("Part Of Body")]
    public SpriteRendererBody[] partOfBody;
    float yPos;
    int layer;
    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();    
    }

    void Update()
    {
        MainBody();
        PartOfBody();
    }

    void MainBody()
    {
        yPos = - transform.position.y;
        layer = (int)(yPos * 100);
        sr.sortingOrder = layer + addLayer;
    }

    void PartOfBody()
    {
        for (int i = 0; i < partOfBody.Length; i++)
        {
            partOfBody[i].spriteRenderer.sortingOrder = layer + partOfBody[i].addLayer;
        }
    }
}


[System.Serializable]
public struct SpriteRendererBody
{
    public SpriteRenderer spriteRenderer;
    public int addLayer;
}
