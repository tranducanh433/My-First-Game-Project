using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG : MonoBehaviour
{
    public static int DamageCaculate(int damage, int crt, int crtd)
    {
        int r = Random.Range(1, 101);
        
        if(r <= crt)
        {
            damage = (int)((float)damage * ((float)crtd / 100f));
        }

        return damage;
    }
}
