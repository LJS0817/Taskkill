using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{
    public Option op;

    public Animator GunAni;
    public Text Hp;
    public Image DroneHp;
    public Text NowBullet;
    public TextMesh Bullet_Text;

    [HideInInspector]
    public int Bullet_Lit;
    public static readonly int Bullet = 9;
    [HideInInspector]
    public int Bullet_Now;
    int Before_reload;

    public Text Number;

    public float Player_Hp = 1f;

    public Image Some;
    float Some_Time;

    public Image HP_Image;

    public GameObject Low;
    bool Low_Change;

    public Image See;

    public Text Press_E;

    public Text Loading;

    public static bool Hit;
    public Image Hit_Img;

    public Image Tap;
    public Text Mission;

    readonly string[] mission = { "DO YOUR BEST", "KEEP STEALTH", "YOU CAN BE A ASSASSIN", "BEST TIMING", "ON AIR", "THIS IS WHY YOUR HERO", "END THIS GAME" };

    public Animator Knife;
    public Animator Pistol;

    public Transform Change_Image;
    [HideInInspector]
    public static bool Loading_done;

    private void Start()
    {
        Some_Time = 0f;
        Low_Change = true;
        Hit = false;
        //Loading_done = true;

        if (Loading_done)
        {
            Loading.transform.parent.gameObject.SetActive(false);
        }

        Bullet_Lit = 18;
        Bullet_Now = Bullet;

        VALUE.Number = Random.Range(1, 10000);
        Number.text = "No." + VALUE.Number.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Loading_done)
        {
            if (!ChangeView.Pause)
            {
                if (!ChangeView.Drone)
                {
                    Battery();
                    if (See.color.a > 0f)
                    {
                        See.color = new Color(See.color.r, See.color.g, See.color.b, Mathf.MoveTowards(See.color.a, 0f, 0.004f));
                    }
                    if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        Tap.gameObject.SetActive(true);
                    }
                    if (Input.GetKeyUp(KeyCode.Tab))
                    {
                        Tap.gameObject.SetActive(false);
                    }
                }
            }
        }
        if (GunAni.gameObject.activeInHierarchy)
        {
            if (GunAni.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Cant_Shot" && Bullet_Now > 0)
            {
                SetText();
            }
        }
    }

    public void Change_Weapon()
    {
        if (Knife.GetInteger("Change") == 0)
        {
            Knife.SetInteger("Change", 2);
        }
        else if (Knife.GetInteger("Change") == 2)
        {
            Knife.SetInteger("Change", 4);
        }
        else if (Knife.GetInteger("Change") == 4)
        {
            Knife.SetInteger("Change", 3);
        }
        else 
        {
            Knife.SetInteger("Change", 4);
        }

        if (Pistol.GetInteger("Change") == 0)
        {
            Pistol.SetInteger("Change", 1);
        }
        else if (Pistol.GetInteger("Change") == 1)
        {
            Pistol.SetInteger("Change", 3);
        }
        else if (Pistol.GetInteger("Change") == 3)
        {
            Pistol.SetInteger("Change", 4);
        }
        else
        {
            Pistol.SetInteger("Change", 3);
        }
    }

    public void Change_Layer()
    {
        if (Pistol.GetInteger("Change") == 4 || Pistol.GetInteger("Change") == 1)
        {
            Knife.transform.SetParent(Change_Image.parent);
            Knife.transform.SetParent(Change_Image);
        }
        else
        {
            Pistol.transform.SetParent(Change_Image.parent);
            Pistol.transform.SetParent(Change_Image);
        }
    }

    public void Set_Mission(int stage)
    {
        Mission.text = "CHAPTER" + "\n" + "\n" + mission[stage];
    }

    void Battery()
    {
        //Character
        Player_Hp = Mathf.MoveTowards(Player_Hp, 0f, 0.000075f);
        int nHp = (int)(Player_Hp * 100);
        Hp.text = "Power :  " + nHp.ToString() + " %";
        HP_Image.color = new Color(Mathf.MoveTowards(HP_Image.color.r, 1f - Player_Hp, 0.01f), Mathf.MoveTowards(HP_Image.color.g, Player_Hp, 0.01f), Mathf.MoveTowards(HP_Image.color.b, Player_Hp, 0.01f), HP_Image.color.a);
        //Drone
        DroneHp.fillAmount = ChangeView.DroneHp = Mathf.MoveTowards(ChangeView.DroneHp, 1f, 0.000002f);
        //Some
        Some_Time += Time.deltaTime;
        if(Some_Time >= 1f)
        {
            Some_Time = 0f;
            if (Some.fillAmount < 1f)
            {
                Some.fillAmount += 0.25f;
            }
            else
            {
                Some.fillAmount = 0f;
            }
        }
        //Low
        if (Player_Hp <= 0.15f)
        {
            Color c;
            c = Low.transform.GetChild(3).GetComponent<Text>().color;
            c.a += Low_Change ? 0.008f : -0.008f;
            Low.transform.GetChild(3).GetComponent<Text>().color = Low.transform.GetChild(2).GetComponent<Text>().color = Low.transform.GetChild(1).GetComponent<Image>().color = Low.transform.GetChild(0).GetComponent<Image>().color = c;

            if (Low.transform.GetChild(Low.transform.childCount - 1).GetComponent<Text>().color.a < 0f || Low.transform.GetChild(Low.transform.childCount - 1).GetComponent<Text>().color.a > 0.5f)
            {
                Low_Change = !Low_Change;
            }
        }
        else
        {
            if (Low.transform.GetChild(Low.transform.childCount - 1).GetComponent<Text>().color.a >= 0f)
            {
                Color c;
                c = Low.transform.GetChild(3).GetComponent<Text>().color;
                c.a += -0.008f;
                Low.transform.GetChild(3).GetComponent<Text>().color = Low.transform.GetChild(2).GetComponent<Text>().color = Low.transform.GetChild(1).GetComponent<Image>().color = Low.transform.GetChild(0).GetComponent<Image>().color = c;
            }
        }

        if (Hit)
        {
            Hit_Img.color = new Color(Hit_Img.color.r, Hit_Img.color.g, Hit_Img.color.b, Mathf.MoveTowards(Hit_Img.color.a, 1f, 0.1f));
            Vector3 r = Hit_Img.rectTransform.localEulerAngles;
            r.z += 4.5f;
            Hit_Img.rectTransform.localEulerAngles = r;
            if (Hit_Img.color.a >= 1f)
            {
                Hit = false;
            }
        }
        else
        {
            if (Hit_Img.color.a >= 0f)
            {
                Hit_Img.color = new Color(Hit_Img.color.r, Hit_Img.color.g, Hit_Img.color.b, Mathf.MoveTowards(Hit_Img.color.a, 0f, 0.1f));
                if(Hit_Img.color.a <= 0.05f)
                {
                    Hit_Img.rectTransform.localEulerAngles = Vector3.zero;
                }
            }
        }
    }

    public void Shooting()
    {
        if (Bullet_Now > 0)
        {
            Bullet_Now -= 1;
            SetText();
        }
    }

    public void SetText()
    {
        GunAni.SetInteger("Bullet", Bullet_Now);
        NowBullet.text = Bullet_Now.ToString() + " \\ " + Bullet_Lit.ToString();
        Bullet_Text.text = Bullet_Now.ToString();
    }

    public void BulletReset()
    {
        Before_reload = Bullet_Now;
        Bullet_Now = 0;
        SetText();
    }

    public void Reload()
    {
        if (Bullet_Lit > 0)
        {
            if (Bullet_Lit + Before_reload >= Bullet)
            {
                Bullet_Lit -= (Bullet - Before_reload);
                Bullet_Now = Bullet;
            }
            else
            {
                Bullet_Now = Bullet_Lit + Before_reload;
                Bullet_Lit -= Bullet_Lit;
            }
            SetText();
        }
    }

    public void Recovery_Hp()
    {
        if (Player_Hp < 0.9f)
        {
            Player_Hp += 0.10f;
        }
        else
        {
            Player_Hp = 1f;
        }
    }
}