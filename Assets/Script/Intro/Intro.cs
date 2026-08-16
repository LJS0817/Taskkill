using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public ParticleSystem P;
    public AudioSource A;
    public AudioSource A_0;
    public AudioSource A_1;
    public AudioSource A_2;

    public UnityEngine.UI.Image I;
    public UnityEngine.UI.Image Img;
    public UnityEngine.UI.Text T;
    public UnityEngine.UI.Image Panel;

    bool Done;
    // Start is called before the first frame update
    void Start()
    {
        Done = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (I.fillAmount >= 1f || Done)
        {
            if(!Done)
            {
                GetComponent<Animator>().enabled = false;
                Done = true;
            }
            Panel.color = new Color(Panel.color.r, Panel.color.g, Panel.color.b, Mathf.MoveTowards(Panel.color.a, 1f, 0.01f));
            if(Panel.color.a >= 1f)
            {
                Load_Scene();
            }
        }
        if (I.fillAmount< 1f)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                I.color = new Color(I.color.r, I.color.b, I.color.g, Mathf.MoveTowards(I.color.a, 1f, 0.03f));
                Img.color = new Color(Img.color.r, Img.color.b, Img.color.g, Mathf.MoveTowards(Img.color.a, 1f, 0.03f));
                T.color = new Color(T.color.r, T.color.b, T.color.g, Mathf.MoveTowards(T.color.a, 1f, 0.03f));
                I.fillAmount =  Mathf.MoveTowards(I.fillAmount, 1f, 0.008f);
            }
            else
            {
                I.color = new Color(I.color.r, I.color.b, I.color.g, Mathf.MoveTowards(I.color.a, 0f, 0.03f));
                Img.color = new Color(Img.color.r, Img.color.b, Img.color.g, Mathf.MoveTowards(Img.color.a, 0f, 0.03f));
                T.color = new Color(T.color.r, T.color.b, T.color.g, Mathf.MoveTowards(T.color.a, 0f, 0.03f));
                I.fillAmount = Mathf.MoveTowards(I.fillAmount, 0f, 0.008f);
            }
        }
    }

    void Spark()
    {
        P.Play();
        P.transform.GetChild(1).GetComponent<AudioSource>().Play();
    }

    void Boom(int n)
    {
        if (GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Break 0")
        {
            if (n == 0)
            {
                A.Play();
            }
            else 
            {
                A_1.Play();
                A_2.Play();
            }
        }
        else if (GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Break")
        {
            A_0.Play();
        }
    }

    void Load_Scene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LogoScene");
    }

    void isDone()
    {
        Done = true;
    }
}
