using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetID : MonoBehaviour
{
    public GameObject[] objectToSetID;
    public string scpiptName;

    public int ID(Transform child)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i) == child)
            {
                return i;
            }
        }
        return -1;
    }
}
