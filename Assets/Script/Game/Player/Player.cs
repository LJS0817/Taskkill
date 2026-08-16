using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public static class ChangeView
{
    public static bool Drone = false;
    public static float DroneHp = 1f;
    public static int Change = 0;

    public static bool Pause = false;
}

public class Player : MonoBehaviour
{
    public Sound Audio;
    public Player_UI ui;
    public Player_Map Map;
    [HideInInspector]
    public Stealth Kill;

    public Image Panel;
    public Transform Offset;
    public GameObject[] Ori_Obj;

    public GameObject DronePanel;
    public Camera DroneCam;
    public Camera PlayerCam;
    public Transform[] DroneObj;

    public GameObject Knife;
    public GameObject Pistol;
    public GameObject Magazine_Ori;
    public GameObject Magazine_Parent;
    public GameObject Magazine_Parent_Before;
    public GameObject Magazine_Trash_Can;

    [HideInInspector]
    public int Dead;
    [HideInInspector]
    public Vector3 Dead_Pos;

    public Ending ending;

    Animator Ani;
    Rigidbody R;
    float Speed;
    bool Shot;
    bool isReload;
    bool Has_Key;

    Transform Object;

    float Move_Time;
    float Run_Time;
    float Effect_Time;

    [HideInInspector]
    public AI ai;

    PostProcessVolume Vol;

    bool NotMove()
    {
        return (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A));
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        ui.SetText();
        Stage();
        Object = null;

        ui.Set_Mission(Map.Stage);

        Dead = 0;
        Shot = isReload = Has_Key = false;
        R = GetComponent<Rigidbody>();
        Ani = GetComponent<Animator>();
        Kill = GetComponent<Stealth>();
        Speed = 25f;
        Move_Time = 2f;
        Run_Time = 0.75f;
        Effect_Time = 8f;
        ai = null;

