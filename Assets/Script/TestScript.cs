using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public LayerMask layer;
    public Transform player;
    void Update()
    {
        bool a = Physics2D.OverlapCircle(transform.position, 2, layer);
        Debug.Log(a);
    }
}
