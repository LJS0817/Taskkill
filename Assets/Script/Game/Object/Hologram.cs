using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hologram : MonoBehaviour
{
    public int n;
    Animator Ani;
    // Start is called before the first frame update
    void Start()
    {
        Ani = GetComponent<Animator>();

        Ani.SetInteger("n", n);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
