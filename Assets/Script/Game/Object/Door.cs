using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator Ani;
    float T;
    public bool Locked;
    public Renderer Locked_Color;
    public Renderer other_Locked_Color;
    public bool Up;
    public TextMesh Stage;
    public Player_Map map;
    public bool Need_Key;
    public AudioSource A;
    bool Key;
    // Start is called before the first frame update
    void Start()
    {
        Key = Need_Key;
        T = 0f;
        Ani = GetComponent<Animator>();
        if (!Up)
        {
            if (Locked || Need_Key)
            {
                Locked_Color.material.SetColor("_EmissionColor", Color.red);
                other_Locked_Color.material.SetColor("_EmissionColor", Color.red);
            }
        }
        else
        {
            map = GameObject.FindGameObjectWithTag("Finish").GetComponent<Player_Map>();
            A.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Up)
        {
            Stage.text = (map.Stage + 1).ToString();
        }
        else
        {
            if (!Locked && !Need_Key)
            {
                if (Locked_Color.material.GetColor("_EmissionColor") == Color.red)
                {
                    Locked_Color.material.SetColor("_EmissionColor", Color.green);
                    other_Locked_Color.material.SetColor("_EmissionColor", Color.green);
                }
            }
            if (Ani.GetBool("Open"))
            {
                T += Time.deltaTime;
                if (T >= 7f)
                {
                    T = 0f;
                    Ani.SetBool("Open", false);
                    A.Play();
                    if (Key)
                    {
                        Need_Key = true;
                        Locked_Color.material.SetColor("_EmissionColor", Color.red);
                        other_Locked_Color.material.SetColor("_EmissionColor", Color.red);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Up)
        {
            if (!Locked && !Need_Key)
            {
                if (other.gameObject.layer == 14)
                {
                    if (!Ani.GetBool("Open"))
                    {
                        Ani.SetBool("Open", true);
                        A.Play();
                        T = 0f;
                    }
                }
            }
        }
    }
}
