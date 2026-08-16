using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [HideInInspector]
    public Vector3 Target_Pos;
    [HideInInspector]
    public Vector3 Rot;
    [HideInInspector]
    public bool Done = true;

    float Rand;

    // Update is called once per frame
    void Update()
    {
        if (!Done)
        {
            if (transform.position != Target_Pos)
            {
                //Debug.Log(Rot);
                transform.position = Vector3.MoveTowards(transform.position, Target_Pos, 4f);
                //transform.localRotation = new Quaternion(transform.localRotation.x, transform.localRotation.y, -0.5f, transform.localRotation.w);
            }
            else
            {
                Destroy(gameObject, 7f);
                Done = true;
            }
        }
    }
}
