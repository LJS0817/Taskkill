using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    public bool Just_Look;
    public int Invisable;
    [HideInInspector]
    public bool Off;
    public Transform Light_Offset;
    public ParticleSystem Spark;
    public ParticleSystem Smoke;
    public Transform Head;
    public Transform Light;
    public Transform off_Rot;
    Vector3[] EndPoint;
    Quaternion Offset;
    int Turn;
    float T;
    float ConstTime;
    float count;
    AudioSource sound;

    public Renderer[] C;
    // Start is called before the first frame update
    void Start()
    {
        Turn = 0;
        Off = false;
        ConstTime = 3f;

        count = 0f;

        T = 5f;

        EndPoint = new Vector3[9];
        EndPoint[0] = Light.forward * 100;
        EndPoint[1] = (Light.right + Light.forward * 3) * 100;
        EndPoint[2] = (Light.right * -1 + Light.forward * 3) * 100;
        EndPoint[3] = (Light.up * -1 + Light.forward * 3) * 100;
        EndPoint[4] = (Light.up + Light.forward * 8) * 100;
        EndPoint[5] = (Light.up * -0.5f - Light.right * -0.75f + Light.forward * 3) * 100;
        EndPoint[6] = (Light.up * -0.5f - Light.right * 0.75f + Light.forward * 3) * 100;
        EndPoint[7] = (Light.up * 0.5f - Light.right * 0.75f + Light.forward * 8) * 100;
        EndPoint[8] = (Light.up * 0.5f - Light.right * -0.75f + Light.forward * 8) * 100;

        if (Invisable != 2)
        {
            Offset = Head.localRotation;
            sound = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!ChangeView.Pause)
        {
            if (count < 1f)
            {
                if (!Off)
                {
                    Turn_Update();
                }
                else
                {
                    if (sound.isPlaying)
                    {
                        sound.Stop();
                    }
                    if (off_Rot.localEulerAngles.x < 23f)
                    {
                        off_Rot.localEulerAngles = new Vector3(Mathf.MoveTowards(off_Rot.localEulerAngles.x, 23f, 0.4f), off_Rot.localEulerAngles.y, off_Rot.localEulerAngles.z);
                    }
                    T += Time.deltaTime;
                    if (T >= ConstTime)
                    {
                        T = 0f;
                        ConstTime = Random.Range(2f, 4f);
                        Spark.Play();
                        Spark.transform.GetChild(1).GetComponent<AudioSource>().Play();
                    }
                    if (C[C.Length - 1].material.GetColor("_BaseColor").r > 0)
                    {
                        for (int i = 0; i < C.Length; i++)
                        {
                            C[i].material.SetColor("_BaseColor", new Color(Mathf.MoveTowards(C[i].material.GetColor("_BaseColor").r, 0f, 0.03f), C[i].material.GetColor("_BaseColor").g, C[i].material.GetColor("_BaseColor").b, 1f));
                        }
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!ChangeView.Pause)
        {
            if (count < 1f)
            {
                if (!Off && !Just_Look)
                {
                    RayCast();
                }
            }
        }
    }

    IEnumerator DoTurn(int much)
    {
        sound.Stop();
        Turn = 100;
        yield return new WaitForSeconds(2f);
        Turn = much;
        sound.Play();
    }

    void Turn_Update()
    {
        if (Invisable == 0)
        {
            if (Turn == 0)
            {
                Head.localRotation = new Quaternion(Head.localRotation.x, Mathf.MoveTowards(Head.localRotation.y, Offset.y + 0.3f, 0.006f), Head.localRotation.z, Head.localRotation.w);
                if (Head.localRotation.y >= Offset.y + 0.25f)
                {
                    StartCoroutine(DoTurn(1));
                }
            }
            else if (Turn == 1)
            {
                Head.localRotation = new Quaternion(Head.localRotation.x, Mathf.MoveTowards(Head.localRotation.y, Offset.y - 0.3f, 0.006f), Head.localRotation.z, Head.localRotation.w);
                if (Head.localRotation.y <= Offset.y - 0.25f)
                {
                    StartCoroutine(DoTurn(0));
                }
            }
        }
        else if (Invisable == 1)
        {
            Head.localEulerAngles = new Vector3(Head.localEulerAngles.x, Head.localEulerAngles.y + 0.3f, Head.localEulerAngles.z);
        }
        else
        {
            T += Time.deltaTime;
            if (T >= 3f)
            {
                T = 0f;
                Light_Offset.gameObject.SetActive(!Light_Offset.gameObject.activeInHierarchy);
                Light.gameObject.SetActive(!Light.gameObject.activeInHierarchy);
            }
        }
    }

    void RayCast()
    {
        if (Light_Offset.gameObject.activeInHierarchy)
        {
            for (int i = 0; i < EndPoint.Length; i++)
            {
                RaycastHit Hit;
                int layerMask = ((1 << LayerMask.NameToLayer("CCTV_Glass")));
                layerMask = ~layerMask;
                if (Physics.Raycast(Light_Offset.position, EndPoint[i], out Hit, 100f, layerMask))
                {
                    if (Hit.collider != null)
                    {
                        Find_Player(Hit);
                    }
                }
            }
            EndPoint[0] = Light.forward * 100;
            EndPoint[1] = (Light.right + Light.forward * 3) * 100;
            EndPoint[2] = (Light.right * -1 + Light.forward * 3) * 100;
            EndPoint[3] = (Light.up * -1 + Light.forward * 3) * 100;
            EndPoint[4] = (Light.up + Light.forward * 8) * 100;
            EndPoint[5] = (Light.up * -0.5f - Light.right * -0.75f + Light.forward * 3) * 100;
            EndPoint[6] = (Light.up * -0.5f - Light.right * 0.75f + Light.forward * 3) * 100;
            EndPoint[7] = (Light.up * 0.5f - Light.right * 0.75f + Light.forward * 8) * 100;
            EndPoint[8] = (Light.up * 0.5f - Light.right * -0.75f + Light.forward * 8) * 100;
        }
    }

    void Find_Player(RaycastHit Hit)
    {
        if (Hit.transform.gameObject.name == "Player" && Hit.transform.gameObject.tag == "Player")
        {
            Color c = Hit.transform.GetComponent<Player>().ui.See.color;
            c.a = Mathf.MoveTowards(c.a, 1f, 0.01f);
            Hit.transform.GetComponent<Player>().ui.See.color = c;
            count = Hit.transform.GetComponent<Player>().ui.See.color.a;
            if (count >= 1f)
            {
                if (Hit.transform.GetComponent<Player>().Dead == 0)
                {
                    Hit.transform.GetComponent<Player>().Dead = 1;
                    Hit.transform.GetComponent<Player>().Dead_Pos = new Vector3(transform.position.x, transform.position.y - 15f, transform.position.z);
                }
            }
        }
    }
}
