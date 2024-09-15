using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCallBack : MonoBehaviour
{
    System.Action m_CallBack;

    public void SetCallBack(System.Action callback)
    {
        m_CallBack = callback;
    }

    public void Activate_CallBack()
    {
        m_CallBack();
    }
}
