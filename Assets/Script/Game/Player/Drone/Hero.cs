using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    public GameObject Rec;
    public Image JumpCool;
    public Image Hp;
    public Image Panel;
    public Text RecTime;
    public Transform Wheel;
    public Transform Offset;

    Rigidbody R;
    float[] RecCtrl;
    int On;
    float Speed;
    [HideInInspector]
    public int jump;
    float HpCtrlTime;
    float RecShowCtrlTime;
    float RecNotShowTime;

    bool Hack;
    public Text Hack_T;
    public Image Hack_I;
    float HackTime;

    Transform Locked;

    public Text Press;

    bool Hold;

    float Hack_Alpha_Speed = 0.05f;

    public Sound Audio;

    // Start is called before the first frame update
    void Start()
    {
        Hold = false;
        HackTime = 0f;
        RecCtrl = new float[4] { 0, 0, 0, 0 };
        On = 0;
        RecNotShowTime = 1.5f;
        RecShowCtrlTime = 0f;
        HpCtrlTime = 1f;
        jump = 0;
        Speed = 20f;
        R = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ChangeView.Pause)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            ChangeViewCtrl();
            Holding();
        }
    }

    private void FixedUpdate()
    {
        if (!ChangeView.Pause)
        {
            if (ChangeView.Drone)
            {
                if (On == 2 && ChangeView.Change == 0)
                {
                    if (Hp.fillAmount > 0f)
                    {
                        Hacking();
                        Move();
                        CamClipping();
                        Jump();
                        WheelRotation();
                        Show_Press();
                        Sound_Effecet();
                    }
                }
                R.MoveRotation(Quaternion.Euler(0, transform.eulerAngles.y + Input.GetAxis("Mouse X"), 0));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            if (jump == 2)
            {
                jump = 0;
                R.velocity = new Vector3(0, R.velocity.y, 0);
                Audio._DRONE_SOUND_PLAY(Sound.E_Drone_Sound.LAND);
            }
        }
    }

    void Sound_Effecet()
    {
        if (NotMove())
        {
            R.velocity = new Vector3(0, R.velocity.y, 0);
            if (Audio.IS_PLAYING(Sound.E_Drone_Sound.FORWARD))
                Audio.STOP_PLAYING_DRONE(Sound.E_Drone_Sound.FORWARD);
            if (Audio.IS_PLAYING(Sound.E_Drone_Sound.BACKWARD))
                Audio.STOP_PLAYING_DRONE(Sound.E_Drone_Sound.BACKWARD);
        }
        else
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                if (!Audio.IS_PLAYING(Sound.E_Drone_Sound.FORWARD))
                {
                    Audio._DRONE_SOUND_PLAY(Sound.E_Drone_Sound.FORWARD);
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (!Audio.IS_PLAYING(Sound.E_Drone_Sound.BACKWARD))
                {
                    Audio._DRONE_SOUND_PLAY(Sound.E_Drone_Sound.BACKWARD);
                }
            }
        }
    }

    void Holding()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Hold = true;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            Hold = false;
        }
    }

    void ChangeViewCtrl()
    {
        if (On == 1)
        {
            if (!Hp.gameObject.activeInHierarchy)
                Hp.gameObject.SetActive(true);
            Hp.fillAmount = Mathf.MoveTowards(Hp.fillAmount, ChangeView.DroneHp, 0.02f);
            if (Hp.fillAmount == ChangeView.DroneHp)
            {
                On = 2;
            }
        }
        else if (On == 2)
        {
            if (Hp.fillAmount > 0f)
            {
                RecCtrl[3] += Mathf.Ceil(Time.deltaTime);
                if (RecCtrl[3] >= 99)
                {
                    RecCtrl[3] = 0f;
                    RecCtrl[2] += 1;
                    if (RecCtrl[2] >= 60)
                    {
                        RecCtrl[2] = 0f;
                        RecCtrl[1] += 1;
                        if (RecCtrl[1] >= 60)
                        {
                            RecCtrl[1] = 0f;
                            RecCtrl[0] += 1;
                        }
                    }
                }
                string[] s = new string[4] { RecCtrl[0] < 10 ? "0" + RecCtrl[0].ToString() : RecCtrl[0].ToString(), RecCtrl[1] < 10 ? "0" + RecCtrl[1].ToString() : RecCtrl[1].ToString(),
                    RecCtrl[2] < 10 ? "0" + RecCtrl[2].ToString() : RecCtrl[2].ToString(), RecCtrl[3] < 10 ? "0" + RecCtrl[3].ToString() : RecCtrl[3].ToString()};
                RecTime.text = s[0] + " : " + s[1] + " : " + s[2] + " : " + s[3];
                if (JumpCool.fillAmount == 1f || (JumpCool.transform.parent.gameObject.activeInHierarchy && JumpCool.fillAmount == 0))
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (jump == 0)
                        {
                            if (JumpCool.fillAmount != 1f) { JumpCool.fillAmount = 1f; }
                            jump = 1;
                            JumpCool.transform.parent.gameObject.SetActive(true);
                        }
                    }
                }
            }
            CtrlUI();
        }
        Drone();
    }

    void Drone()
    {
        if (ChangeView.Change == 2)
        {
            if (ChangeView.DroneHp > 0f)
            {
                if (On == 0) { On = 1; }
                Panel.color = new Color(Panel.color.r, Panel.color.g, Panel.color.b, Panel.color.a - 0.01f);
                HackCool(true);
                Press.color = new Color(Press.color.r, Press.color.g, Press.color.b, 0f);

                if (Panel.color.a < 0f)
                {
                    ChangeView.Change = 0;
                }
            }
            else
            {
                ChangeView.Change = 0;
            }
        }
        else if (ChangeView.Change == 3)
        {
            if (ChangeView.DroneHp > 0f)
            {
                Panel.color = new Color(Panel.color.r, Panel.color.g, Panel.color.b, Panel.color.a + 0.02f);
                if (Panel.color.a > 1f)
                {
                    On = 0;
                    Hp.fillAmount = 0;
                    ChangeView.Change = 4;
                }
            }
            else
            {
                On = 0;
                ChangeView.Change = 4;
            }
        }
    }

    void Move()
    {
        if (jump == 0)
        {
            R.velocity = transform.rotation * new Vector3(Input.GetAxis("Horizontal") * Speed, R.velocity.y, Input.GetAxis("Vertical") * Speed);
        }
    }

    void Jump()
    {
        if (jump == 1)
        {
            float Power = 40f;
            Vector3 Forward = Offset.forward;
            Forward.y *= Forward.y <= 0 ? 0.01f : 2f;
            jump = 2;
            if (Forward.y <= 0.01f) { Power = 90f; StartCoroutine(StopMove()); }
            R.AddForce(Forward * Power, ForceMode.Impulse);
            Audio._DRONE_SOUND_PLAY(Sound.E_Drone_Sound.JUMP);
        }
    }

    void WheelRotation()
    {
        if (Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f)
        {
            Wheel.Rotate(((Input.GetAxis("Vertical") != 0f) ? Input.GetAxis("Vertical") : Input.GetAxis("Horizontal")) * Speed, 0, 0);
        }
    }

    void CamClipping()
    {
        Offset.Rotate(-Input.GetAxis("Mouse Y"), 0, 0);
        if (Offset.localRotation.x < -0.3f) { Offset.localRotation = new Quaternion(-0.3f, Offset.localRotation.y, Offset.localRotation.z, Offset.localRotation.w); }
        if (Offset.localRotation.x > 0.3f) { Offset.localRotation = new Quaternion(0.3f, Offset.localRotation.y, Offset.localRotation.z, Offset.localRotation.w); }
    }

    void CtrlUI()
    {
        //Rec
        RecShowCtrlTime += Time.deltaTime;
        if (RecShowCtrlTime >= RecNotShowTime)
        {
            RecShowCtrlTime = 0f;
            Rec.SetActive(!Rec.activeInHierarchy);
            if (!Rec.activeInHierarchy)
            {
                RecNotShowTime = 0.7f;
            }
            else
            {
                RecNotShowTime = 1.5f;
            }
        }
        //Hp
        ChangeView.DroneHp = Hp.fillAmount -= 0.01f * Time.deltaTime;
        if (Hp.fillAmount < 0.2f)
        {
            if (Hp.fillAmount == 0f)
            {
                if (ChangeView.DroneHp != 0f)
                {
                    ChangeView.DroneHp = 0;
                }
                if (Panel.color.a < 1.1f)
                {
                    Panel.color = new Color(Panel.color.r, Panel.color.g, Panel.color.b, Panel.color.a + 0.02f);
                    Panel.transform.GetChild(0).GetComponent<Text>().color = new Color(Panel.transform.GetChild(0).GetComponent<Text>().color.r, Panel.transform.GetChild(0).GetComponent<Text>().color.g, Panel.transform.GetChild(0).GetComponent<Text>().color.b, Panel.transform.GetChild(0).GetComponent<Text>().color.a + 0.02f);
                }
            }
            else
            {
                HpCtrlTime += Time.deltaTime;
                if (HpCtrlTime > 2.1f)
                {
                    HpCtrlTime = 0f;
                    StartCoroutine(Clocking());
                }
            }
        }
        //Jump
        if (JumpCool.transform.parent.gameObject.activeInHierarchy)
        {
            JumpCool.fillAmount -= 0.7f * Time.deltaTime;

            if (JumpCool.fillAmount == 0)
            {
                JumpCool.color = new Color(JumpCool.color.r, JumpCool.color.g, JumpCool.color.b, JumpCool.color.a - 0.02f);
                Image C = JumpCool.transform.parent.GetComponent<Image>();
                C.color = new Color(C.color.r, C.color.g, C.color.b, C.color.a - 0.02f);
                if (C.color.a < 0f)
                {
                    JumpCool.transform.parent.gameObject.SetActive(false);
                    JumpCool.fillAmount = 1f;
                }
            }
            else
            {
                if (JumpCool.color.a < 1f)
                {
                    JumpCool.color = new Color(JumpCool.color.r, JumpCool.color.g, JumpCool.color.b, JumpCool.color.a + 0.01f);
                    Image C = JumpCool.transform.parent.GetComponent<Image>();
                    C.color = new Color(C.color.r, C.color.g, C.color.b, C.color.a + 0.01f);
                }
            }
        }
    }

    IEnumerator Clocking()
    {
        int count = 0;
        while (count < 4)
        {
            count++;
            Hp.gameObject.SetActive(!Hp.gameObject.activeInHierarchy);
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator StopMove()
    {
        yield return new WaitForSeconds(0.7f);
        if (jump != 0)
        {
            R.velocity = Vector3.zero;
            jump = 0;
        }
    }

    bool NotMove()
    {
        return (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A));
    }

    bool CanHacked()
    {
        RaycastHit Hit;
        int layerMask = ((1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Lock")));
        Debug.DrawRay(Offset.position, Offset.forward * 100f, Color.red);
        if (Physics.Raycast(Offset.position, Offset.forward, out Hit, 100f, layerMask))
        {
            if (Hit.collider != null && Hit.transform.gameObject.layer == 20)
            {
                if (Hack_I.fillAmount == 1f)
                {
                    Locked = Hit.transform;
                }
                return true;
            }
            return false;
        }
        return false;
    }

    void Hacking()
    {
        if (Hold)
        {
            if (CanHacked())
            {
                HackCool(false);
            }
            else
            {
                HackCool(true);
            }
        }
        else
        {
            if (Hack_I.fillAmount < 1f)
            {
                if (Hack_I.fillAmount > 0f) { Hack_I.fillAmount = Mathf.MoveTowards(Hack_I.fillAmount, 0f, 0.01f); }
            }
            HackCool(true);
        }
        if (Locked != null)
        {
            CheckName();
        }
    }

    void Show_Press()
    {
        if (CanHacked())
        {
            if(Press.text != "Hold E")
            {
                Press.text = "Hold E";
            } 
            if(Press.color.a < 1f)
            {
                Press.color = new Color(Press.color.r, Press.color.g, Press.color.b, Mathf.MoveTowards(Press.color.a, 1f, 0.1f));
            }
        }
        else
        {
            if (Press.color.a > 0f)
            {
                Press.color = new Color(Press.color.r, Press.color.g, Press.color.b, Mathf.MoveTowards(Press.color.a, 0f, 0.1f));
            }
        }
    }

    void HackCool(bool What)
    {
        if (What)
        {
            if (Hack_T.text != null)
            {
                Hack_T.text = "";
            }
            if(Hack_T.color.a > 0f) Hack_T.color = new Color(Hack_T.color.r, Hack_T.color.g, Hack_T.color.b, Hack_T.color.a - Hack_Alpha_Speed);
            if (Hack_I.color.a > 0f) Hack_I.color = new Color(Hack_I.color.r, Hack_I.color.g, Hack_I.color.b, Hack_I.color.a - Hack_Alpha_Speed);
            Hack_I.fillAmount = 0f;
            if (Hack_I.transform.parent.GetComponent<Image>().color.a > 0f) Hack_I.transform.parent.GetComponent<Image>().color = new Color(Hack_I.transform.parent.GetComponent<Image>().color.r, Hack_I.transform.parent.GetComponent<Image>().color.g, Hack_I.transform.parent.GetComponent<Image>().color.b, Hack_I.transform.parent.GetComponent<Image>().color.a - Hack_Alpha_Speed);
        }
        else
        {
            if (Hack_I.color.a >= 1f && Hack_T.color.a >= 1f)
            {
                Typing();
                if (Hack_I.fillAmount < 1f) { Hack_I.fillAmount = Mathf.MoveTowards(Hack_I.fillAmount, 1f, 0.01f); }
            }
            else
            {
                if (Hack_T.color.a < 1f) Hack_T.color = new Color(Hack_T.color.r, Hack_T.color.g, Hack_T.color.b, Hack_T.color.a + Hack_Alpha_Speed);
                if (Hack_I.color.a < 1f) Hack_I.color = new Color(Hack_I.color.r, Hack_I.color.g, Hack_I.color.b, Hack_I.color.a + Hack_Alpha_Speed);
                if (Hack_I.transform.parent.GetComponent<Image>().color.a < 1f) Hack_I.transform.parent.GetComponent<Image>().color = new Color(Hack_I.transform.parent.GetComponent<Image>().color.r, Hack_I.transform.parent.GetComponent<Image>().color.g, Hack_I.transform.parent.GetComponent<Image>().color.b, Hack_I.transform.parent.GetComponent<Image>().color.a + Hack_Alpha_Speed);
            }
        }
    }

    string CreateRandomCharacter(int count)
    {
        string s = "";
        for(int i=0; i<count; i++)
        {
            s += (char)Random.Range(32, 126);
        }
        return s;   
    }

    void Typing()
    {
        if (Hack_I.fillAmount < 1f)
        {
            if(!Audio.IS_PLAYING(Sound.E_Drone_Sound.HACKING))
            {
                Audio._DRONE_SOUND_PLAY(Sound.E_Drone_Sound.HACKING);
            }
            HackTime += Time.deltaTime;
            if (HackTime >= 0.08f)
            {
                Hack_T.text = CreateRandomCharacter(8);
                HackTime = 0f;
            }
        }
        else
        {
            if (Hack_T.text != "Complete")
            {
                string s = "Complete";
                int i = 0;
                char[] c = Hack_T.text.ToCharArray();
                if (Audio.IS_PLAYING(Sound.E_Drone_Sound.HACKING))
                {
                    Audio.STOP_PLAYING_DRONE(Sound.E_Drone_Sound.HACKING);
                }
                while (i < s.Length)
                {
                    try
                    {
                        c[i] = s[i];
                    }
                    catch(System.IndexOutOfRangeException ex)
                    {
                        c = s.ToCharArray();
                    }
                    i++;
                    Hack_T.text = new string(c);
                }
            }
        }
    }

    void CheckName()
    {
        if (Locked.name.Contains("door"))
        {
            Locked.GetComponent<Door>().Locked = false;
            Locked = null;
        }
        else
        {
            Locked = Locked.parent;
            CheckName();
        }
    }
}
