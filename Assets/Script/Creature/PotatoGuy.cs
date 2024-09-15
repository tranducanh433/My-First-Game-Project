using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoGuy : MonoBehaviour
{
    GameObject selectBush;
    GameObject[] bush;

    void Start()
    {
        bush = GameObject.FindGameObjectsWithTag("Bush");
        if (bush.Length != 0)
        {
            selectBush = bush[Random.Range(0, bush.Length)];
        }
    }

    void Update()
    {
        if(selectBush == null)
        {
            bush = GameObject.FindGameObjectsWithTag("Bush");
            if(bush.Length != 0)
            {
                selectBush = bush[Random.Range(0, bush.Length)];
            }
        }

        if (selectBush == null) return;
        transform.position = Vector2.MoveTowards(transform.position, selectBush.transform.position, 5 * Time.deltaTime);
    }
}
