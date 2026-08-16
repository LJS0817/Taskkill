using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Deco : MonoBehaviour
{
    Image Img;
    float v;

    bool NotMove()
    {
        return (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A));
    }

    // Start is called before the first frame update
    void Start()
    {
        Img = GetComponent<Image>();
        v = Random.Range(0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!NotMove())
        {
            Img.fillAmount = Mathf.MoveTowards(Img.fillAmount, v, 0.006f);
            if (Img.fillAmount >= v)
            {
                v = Random.Range(0f, 1f);
            }
        }
        else
        {
            if (Img.fillAmount >= 0f)
            {
                Img.fillAmount = Mathf.MoveTowards(Img.fillAmount, 0f, 0.01f);
            }
        }
    }
}
