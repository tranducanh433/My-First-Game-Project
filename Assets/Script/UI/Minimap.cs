using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Minimap_Room[] rooms;
    public Transform roomsContainer;

    public void SetPlayerPosition(int id)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if(i == id)
            {
                rooms[i].Show();
                if(GetComponent<Chasing>() == null)
                    gameObject.AddComponent<Chasing>();
                GetComponent<Chasing>().Move(roomsContainer, transform.position, rooms[i].transform.position, 5, false);
            }
            else
            {
                rooms[i].Exit();
            }
        }

    }
}
