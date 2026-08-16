using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auto_Hp : MonoBehaviour
{
    public AI Hp;
    // Update is called once per frame
    public void Sync_Hp()
    {
        if (Hp.Hp > 0f)
        {
            if (transform.tag == "Enemy_Head")
            {
                Hp.Hp -= 1f;
            }
            else
            {
                Hp.Hp -= 0.25f;
            }
        }
        else
        {
            Hp.Hp = 0f;
        }
    }
}
