using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chart : MonoBehaviour
{
    public Image[] I;
    float[] Limit;

    public Transform[] Map;
    // Start is called before the first frame update
    void Start()
    {
        Limit = new float[I.Length];
        for(int i=0; i< I.Length; i++)
        {
            Limit[i] = Random.Range(0f, 0.9f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < I.Length; i++)
        {
            I[i].fillAmount = Mathf.MoveTowards(I[i].fillAmount, Limit[i], 0.005f);
            if (I[i].fillAmount >= Limit[i])
            {
                Limit[i] = Random.Range(0f, 0.9f);
            }
        }

        for(int i=0; i<Map.Length; i++)
        {
            Vector3 v = Map[i].eulerAngles;
            v.y += 1;
            Map[i].eulerAngles = v;
        }
    }
}
