using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchNPC : MonoBehaviour
{
    public CraftTable craftTable;
    bool playerIsIn;

    void Update()
    {
        if(playerIsIn && Input.GetKeyDown(KeyCode.E))
        {
            UIManager.instance.OpenCraftUI(craftTable);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Immune"))
        {
            playerIsIn = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Immune"))
        {
            playerIsIn = false;
        }
    }
}
