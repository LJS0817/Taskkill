using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logo : MonoBehaviour
{
    public static bool Loading;
    public Image Go;
    bool Be_Start;

    Option O;
    Transform T;

    public Transform Sound;
    AudioSource[] A;

    int Idx;

    // Start is called before the first frame update
    void Start()
    {
        Idx = 0;
        O = GetComponent<Option>();
        Be_Start = false;
        Loading = false;
        T = null;

        A = new AudioSource[Sound.childCount];
        for(int i=0; i<A.Length; i++)
        {
            A[i] = Sound.GetChild(i).GetComponent<AudioSource>();
        }

        A[Idx].Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Be_Start)
        {
            if (!A[Idx].isPlaying)
            {
                if (Idx < A.Length - 1)
                {
                    ++Idx;
                    A[Idx].Play();
                }
                else
                {
                    Idx = 0;
                    A[Idx].Play();
                }
                Debug.Log(Idx);
            }
            if (!O.Quit.activeInHierarchy && !O.Credit.activeInHierarchy && !O.Setting_Image.activeInHierarchy)
            {
                Ray R = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit Hit;
                if (Physics.Raycast(R, out Hit))
                {
                    if (Hit.collider != null)
                    {
                        if (Hit.transform.tag == "Finish")
                        {
                            if (Input.GetMouseButtonDown(0))
                            {
                                T = Hit.transform;
                                Color c = new Color(87f / 255f, 87f / 255f, 87f / 255f);
                                T.GetComponent<Text_Sync>().Sync(true);
                                T.GetComponent<Renderer>().material.SetColor("_BaseColor", c);
                                T.GetChild(0).GetComponent<TextMesh>().color = Color.gray;
                            }
                            else if (!Input.GetMouseButtonUp(0) && !Input.GetMouseButton(0))
                            {
                                if (T == null)
                                {
                                    T = Hit.transform;
                                    T.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.white);
                                    T.GetChild(0).GetComponent<TextMesh>().color = Color.white;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (T != null)
                    {
                        Color c = new Color(171f / 255f, 171f / 255f, 171f / 255);
                        T.GetComponent<Renderer>().material.SetColor("_BaseColor", c);
                        T.GetChild(0).GetComponent<TextMesh>().color = new Color(191f / 255f, 191f / 255f, 191f / 255f);
                        T = null;
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    if (T != null)
                    {
                        T.GetComponent<Text_Sync>().Sync(false);
                        Color c = new Color(171f / 255f, 171f / 255f, 171f / 255);
                        T.GetComponent<Renderer>().material.SetColor("_BaseColor", c);
                        T.GetChild(0).GetComponent<TextMesh>().color = new Color(191f / 255f, 191f / 255f, 191f / 255f);
                        O.Player_Sound();
                        T = null;
                    }
                }
            }
        if (Loading)
        {
                if (Go.gameObject.activeInHierarchy)
                {
                    Go.color = new Color(Go.color.r, Go.color.g, Go.color.b, Mathf.MoveTowards(Go.color.a, 1f, 0.03f));
                    if (Go.color.a >= 1f)
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScene");
                    }
                }
                else
                {
                    Go.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            Go.color = new Color(Go.color.r, Go.color.g, Go.color.b, Mathf.MoveTowards(Go.color.a, 0f, 0.03f));
            if (Go.color.a <= 0f)
            {
                Go.gameObject.SetActive(false);
                Be_Start = true;
            }
        }
    }
}
