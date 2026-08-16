using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    public Transform Stick;
    public Transform Circle;
    public AudioSource Sound;

    public bool Alert;

    bool Change;

    Vector3 vec;
    // Start is called before the first frame update
    void Start()
    {
        vec = Circle.position;
        Change = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Alert)
        {
            Circle.position = new Vector3(vec.x, vec.y + Random.Range(-0.15f, 0.15f), vec.z + Random.Range(-0.15f, 0.15f));
            Quaternion v = Stick.rotation;
            if (Change)
            {
                v.x = Mathf.MoveTowards(Stick.rotation.x, 0.5f, 0.1f);
                if (v.x <= 0.5f)
                {
                    Change = false;
                }
            }
            else
            {
                v.x = Mathf.MoveTowards(v.x, 0.6f, 0.1f);
                if (v.x >= 0.6f)
                {
                    Change = true;
                }
            }
            Stick.rotation = v;
        }
    }
}
