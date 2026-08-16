using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public GameObject Audio;

    [HideInInspector]
    public NavMeshAgent Nav;

    public Transform[] EndPos;
    int End_Index;

    Vector3 Pos;
    [HideInInspector]
    public Animator Ani;
    public Transform Point_Pos;
    public Transform Point_Pos_2;
    public Renderer C;      //Using Change Color
    [HideInInspector]
    public bool Dead;

    const float allowance = 1.5f;       //허용범위 ±1.5
    public float Hp;

    public GameObject Dead_T;

    List<Vector3> EndPoint;

    public int Sit;

    public int Talk;
    public bool Key_Card;

    float count;

    public Transform Ammo;
    public Transform gun;

    float T;
    float ConstTime;
    public ParticleSystem Spark;
    public ParticleSystem Smoke;

    public bool Catch;

    public Transform Card;

    public AudioSource[] A;
    float Walk_Time;
    // Start is called before the first frame update
    void Start()
    {
        ConstTime = Hp * 2f;
        T = 5f;
        Catch = false;
        Walk_Time = 0f;

        End_Index = 1;
        count = 0f;
        Dead = false;
        Hp = 1f;
        Ani = GetComponent<Animator>();

        if (Talk == 0 || Talk == 50)
            Ani.SetInteger("Sit", Sit);
        else
            Ani.SetInteger("Sit", 100);
        Ani.SetInteger("Talk", Talk);

        if (Sit < 50)
        {
            Pos = EndPos[End_Index].position;

            if ((Sit == 0 || Sit == 4) && Talk == 0)
            {
                Nav = GetComponent<NavMeshAgent>();
            }
            else
            {
                Destroy(GetComponent<NavMeshAgent>());
            }

            if (Key_Card)
            {
                Card.gameObject.SetActive(true);
            }

            EndPoint = new List<Vector3>();

            EndPoint.Add(transform.forward * 100f);

            EndPoint.Add((transform.right + transform.forward * 0.5f) * 400f);
            EndPoint.Add((transform.right + transform.forward * 0.75f) * 400f);
            EndPoint.Add((transform.right + transform.forward * 1f) * 400f);
            EndPoint.Add((transform.right + transform.forward * 1.5f) * 400f);
            EndPoint.Add((transform.right + transform.forward * 2) * 400f);
            EndPoint.Add((transform.right + transform.forward * 2.5f) * 400f);
            EndPoint.Add((transform.right + transform.forward * 3f) * 400f);
            EndPoint.Add((transform.right + transform.forward * 3.5f) * 400f);
            EndPoint.Add((transform.right + transform.forward * 4f) * 400f);

            EndPoint.Add((transform.right * -1 + transform.forward * 0.5f) * 400f);
            EndPoint.Add((transform.right * -1 + transform.forward * 0.75f) * 400f);
            EndPoint.Add((transform.right * -1 + transform.forward * 1f) * 400f);
            EndPoint.Add((transform.right * -1 + transform.forward * 1.5f) * 400f);
            EndPoint.Add((transform.right * -1 + transform.forward * 2) * 400f);
            EndPoint.Add((transform.right * -1 + transform.forward * 2.5f) * 400f);
            EndPoint.Add((transform.right * -1 + transform.forward * 3f) * 400f);
            EndPoint.Add((transform.right * -1 + transform.forward * 3.5f) * 400f);
            EndPoint.Add((transform.right * -1 + transform.forward * 4f) * 400f);

            EndPoint.Add((transform.up * -1 + transform.forward * 3) * 500f);
            EndPoint.Add((transform.up + transform.forward * 8) * 500f);

            EndPoint.Add((transform.up * -0.5f - transform.right * -0.75f + transform.forward * 3) * 500f);
            EndPoint.Add((transform.up * -0.5f - transform.right * 0.75f + transform.forward * 3) * 500f);

            EndPoint.Add((transform.up * 0.5f - transform.right * 0.75f + transform.forward * 8) * 500f);
            EndPoint.Add((transform.up * 0.5f - transform.right * -0.75f + transform.forward * 8) * 500f);

            EndPoint.Add((transform.up * -0.75f - transform.right * 1f + transform.forward * 8) * 500f);
            EndPoint.Add((transform.up * -0.75f - transform.right * -1f + transform.forward * 8) * 500f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Sit < 50)
        {
            if (!ChangeView.Pause)
            {
                if (Sit == 4 && Talk == 0)
                {
                    if (!Catch)
                    {
                        isTurn();
                    }
                }
                else
                {
                    if (count < 1f)
                    {
                        if (C.material.GetColor("_BaseColor").r == Hp)
                        {
                            if (Hp <= 0f && !Dead)
                            {
                                GameObject g = Instantiate(Audio.gameObject, transform.position, Quaternion.identity, transform.parent);
                                g.GetComponent<AudioSource>().Play();
                                Destroy(g, 7f);
                                Dead_T.SetActive(true);
                                Dead = true;
                                if (!Key_Card)
                                {
                                    gun.parent = Ammo;
                                    if (Random.Range(0, 100) < 45)
                                    {
                                        gun.gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        Destroy(gun.gameObject);
                                    }
                                }
                                else
                                {
                                    Card.gameObject.AddComponent<Rigidbody>();
                                    Card.parent = Ammo;
                                }
                                Destroy(gameObject);
                            }
                        }
                        else
                        {
                            C.material.SetColor("_BaseColor", new Color(Mathf.MoveTowards(C.material.GetColor("_BaseColor").r, Hp, 0.03f), C.material.GetColor("_BaseColor").g, C.material.GetColor("_BaseColor").b, 1f));
                        }
                        if (Sit == 0 && Talk == 0)
                        {
                            if (!Dead)
                            {
                                if (!Catch)
                                {
                                    isTurn();
                                }
                                if (Hp <= 0.5f)
                                {
                                    T += Time.deltaTime;
                                    if (T >= ConstTime)
                                    {
                                        T = 0f;
                                        ConstTime = (1f + Hp) * Random.Range(1f, 3f);
                                        Spark.Play();
                                        Spark.transform.GetChild(1).GetComponent<AudioSource>().Play();
                                        if (Hp <= 0.25f)
                                        {
                                            Smoke.Play();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (Sit < 50)
        {
            if (!ChangeView.Pause)
            {
                if (Hp > 0f)
                {
                    if (Sit < 3 && Talk == 0)
                    {
                        if (!Catch)
                        {
                            RayCast();
                        }
                    }
                }
            }
        }
    }

    void isTurn()
    {
        if (Ani.GetInteger("Aim") == 0)
        {
            Nav.SetDestination(Pos);
            if ((transform.position.x >= Pos.x - allowance && transform.position.x <= Pos.x + allowance)
                && (transform.position.z >= Pos.z - allowance && transform.position.z <= Pos.z + allowance))
            {
                Walk_Time = 0f;

                if (Sit < 4)
                {
                    if (Pos == EndPos[End_Index].position)
                    {
                        ++End_Index;
                        if (End_Index >= EndPos.Length)
                        {
                            End_Index = 0;
                        }
                        Pos = EndPos[End_Index].position;
                        Ani.SetInteger("Aim", 1);
                    }
                    else
                    {
                        Pos = EndPos[End_Index].position;
                        Ani.SetInteger("Aim", 1);
                    }
                }
                else
                {
                    if (Pos == EndPos[1].position)
                    {
                        if (Ani.GetInteger("Sit") != 1)
                        {
                            Ani.SetInteger("Sit", 1);
                        }
                        else
                        {
                            C.material.SetColor("_BaseColor", new Color(Mathf.MoveTowards(C.material.GetColor("_BaseColor").r, 1f, 0.03f), Mathf.MoveTowards(C.material.GetColor("_BaseColor").g, 0f, 0.03f),
                                Mathf.MoveTowards(C.material.GetColor("_BaseColor").b, 0f, 0.03f)));
                            transform.position = EndPos[1].position;
                            transform.forward = EndPos[1].forward * -1;
                            if (C.material.GetColor("_BaseColor") == Color.red)
                            {
                                Pos = EndPos[2].position;
                                Ani.SetInteger("Sit", 5);
                            }
                        }
                    }
                    else if(Pos == EndPos[2].position)
                    {
                        C.material.SetColor("_BaseColor",new Color(0f, 156 / 255, 1f));
                        Pos = EndPos[1].position;
                        transform.position = EndPos[0].position;
                    }
                }
            }
            else
            {
                Walk_Time += Time.deltaTime;
                if (Walk_Time >= 0.625f)
                {
                    A[Random.Range(0, 2)].Play();
                    Walk_Time = 0f;
                }
            }
        }
    }

    void RayCast()
    {
        int layerMask = ((1 << LayerMask.NameToLayer("CCTV_Glass")));
        layerMask = ~layerMask;
        for (int i = 0; i < EndPoint.Count; i++)
        {
            RaycastHit Hit;
            if (Sit == 0)
            {
                if (Physics.Raycast(Point_Pos.position, EndPoint[i], out Hit, 400f, layerMask))
                {
                    if (Hit.collider != null)
                    {
                        Find_Player(Hit);
                    }
                }
            }
            if (Physics.Raycast(Point_Pos_2.position, EndPoint[i], out Hit, 400f, layerMask))
            {
                if (Hit.collider != null)
                {
                    Find_Player(Hit);
                }
            }
        }

        EndPoint[0] = transform.forward * 400f;

        EndPoint[1] = (transform.right + transform.forward * 0.5f) * 400f;
        EndPoint[2] = (transform.right + transform.forward * 0.75f) * 400f;
        EndPoint[3] = (transform.right + transform.forward * 1f) * 400f;
        EndPoint[4] = (transform.right + transform.forward * 1.5f) * 400f;
        EndPoint[5] = (transform.right + transform.forward * 2f) * 400f;
        EndPoint[6] = (transform.right + transform.forward * 2.5f) * 400f;
        EndPoint[7] = (transform.right + transform.forward * 3f) * 400f;
        EndPoint[8] = (transform.right + transform.forward * 3.5f) * 400f;
        EndPoint[9] = (transform.right + transform.forward * 4f) * 400f;

        EndPoint[10] = (transform.right * -1 + transform.forward * 0.5f) * 400f;
        EndPoint[11] = (transform.right * -1 + transform.forward * 0.75f) * 400f;
        EndPoint[12] = (transform.right * -1 + transform.forward * 1f) * 400f;
        EndPoint[13] = (transform.right * -1 + transform.forward * 1.5f) * 400f;
        EndPoint[14] = (transform.right * -1 + transform.forward * 2f) * 400f;
        EndPoint[15] = (transform.right * -1 + transform.forward * 2.5f) * 400f;
        EndPoint[16] = (transform.right * -1 + transform.forward * 3f) * 400f;
        EndPoint[17] = (transform.right * -1 + transform.forward * 3.5f) * 400f;
        EndPoint[18] = (transform.right * -1 + transform.forward * 4.5f) * 400f;


        EndPoint[19] = (transform.up * -1 + transform.forward * 3) * 500f;
        EndPoint[20] = (transform.up + transform.forward * 8) * 500f;

        EndPoint[21] = (transform.up * -0.5f - transform.right * -0.75f + transform.forward * 3) * 500f;
        EndPoint[22] = (transform.up * -0.5f - transform.right * 0.75f + transform.forward * 3) * 500f;

        EndPoint[23] = (transform.up * 0.5f - transform.right * 0.75f + transform.forward * 8) * 500f;
        EndPoint[24] = (transform.up * 0.5f - transform.right * -0.75f + transform.forward * 8) * 500f;

        EndPoint[25] = (transform.up * -0.75f - transform.right * 1f + transform.forward * 8) * 500f;
        EndPoint[26] = (transform.up * -0.75f - transform.right * -1f + transform.forward * 8) * 500f;
    }

    void Find_Player(RaycastHit Hit)
    {
        if(Hit.transform.gameObject.name == "Player" && Hit.transform.gameObject.tag == "Player")
        {
            Color c =Hit.transform.GetComponent<Player>().ui.See.color;
            c.a = Mathf.MoveTowards(c.a, 1f, 0.01f);
            Hit.transform.GetComponent<Player>().ui.See.color = c;
            count = Hit.transform.GetComponent<Player>().ui.See.color.a;

            if (count >= 0.6f)
            {
                Vector3 vec = Hit.transform.position - transform.position;
                vec.Normalize();
                Quaternion q = Quaternion.LookRotation(vec);
                transform.rotation = q;

                Pos = Hit.transform.position;
            }
            if (count >= 1f)
            {
                Ani.SetInteger("Aim", 2);
                if (Sit == 0)
                    Nav.enabled = false;
                //Pos = transform.position;
                //Nav.SetDestination(Pos);

                if (Hit.transform.GetComponent<Player>().Dead == 0)
                {
                    Hit.transform.GetComponent<Player>().Dead = 1;
                    Hit.transform.GetComponent<Player>().Dead_Pos = new Vector3(transform.position.x, Camera.main.transform.position.y - 3.6f, transform.position.z);
                }
            }
        }
        if (Hit.transform.gameObject.tag == "Broke")
        {
            Vector3 vec = Hit.transform.position - transform.position;
            vec.Normalize();
            Quaternion q = Quaternion.LookRotation(vec);
            transform.rotation = q;

            Pos = Hit.transform.position;
        }
    }

    public void End_Point_Final()
    {
        Ani.SetInteger("Aim", 0);
    }

    void Shot()
    {
        A[0].Play();
    }
}