        Vol = Camera.main.GetComponent<PostProcessVolume>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Map.Up == 0 && !Ani.enabled)
        {
            Ani.enabled = true;
        }
        Ani.SetBool("Load", Player_UI.Loading_done);
        if(ending.End_ < 4)
        {
            Audio.SOUND_UPDATE();
        }
        else
        {
            if(Audio.A[3].volume > 0)
            {
                Audio.A[3].volume = Mathf.MoveTowards(Audio.A[3].volume, 0f, 0.01f);
            }
        }
        if (!ending.End)
        {
            if (!ChangeView.Pause)
            {
                if (Dead == 0)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    ChangeViewLoading();
                    if (ChangeView.Change == 0)
                    {
                        if (!ChangeView.Drone)
                        {
                            Stage_Up();
                            if (!Kill.Kill && Player_UI.Loading_done)
                            {
                                Sound_Move();
                                Weapon();
                                if (!Ani.GetBool("Crouch"))
                                {
                                    if (Vol.profile.GetSetting<Vignette>().intensity.value > 0.4f)
                                    {
                                        Vol.profile.GetSetting<Vignette>().intensity.value = Mathf.MoveTowards(Vol.profile.GetSetting<Vignette>().intensity, 0.4f, 0.01f);
                                    }
                                    Run();
                                    if (Pistol.activeInHierarchy)
                                    {
                                        Shooting();
                                        SetReload();
                                    }
                                    ChargeDrone();
                                    Recovery();
                                }
                                else
                                {
                                    if(Vol.profile.GetSetting<Vignette>().intensity.value < 0.6f)
                                    {
                                        Vol.profile.GetSetting<Vignette>().intensity.value = Mathf.MoveTowards(Vol.profile.GetSetting<Vignette>().intensity, 0.6f, 0.01f);
                                    }
                                }
                                if (Map.Up == 0)
                                {
                                    Crouch();
                                }
                            }
                        }
                    }
                    Low_Dead();
                }
                else if (Dead == 1)
                {
                    Vector3 vec = Dead_Pos - transform.position;
                    vec.Normalize();
                    Quaternion q = Quaternion.LookRotation(vec);
                    transform.rotation = Quaternion.Lerp(transform.rotation, q, 0.03f);

                    if (Ani.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Dead")
                    {
                        Ani.SetTrigger("Dead");
                        R.velocity = Vector3.zero;
                    }
                }
                else
                {
                    Dead_Fade();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!ending.End && Player_UI.Loading_done)
        {
            if (Dead == 0)
            {
                if (ChangeView.Change == 0)
                {
                    if (!ChangeView.Drone)
                    {
                        if (!ChangeView.Pause)
                        {
                            if (!Kill.Kill)
                            {
                                Move();
                            }
                            Cast();
                        }
                    }
                }
            }
        }
    }

    void Low_Dead()
    {
        if (ui.Player_Hp <= 0f)
            End_2();
    }

    void Change_Weapon()
    {
        Knife.SetActive(!Knife.activeInHierarchy);
        Pistol.SetActive(!Pistol.activeInHierarchy);
        Ani.SetBool("Knife", Knife.activeInHierarchy);
    }

    void Weapon()
    {
        if (!Ani.GetBool("Crouch"))
        {
            if (Input.GetKeyUp(KeyCode.V))
            {
                if (Ani.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Change")
                {
                    Ani.SetTrigger("Change");
                    ui.Change_Weapon();
                }
            }
        }
    }

    void Sound_Move()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            Move_Time += Time.deltaTime;
            if (Move_Time >= Run_Time)
            {
                Move_Time = 0f;
                Audio.PLAY_SOUND(Sound.E_Sound.WALK);
            }
        }
        else if (Move_Time != 2f)
        {
            Move_Time = 2f;
        }

        if (ui.Player_Hp <= 0.15f)
        {
            Effect_Time += Time.deltaTime;
            if (Effect_Time >= 8f)
            {
                Effect_Time = 0f;
                Audio.PLAY_SOUND(Sound.E_Sound.LOW_POWER);
            }
        }
    }

    void Cast()
    {
        RaycastHit Hit;
        int layerMask = ((1 << LayerMask.NameToLayer("Player")));
        layerMask = ~layerMask;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out Hit, 20f, layerMask))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 20, Color.red);
            if (Hit.collider != null)
            {
                Going_Up(Hit.transform.gameObject);
                Get_Gun(Hit);
                Open_Vent(Hit);
                Get_Key(Hit);
                Set_Key(Hit);
                Set_End(Hit);
                Stealth_Kill(Hit);
                if (Hit.transform.gameObject.tag == "Ammo" || Hit.transform.gameObject.tag == "Up" || Hit.transform.gameObject.tag == "Vent" || Hit.transform.gameObject.tag == "Card"
                    || Hit.transform.gameObject.tag == "Key" || Hit.transform.gameObject.tag == "Ending" ||  Hit.transform.gameObject.layer == 18)
                {
                    if (!Has_Key)
                    {
                        if (Hit.transform.gameObject.tag == "Key")
                        {
                            if (ui.Press_E.text != "Need KeyCard") { ui.Press_E.text = "Need KeyCard"; }
                        }
                        else
                        {
                            if (Knife.activeInHierarchy && Hit.transform.gameObject.layer == 18)
                            {
                                if (ui.Press_E.text != "On Target") { ui.Press_E.text = "On Target"; }
                            }
                            else
                            {
                                if (ui.Press_E.text != "Press E") { ui.Press_E.text = "Press E"; }
                            }
                        }
                    }
                    else
                    {
                        if (Knife.activeInHierarchy && Hit.transform.gameObject.layer == 18)
                        {
                            if (ui.Press_E.text != "On Target") { ui.Press_E.text = "On Target"; }
                        }
                        else
                        {
                            if (ui.Press_E.text != "Press E") { ui.Press_E.text = "Press E"; }
                        }
                    }
                    if (ui.Press_E.color.a < 1f) { ui.Press_E.color = new Color(ui.Press_E.color.r, ui.Press_E.color.g, ui.Press_E.color.b, Mathf.MoveTowards(ui.Press_E.color.a, 1f, 0.1f)); }
                }
                else
                {
                    if (ui.Press_E.color.a > 0f) { ui.Press_E.color = new Color(ui.Press_E.color.r, ui.Press_E.color.g, ui.Press_E.color.b, Mathf.MoveTowards(ui.Press_E.color.a, 0f, 0.1f)); }
                }
            }
        }
        else
        {
            if (ui.Press_E.color.a > 0f) { ui.Press_E.color = new Color(ui.Press_E.color.r, ui.Press_E.color.g, ui.Press_E.color.b, Mathf.MoveTowards(ui.Press_E.color.a, 0f, 0.1f)); }
        }
    }

    void Run()
    {
        if (gameObject.activeInHierarchy)
        {
            if (!isReload)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    Speed = 50f;
                    Ani.SetBool("Run", true);
                    Run_Time = 0.45f;
                }
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    Speed = 25f;
                    Run_Time = 0.75f;
                    Ani.SetBool("Run", false);
                }
                if (NotMove())
                {
                    Speed = 25f;
                    Run_Time = 0.75f;
                    Ani.SetBool("Run", false);
                }
            }
        }
    }

    void Going_Up(GameObject other)
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (other.tag == "Up" && Map.Up == 0)
            {
                Ani.SetTrigger("Click");
                other.transform.parent.parent.parent.GetComponent<Animator>().SetBool("Done", !other.transform.parent.parent.parent.GetComponent<Animator>().GetBool("Done"));
                other.transform.parent.parent.parent.GetComponent<Door>().A.Play();
                Audio.PLAY_SOUND(Sound.E_Sound.PICK);
                Map.Up = 1;
            }
        }
    }

    void Stealth_Kill(RaycastHit Hit)
    {
        if(!Ani.GetBool("Crouch"))
        {
            if (Input.GetKeyUp(KeyCode.E) || (Knife.activeInHierarchy && Input.GetMouseButtonDown(0)))
            {
                if (Hit.transform.gameObject.layer == 18)
                {
                    if (!Kill.Kill)
                    {
                        ai = Hit.transform.GetComponent<AI>();
                        Hit.transform.parent = Kill.Enemy_Pos;
                        ai.transform.position = Kill.Enemy_Pos.position;
                        ai.transform.localRotation = Quaternion.identity;
                        ai.Catch = true;
                        if (ai.Nav != null)
                            ai.Nav.enabled = false;
                        R.isKinematic = true;
                        if (Pistol.activeInHierarchy)
                            Kill.Stealth_Kill(2);
                        else
                            Kill.Stealth_Kill(1);
                    }
                }
            }
        }
    }

    public void Start_Stealth()
    {
        Kill.P.Play();
        Kill.P.transform.GetChild(1).GetComponent<AudioSource>().Play();
    }

    public void Stealth_Hp()
    {
        ai.Hp = 0f;
    }

    public void Stealth_Ai_Ani()
    {
        ai.Ani.SetInteger("Stealth", Ani.GetInteger("Stealth"));
    }

    public void Stealth_Dead()
    {
        Kill.Stealth_Kill(0);
        Kill.Kill = false;
        R.isKinematic = false;
        Recovery();
        ai = null;
    }

    public void Stage()
    {
        Map.Stage_Up();
    }

    void ChangeDrone()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (Ani.GetInteger("Drone") == 0)
            {
                Ani.SetBool("Run", false);
                Speed = 25f;
                Ani.SetInteger("Drone", 1);
            }
        }
    }

    void Stage_Up()
    {
        Map.Stage_Up();
        if (Map.Up == 1)
        {
            Panel.color = new Color(Panel.color.r, Panel.color.g, Panel.color.b, Mathf.MoveTowards(Panel.color.a, 1f, 0.02f));
            if (Panel.color.a >= 1f)
            {
                Map.Up = 2;
                Audio.PLAY_BACKGROUND_MUSIC(Map.Stage + 1);
                ui.Set_Mission(Map.Stage + 1);
                Has_Key = false;
                ui.Recovery_Hp();
            }
        }
        else if (Map.Up == 3)
        {
            Panel.color = new Color(Panel.color.r, Panel.color.g, Panel.color.b, Mathf.MoveTowards(Panel.color.a, 0f, 0.02f));
            if (Panel.color.a <= 0f)
            {
                Map.Up = 0;
                if(Map.Stage == 6)
                {
                    ending.Box.gameObject.SetActive(true);
                }
            }
        }
        else if (Map.Up == 4)
        {
            Panel.color = new Color(Panel.color.r, Panel.color.g, Panel.color.b, Mathf.MoveTowards(Panel.color.a, 1f, 0.02f));
            if (Panel.color.a >= 1f)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.SceneManagement.SceneManager.LoadScene("ResultScene");
            }
        }

        if (Map.Check_Fire(Pistol.activeInHierarchy))
        {
            Dead_Pos = Map.Ala.transform.position;
            Dead = 1;
        }
    }

    void Dead_Fade()
    {
        Panel.color = new Color(Panel.color.r, Panel.color.g, Panel.color.b, Mathf.MoveTowards(Panel.color.a, 1f, 0.02f));
        if (Panel.color.a >= 1f)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            UnityEngine.SceneManagement.SceneManager.LoadScene("ResultScene");
            Dead = 3;
        }
    }

    public void Drone()
    {
        DroneObj[1].gameObject.SetActive(true);

        Ani.SetInteger("Drone", 2);
    }

    public void Throw()
    {
        DroneObj[1].GetComponent<Rigidbody>().useGravity = true;
        DroneObj[1].GetComponent<Rigidbody>().AddForce(new Vector3(transform.forward.x, 1f, transform.forward.z) * 30f, ForceMode.Impulse);
        DroneObj[1].GetComponent<Hero>().jump = 2;
        DroneObj[1].parent = DroneObj[0];
        DroneObj[1].eulerAngles = new Vector3(DroneObj[1].eulerAngles.x, transform.eulerAngles.y, 0);
        ChangeView.Change = 1;
    }

    void Move()
    {
        if (Map.Up == 0)
        {
            R.velocity = transform.rotation * new Vector3(Input.GetAxis("Horizontal") * Speed, R.velocity.y, Input.GetAxis("Vertical") * Speed);

            R.MoveRotation(Quaternion.Euler(0, transform.eulerAngles.y + Input.GetAxis("Mouse X"), 0));

            Offset.Rotate(-Input.GetAxis("Mouse Y"), 0, 0);
            if (Offset.localRotation.x < -0.5f) { Offset.localRotation = new Quaternion(-0.5f, Offset.localRotation.y, Offset.localRotation.z, Offset.localRotation.w); }
            if (Offset.localRotation.x > 0.5f) { Offset.localRotation = new Quaternion(0.5f, Offset.localRotation.y, Offset.localRotation.z, Offset.localRotation.w); }

            GetComponent<BoxCollider>().center = new Vector3(GetComponent<BoxCollider>().center.x, GetComponent<BoxCollider>().center.y, 3 + Offset.localRotation.x * 10f);
        }


        if (NotMove())
        {
            R.velocity = new Vector3(0, R.velocity.y, 0);
            if (Ani.GetBool("Walk")) { Ani.SetBool("Walk", false); }
        }
        else
        {
            //if (Ani.GetBool("Crouch") && !Ani.GetBool("Walk"))
            //{
            //    Ani.SetInteger("Crouch_Walk", 1);
            //}
            if (!Ani.GetBool("Run") && Run_Time != 0.75f && !isReload && !Ani.GetBool("Crouch"))
            {
                Run_Time = 0.75f;
            }

            if (!Ani.GetBool("Walk"))
            {
                Ani.SetBool("Walk", true);
            }
        }
    }

    void Shooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!Shot && ui.Bullet_Now > 0)
            {
                Ani.SetBool("Run", false);
                Speed = 25f;

                Ani.SetTrigger("Shot");
                StartCoroutine(CanShot());
            }
            if (ui.Bullet_Now == 0 && ui.Bullet_Lit > 0 && !isReload)
            {
                ValueReload();
            }
        }
    }

    void ValueReload()
    {
        Ani.SetBool("Run", false);
        isReload = true;
        Speed = 10f;
        Run_Time = 1f;
        Ani.SetTrigger("Reload");
    }

    void SetReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && ui.Bullet_Now < Player_UI.Bullet && ui.Bullet_Lit > 0 && !Shot)
        {
            ValueReload();
        }
    }

    void ChangeValue()
    {
        ChangeView.Drone = !ChangeView.Drone;
        PlayerCam.enabled = !PlayerCam.enabled;
        DroneCam.enabled = !DroneCam.enabled;
        DroneCam.gameObject.GetComponent<UnityEngine.Rendering.PostProcessing.PostProcessVolume>().enabled = DroneCam.enabled;
        DroneCam.gameObject.GetComponent<AudioListener>().enabled = DroneCam.enabled;
        DronePanel.SetActive(!DronePanel.activeInHierarchy);
        Panel.transform.parent.gameObject.SetActive(!DronePanel.activeInHierarchy);
        DroneObj[1].GetComponent<BoxCollider>().isTrigger = DroneObj[1].GetChild(2).GetChild(0).GetComponent<CapsuleCollider>().isTrigger = !DroneObj[1].GetChild(2).GetChild(0).GetComponent<CapsuleCollider>().isTrigger;
        R.isKinematic = !R.isKinematic;
        GetComponent<BoxCollider>().enabled = !GetComponent<BoxCollider>().enabled;
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Ani.SetBool("Run", false);
            Ani.SetBool("Crouch", !Ani.GetBool("Crouch"));
            Speed = Ani.GetBool("Crouch") ? 10f : 25f;
            Run_Time = 1f;
            Audio.PLAY_SOUND(Ani.GetBool("Crouch") ? Sound.E_Sound.CROUCH : Sound.E_Sound.GET_UP);
            GetComponent<BoxCollider>().size = new Vector3(GetComponent<BoxCollider>().size.x, Ani.GetBool("Crouch") ? 10f : 18f, GetComponent<BoxCollider>().size.z);
            GetComponent<BoxCollider>().center = new Vector3(GetComponent<BoxCollider>().center.x, Ani.GetBool("Crouch") ? -4f : 0.4f, GetComponent<BoxCollider>().center.z);
        }
        if (Ani.GetBool("Crouch") || Ani.GetBool("Run"))
        {
            if (ui.op.Vary_Aim[ui.op.AimIndex].color.a > 0f)
            {
                for (int i = 0; i < ui.op.Vary_Aim.Length; i++)
                {
                    Color c = ui.op.Vary_Aim[i].color;
                    c.a = Mathf.MoveTowards(c.a, 0f, 0.05f);
                    ui.op.Vary_Aim[i].color = c;
                }
            }
        }
        else
        {
            if (ui.op.Vary_Aim[ui.op.AimIndex].color.a < 1f)
            {
                for (int i = 0; i < ui.op.Vary_Aim.Length; i++)
                {
                    Color c = ui.op.Vary_Aim[i].color;
                    c.a = Mathf.MoveTowards(c.a, 1f, 0.05f);
                    ui.op.Vary_Aim[i].color = c;
                }
            }
        }
    }

    void Recovery()
    {
        if (Pistol.GetComponent<Gun>().Dead_Enemy)
        {
            ui.Recovery_Hp();
            Pistol.GetComponent<Gun>().Dead_Enemy = false;
        }
    }

    void ChangeViewLoading()
    {
        if (ChangeView.Change == 1)
        {
            Panel.color = new Color(Panel.color.r, Panel.color.g, Panel.color.b, Panel.color.a + 0.02f);
            if (Panel.color.a > 1f)
            {
                ChangeValue();
                for (int i = 0; i < Ori_Obj.Length; i++)
                {
                    Ori_Obj[i].SetActive(!Ori_Obj[i].activeInHierarchy);
                }
                ChangeView.Change = 2;
            }
        }
        else if (ChangeView.Change == 4)
        {
            ChangeValue();
            for (int i = Ori_Obj.Length - 1; i >= 0; i--)
            {
                Ori_Obj[i].SetActive(!Ori_Obj[i].activeInHierarchy);
            }
            Ani.SetInteger("Drone", 4);
            DroneObj[1].parent = DroneObj[2];
            DroneObj[1].gameObject.SetActive(false);
            DroneObj[1].localPosition = Vector3.zero;
            DroneObj[1].eulerAngles = Vector3.zero;
            DroneObj[1].GetComponent<Rigidbody>().useGravity = false;
            ChangeView.Change = 5;
        }
        else if (ChangeView.Change == 5)
        {
            Panel.color = new Color(Panel.color.r, Panel.color.g, Panel.color.b, Panel.color.a - 0.02f);
            if (Panel.color.a < 0)
            {
                Ani.SetInteger("Drone", 0);
                ChangeView.Change = 0;
            }
        }
        if (ChangeView.Change == 0 && Map.Up == 0)
        {
            if (Input.GetKeyUp(KeyCode.Q))
            {
                if (Ani.GetInteger("Drone") == 0)
                {
                    Ani.SetInteger("Drone", 1);
                    Audio.PLAY_SOUND(Sound.E_Sound.SIT);
                }
                else if (Ani.GetInteger("Drone") == 2)
                {
                    ChangeView.Change = 3;
                }
            }
        }
    }

    void ChargeDrone()
    {
        if (ChangeView.DroneHp < 1f)
        {
            ChangeView.DroneHp += 0.001f;
        }
        else if (ChangeView.DroneHp > 1f)
        {
            ChangeView.DroneHp = 1f;
        }
    }

    IEnumerator CanShot()
    {
        Shot = true;
        yield return new WaitForSeconds(0.2f);
        Shot = false;
        ui.Shooting();
    }

    public void GunAni()
    {
        Audio.PLAY_SOUND(Sound.E_Sound.FIRE);
        Pistol.GetComponent<Animator>().SetTrigger("Shot");
        if(Ani.GetInteger("Stealth") == 2)
        {
            ui.Shooting();
        }
    }

    public void Reload()
    {
        Magazine_Parent.transform.GetChild(0).gameObject.AddComponent<Rigidbody>();
        Destroy(Magazine_Parent.transform.GetChild(0).gameObject, 2f);
        Magazine_Parent.transform.GetChild(0).parent = Magazine_Trash_Can.transform;
        Pistol.GetComponent<Animator>().SetTrigger("Reload");
        ui.BulletReset();
    }

    public void Magazine_Out()
    {
        Audio.PLAY_SOUND(Sound.E_Sound.RELOAD_IN);
    }

    public void ChangeMagazine()
    {
        Instantiate(Magazine_Ori, Magazine_Parent_Before.transform.position, Magazine_Parent_Before.transform.rotation, Magazine_Parent_Before.transform);
    }

    public void SetParent()
    {
        Transform C = Magazine_Parent_Before.transform.GetChild(0);
        C.parent = Magazine_Parent.transform;
        C.localPosition = Vector3.zero;
        C.localRotation = Quaternion.identity;
        ui.Reload();
        isReload = false;
        Speed = 25f;
    }

    public void End()
    {
        if (Pistol.activeInHierarchy)
        {
            Pistol.GetComponent<BoxCollider>().enabled = true;
            Pistol.AddComponent<Rigidbody>();
            Pistol.GetComponent<Rigidbody>().mass = 0.01f;
        }
        else
        {
            Knife.GetComponent<BoxCollider>().enabled = true;
            Knife.AddComponent<Rigidbody>();
            Knife.GetComponent<Rigidbody>().mass = 0.01f;
        }
    }

    public void End_2()
    {
        Dead = 2;
    }

    void Get_Gun(RaycastHit Hit)
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (Hit.transform.gameObject.tag == "Ammo" && Object == null)
            {
                Object = Hit.transform;
                Audio.PLAY_SOUND(Sound.E_Sound.PICK);
                Ani.SetTrigger("Get");
            }
        }
    }

    void Open_Vent(RaycastHit Hit)
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (Hit.transform.gameObject.tag == "Vent")
            {
                Hit.transform.GetComponent<Animator>().SetTrigger("Open");
                Hit.transform.gameObject.tag = "Untagged";
                Ani.SetTrigger("Get");
            }
        }
    }

    void Get_Key(RaycastHit Hit)
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (Hit.transform.gameObject.tag == "Card" && Object == null)
            {
                Ani.SetTrigger("Get");
                Object = Hit.transform;
                Audio.PLAY_SOUND(Sound.E_Sound.PICK);
                Has_Key = true;
            }
        }
    }

    void Set_Key(RaycastHit Hit)
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (Hit.transform.gameObject.tag == "Key")
            {
                if (Has_Key)
                {
                    Transform T = FindDoor(Hit.transform);
                    T.GetComponent<Door>().Need_Key = false;
                    Audio.PLAY_SOUND(Sound.E_Sound.UNLOCK);
                }
                else
                {
                    Audio.PLAY_SOUND(Sound.E_Sound.LOCK);
                }
            }
        }
    }

    void Set_End(RaycastHit Hit)
    {
        if (Hit.transform.gameObject.tag == "Ending")
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                Ani.SetTrigger("End");
                ui.HP_Image.gameObject.SetActive(false);
                ui.op.Player_Sound();
                Ani.enabled = false;
                ending.End = true;
            }
        }
    }

    public void Destroy_Ammo()
    {
        if (Object != null)
        {
            if (Object.tag == "Ammo")
            {
                ui.Bullet_Lit += Random.Range(2, 5);
                ui.SetText();
                Destroy(Object.gameObject);
                Object = null;
            }
            else
            {
                Destroy(Object.gameObject);
                Object = null;
            }
        }
    }

    Transform FindDoor(Transform T)
    {
        if (T.name.Contains("door"))
        {
            return T;
        }
        else
        {
            return FindDoor(T.parent);
        }
    }

    public void Reloading()
    {
        Audio.PLAY_SOUND(Sound.E_Sound.RELOAD_IN);
        Audio.PLAY_SOUND(Sound.E_Sound.RELOAD);
    }

    void Before_Play(int n)
    {
        if(n == 10)
        {
            ui.Loading.text = "C O M P L E T E";
        }
        else if(n < 10)
        {
            Audio.PLAY_BEFORE_SOUND(n);
        }
        else
        {
            Destroy(ui.Loading.transform.parent.gameObject);
            Player_UI.Loading_done = true;
        }
    }
}