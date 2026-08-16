using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alpha_Sync : MonoBehaviour
{
    [HideInInspector]
    public Renderer R;

    float CTRL;

    // Start is called before the first frame update
    void OnEnable()
    {
        CTRL = 0f;
        R = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        CTRL += Time.deltaTime;
        if (CTRL >= 5f)
        {
            if (R.material.HasProperty("Vector1_7ECF6E17"))
            {
                if (R.material.GetFloat("Vector1_7ECF6E17") >= 1f)
                {
                    Destroy(gameObject);
                }
                else
                {
                    R.material.SetFloat("Vector1_7ECF6E17", R.material.GetFloat("Vector1_7ECF6E17") + Time.deltaTime * 0.5f);
                }
            }
            else
            {
                Color c = R.material.GetColor("_BaseColor");
                c.a = Mathf.MoveTowards(c.a, 0f, 0.01f);
                R.material.SetColor("_BaseColor", c);
                if(c.a <= 0f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
