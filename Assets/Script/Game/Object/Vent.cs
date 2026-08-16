using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    Animator Ani;
    // Start is called before the first frame update
    void Start()
    {
        Ani = GetComponent<Animator>();
    } 

    public void Add()
    {
        gameObject.AddComponent<Alpha_Sync>();
        transform.GetChild(0).gameObject.AddComponent<Alpha_Sync>();
    }
}
