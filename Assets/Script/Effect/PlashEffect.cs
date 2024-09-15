using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlashEffectType
{
    ZoomIn, ZoomOut
}
public class PlashEffect : MonoBehaviour
{
    float m_endScale;
    float m_currentScale;
    PlashEffectType m_plashEffectType;
    float m_speed;

    void Update()
    {
        if(m_plashEffectType == PlashEffectType.ZoomIn)
        {
            m_currentScale -= m_speed * Time.deltaTime;
            transform.localScale = new Vector3(m_currentScale, m_currentScale, 1);

            if (m_currentScale <= 0)
            {
                Destroy(gameObject);
            }
        }
        if (m_plashEffectType == PlashEffectType.ZoomOut)
        {
            m_currentScale += m_speed * Time.deltaTime;
            transform.localScale = new Vector3(m_currentScale, m_currentScale, 1);

            if (m_currentScale >= m_endScale)
            {
                float a = GetComponent<SpriteRenderer>().color.a - 2 * Time.deltaTime;
                Color currentColor = GetComponent<SpriteRenderer>().color;
                GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, a);

                if (a <= 0)
                    Destroy(gameObject);
            }
        }
    }

    public void SetZoomInData(float startScale, float speed, Color color)
    {
        transform.localScale = new Vector3(startScale, startScale, 1);
        m_currentScale = startScale;
        m_plashEffectType = PlashEffectType.ZoomIn;
        m_speed = speed;

        GetComponent<SpriteRenderer>().color = color;
    }
    public void SetZoomOutData(float endScale, float speed, Color color)
    {
        transform.localScale = new Vector3(0, 0, 1);
        m_currentScale = 0;
        m_endScale = endScale;
        m_plashEffectType = PlashEffectType.ZoomOut;
        m_speed = speed;

        GetComponent<SpriteRenderer>().color = color;
    }
}
