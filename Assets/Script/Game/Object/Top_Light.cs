using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Top_Light : MonoBehaviour
{
    Renderer C;
    public bool Off;

    float T;
    float Limit_T;
    bool Change;
    Color now_EM_Color;
    Color now_Color;
    Color Light_Color;

    public bool Top;

    Light Lamp;
    // Start is called before the first frame update
    void Start()
    {
        C = GetComponent<Renderer>();
        Limit_T = 0.7f;
        T = Limit_T;
        Change = false;

        now_EM_Color = C.material.GetColor("_EmissionColor");
        now_Color = C.material.GetColor("_BaseColor");

        if (!Top)
        {
            Lamp = transform.GetChild(0).GetComponent<Light>();
            Light_Color = Lamp.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Off)
        {
            if(Limit_T <= 0f)
            {
                C.material.SetColor("_EmissionColor", now_EM_Color);
                C.material.SetColor("_BaseColor", now_Color);
                if (!Top)
                    Lamp.color = Light_Color;
                T = Limit_T = 0.7f;
                Off = false;
                Change = false;
            }
            else
            {
                T += Time.deltaTime;
                if (T >= Limit_T)
                {
                    T = 0f;
                    Limit_T -= 0.1f;
                    if (!Change)
                    {
                        C.material.SetColor("_EmissionColor", Color.black);
                        C.material.SetColor("_BaseColor", Color.black);
                        if(!Top)
                            Lamp.color = Color.black;
                    }
                    else
                    {
                        C.material.SetColor("_EmissionColor", now_EM_Color);
                        C.material.SetColor("_BaseColor", now_Color);
                        if (!Top)
                            Lamp.color = Light_Color;
                    }
                    Change = !Change;
                }
            }
        }
    }
}
