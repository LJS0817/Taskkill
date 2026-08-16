using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class VALUE
{
    public static int Number = 100;
}

public class Result : MonoBehaviour
{
    AudioSource Audio;
    public ParticleSystem P;
    public Image I;
    public Text Num;
    int On;
    float T_T;

    public Transform Sound;
    AudioSource[] A;

    int Idx;
    // Start is called before the first frame update
    void Start()
    {
        T_T = 0f;
        Audio = GetComponent<AudioSource>();
        On = 0;
        StartCoroutine(T());
        Num.text = "No." + VALUE.Number.ToString();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Idx = 0;

        A = new AudioSource[Sound.childCount];
        for (int i = 0; i < A.Length; i++)
        {
            A[i] = Sound.GetChild(i).GetComponent<AudioSource>();
        }

        A[Idx].Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!A[Idx].isPlaying)
        {
            if (Idx >= A.Length)
            {
                Idx = 0;
                A[Idx].Play();
            }
            else
            {
                ++Idx;
                A[Idx].Play();
            }
        }
        T_T += Time.deltaTime;
        if(T_T >= 4f)
        {
            T_T = 0f;
            P.Play();
            P.transform.GetChild(1).GetComponent<AudioSource>().Play();
        }
        if (On == 1)
        {
            I.color = new Color(I.color.r, I.color.g, I.color.b, Mathf.MoveTowards(I.color.a, 0f, 0.02f));
            if(I.color.a <= 0f)
            {
                On = 0;
                I.gameObject.SetActive(false);
            }
        }
        else if (On > 1)
        {
            I.color = new Color(I.color.r, I.color.g, I.color.b, Mathf.MoveTowards(I.color.a, 1f, 0.02f));
            if (I.color.a >= 1f)
            {
                if(On == 2)
                {
                    SceneManager.LoadScene("LoadingScene");
                }
                else
                {
                    SceneManager.LoadScene("LogoScene");
                }
            }
        }
    }

    public void Click_UI(bool Yes)
    {
        I.gameObject.SetActive(true);
        Audio.Play();
        if (Yes)
        {
            On = 2;
        }
        else
        {
            On = 3;
        }
    }

    IEnumerator T()
    {
        yield return new WaitForSeconds(0.5f);
        On = 1;
    }
}
