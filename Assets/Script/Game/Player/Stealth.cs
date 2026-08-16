using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stealth : MonoBehaviour
{
    public bool Kill;

    public Transform Enemy_Pos;
    Animator Ani;

    public ParticleSystem P;

    private void Start()
    {
        Ani = GetComponent<Animator>();
    }

    public void Stealth_Kill(int n)
    {
        Kill = true;
        Ani.SetInteger("Stealth", n);
    }
}
