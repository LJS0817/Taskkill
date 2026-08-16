using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead_AI : MonoBehaviour
{
    public Transform T;
    public Renderer C;
    Alpha_Sync As;

    private void Start()
    {
        As = GetComponent<Alpha_Sync>();
        transform.position = T.position;
        As.R = GetComponent<Renderer>();
        As.R.material.SetColor("Color_37E1C2E", C.material.GetColor("_BaseColor"));
        gameObject.AddComponent<Rigidbody>();
    }
}
