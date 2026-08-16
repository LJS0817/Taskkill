using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public bool End;

    public TextMesh T;
    public Transform Pos;
    public Transform target;

    public Transform Box;

    public Text tex;

    public Image I;

    float time;

    bool Do;
    int Idx;
    readonly string s = "TASKKILL /F /IM BRAINWASHING.EXE";

    [HideInInspector]
    public int End_;
    public AudioSource A;

    public AudioSource[] Type_A;

    private void Start()
    {
        Do = true;
        End_ = 1;
        time = 0f;
        Idx = 0;
        Box.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Box.gameObject.activeInHierarchy)
        {
            if (Box.position.y > 24.4f)
            {
                if(!A.isPlaying)
                    A.Play();
                Vector3 v = Box.position;
                v.y -= 0.3f;
                Box.position = v;
            }
        }
        if (End)
        {
            if (Camera.main.transform.position != Pos.position)
            {
                Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, Pos.position, 0.5f);
            }

            Camera.main.transform.parent = target;
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.identity, 0.05f);

            Type();
        }
    }

    IEnumerator Typing(string str)
    {
        tex.text = "";
        foreach (char c in str)
        {
            tex.text += c;
            Type_A[Random.Range(0, Type_A.Length)].Play();
            yield return new WaitForSeconds(0.15f);
        }
    }

    IEnumerator Typing()
    {
        T.text = "";
        string str = "PROCESS SHUTDOWN";
        foreach (char c in str)
        {
            T.text += c;
            Type_A[Random.Range(0, Type_A.Length)].Play();
            yield return new WaitForSeconds(0.15f);
        }
        End_ = 2;
    }

    void Type()
    {
        if (End_ == 1)
        {
            if (Idx < s.Length)
            {
                time += Time.deltaTime;
                if (time >= 0.8f)
                {
                    time = 0f;
                    T.text = Do ? T.text.Insert(T.text.Length, "_") : T.text.Remove(T.text.Length - 1);
                    Do = !Do;
                }
                if (Input.anyKeyDown)
                {
                    Idx++;
                    Type_A[Random.Range(0, Type_A.Length)].Play();

                    T.text = s.Substring(0, Idx);
                    T.text += " ";
                }
            }
            else if (Idx == s.Length)
            {
                if (Input.GetKeyUp(KeyCode.Return))
                {
                    ++Idx;
                    Type_A[Random.Range(0, Type_A.Length)].Play();
                    StartCoroutine(Typing());
                    A.Stop();
                    time = 0f;
                }
            }
        }
        else if (End_ == 2)
        {
            if (Idx < s.Length + 3)
            {
                time += Time.deltaTime;
                if (time >= 0.5f)
                {
                    time = 0f;
                    if (T.text[T.text.Length - 3] == '.' && T.text[T.text.Length - 2] == '.' && T.text[T.text.Length - 1] == '.')
                    {
                        T.text = T.text.Replace(".", "");
                        Idx++;
                    }
                    else
                    {
                        T.text += ".";
                    }
                }
            }
            if (Idx == s.Length + 3)
            {
                I.color = new Color(1f, 1f, 1f, Mathf.MoveTowards(I.color.a, 1f, 0.01f));
                if (I.color.a >= 1f)
                {
                    StartCoroutine(Typing("GOOD JOB BUDDY"));
                    time = 0f;
                    End_ = 3;
                }
            }
        }
        else if (End_ ==  3)
        {
            if (tex.text == "GOOD JOB BUDDY")
            {
                time += Time.deltaTime;
                if (time >= 3f)
                {
                    StartCoroutine(Typing("THANK YOU FOR PLAYING"));
                    time = 0f;
                    End_ = 4;
                }
            }
        }
        else
        {
            if (tex.text == "THANK YOU FOR PLAYING")
            {
                time += Time.deltaTime;
                if (time >= 2f)
                {
                    I.color = new Color(Mathf.MoveTowards(I.color.r, 0f, 0.02f), Mathf.MoveTowards(I.color.g, 0f, 0.02f), Mathf.MoveTowards(I.color.b, 0f, 0.02f), 1f);
                    if (I.color.r <= 0f)
                    {
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                        UnityEngine.SceneManagement.SceneManager.LoadScene("LogoScene");
                    }
                }
            }
        }
    }
}
