using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject CCTV_Audio;
    public Sound Audio;
    bool Shot;
    public GameObject Cartridge;
    public Transform CartridgeParent;
    public Transform CartridgeNextParent;
    public Transform RayOffset;

    public GameObject Bullet_Hole;

    public GameObject Spark;
    public Transform SparkParent;

    public GameObject Glass;

    [HideInInspector]
    public Vector3 Knife_Pos;
    [HideInInspector]
    public bool Knife;
    [HideInInspector]
    public Vector3 Knife_Rot;
    [HideInInspector]
    public Transform target;

    [HideInInspector]
    public bool Dead_Enemy;

    Vector3 Dir;

    public GameObject Feather;
    public GameObject Wood;

    private void Start()
    {
        Knife = false;
        Shot = false;
        Dead_Enemy = false;
        Dir = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (!ChangeView.Pause)
        {
            if (Shot)
            {
                RaycastHit Hit;
                int layerMask = ((1 << LayerMask.NameToLayer("Scanned_Enemy")) | (1 << LayerMask.NameToLayer("Knife")) | (1 << LayerMask.NameToLayer("ammo")) | (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Gun")) | (1 << LayerMask.NameToLayer("Door")));
                layerMask = ~layerMask;
                if (Physics.Raycast(RayOffset.position, RayOffset.forward, out Hit, Mathf.Infinity, layerMask))
                {
                    Debug.DrawRay(RayOffset.position, RayOffset.forward * 100, Color.red);
                    if (Hit.collider != null)
                    {
                        Dir = Hit.point;
                        Shot = false;
                        if (Hit.transform.gameObject.tag != "Wood")
                        {
                            GameObject G = Instantiate(Spark, Hit.point, Quaternion.FromToRotation(Vector3.forward, Hit.normal), SparkParent);
                            Destroy(G, 4f);

                            if (Hit.transform.gameObject.tag != "Glass" && Hit.transform.gameObject.tag != "Pillow")
                                Audio._3D_SOUND_PLAY(Sound.E_3D_Sound.METAL, Hit.point);

                            Create_Bullet_Hole(Hit);
                        }

                        CCTV_Hit(Hit);
                        Enemy(Hit);
                        Hit_Light(Hit);
                        Object_Hit(Hit);
                        return;
                    }
                }
                Shot = false;
            }
        }
    }

    public void isShot()
    {
        Shot = true;
    }

    public void CreateCartridge()
    {
        GameObject G = Instantiate(Cartridge, CartridgeParent.position, CartridgeParent.localRotation, CartridgeParent);
        G.GetComponent<Rigidbody>().AddForce((((CartridgeParent.right * -1) - CartridgeParent.up) * Random.Range(0.3f, 0.7f))* -20, ForceMode.Impulse);
        G.transform.parent = CartridgeNextParent;
    }

    public void Create_Bullet_Hole(RaycastHit Hit)
    {
        if (!GameObject.Find("Player").GetComponent<Player>().Kill.Kill)
        {
            GameObject BH = Instantiate(Bullet_Hole, Hit.point + Hit.normal * 0.001f, Quaternion.FromToRotation(Vector3.up, Hit.normal), SparkParent);
            BH.transform.localScale = new Vector3(0.05481542f, 0.05481542f, 0.05481542f);

            BH.transform.parent = Hit.transform;
            Destroy(BH, 7f);
        }
    }

    void CCTV_Hit(RaycastHit Hit)
    {
        if (Hit.transform.gameObject.layer == 10)
        {
            Player_UI.Hit = true;
            Audio._3D_SOUND_PLAY(Sound.E_3D_Sound.METAL, Hit.point);
            if (Hit.transform.name == "body")
            {
                if (!Hit.transform.parent.parent.GetComponent<CCTV>().Off)
                {
                    Hit.transform.parent.parent.GetComponent<CCTV>().Off = true;
                    Hit.transform.parent.parent.GetComponent<CCTV>().Light.gameObject.SetActive(false);
                    Hit.transform.parent.parent.GetComponent<CCTV>().Light_Offset.gameObject.SetActive(false);
                    Hit.transform.parent.parent.GetComponent<CCTV>().Smoke.Play();
                    GameObject g = Instantiate(CCTV_Audio, Hit.transform.parent.position, Quaternion.identity, Hit.transform.parent.parent);
                    g.GetComponent<AudioSource>().Play();
                    Destroy(g, 7f);
                }
            }
            else if (Hit.transform.name == "head")
            {
                if (!Hit.transform.parent.parent.parent.GetComponent<CCTV>().Off)
                {
                    Hit.transform.parent.parent.parent.GetComponent<CCTV>().Off = true;
                    Hit.transform.parent.parent.parent.GetComponent<CCTV>().Light.gameObject.SetActive(false);
                    Hit.transform.parent.parent.parent.GetComponent<CCTV>().Light_Offset.gameObject.SetActive(false);
                    Hit.transform.parent.parent.parent.GetComponent<CCTV>().Smoke.Play();
                    GameObject g = Instantiate(CCTV_Audio, Hit.transform.parent.parent.parent.position, Quaternion.identity, Hit.transform.parent.parent.parent.parent);
                    g.GetComponent<AudioSource>().Play();
                    Destroy(g, 7f);
                }
            }
            else if (Hit.transform.name == "Tail")
            {
                if (!Hit.transform.parent.GetComponent<CCTV>().Off)
                {
                    Hit.transform.parent.GetComponent<CCTV>().Off = true;
                    Hit.transform.parent.GetComponent<CCTV>().Light.gameObject.SetActive(false);
                    Hit.transform.parent.GetComponent<CCTV>().Light_Offset.gameObject.SetActive(false);
                    Hit.transform.parent.GetComponent<CCTV>().Smoke.Play();
                    GameObject g = Instantiate(CCTV_Audio, Hit.transform.parent.position, Quaternion.identity, Hit.transform.parent.parent);
                    g.GetComponent<AudioSource>().Play();
                    Destroy(g, 7f);
                }

            }
        }
    }

    void Enemy(RaycastHit Hit)
    {
        if (Hit.transform.gameObject.layer == 17)
        {
            Player_UI.Hit = true;
            Hit.transform.GetComponent<Auto_Hp>().Sync_Hp();
            Dead_Enemy = Hit.transform.GetComponent<Auto_Hp>().Hp.Hp <= 0f ? true : false;
        }
    }

    void Hit_Light(RaycastHit Hit)
    {
        if(Hit.transform.gameObject.tag == "Light")
        {
            if(!Hit.transform.GetComponent<Top_Light>().Off)
            {
                GameObject G = Instantiate(Glass, Hit.point, Quaternion.FromToRotation(Vector3.forward, Hit.normal), Hit.transform);
                Destroy(G, 5f);
                Hit.transform.GetComponent<Top_Light>().Off = true;
                Audio._3D_SOUND_PLAY(Sound.E_3D_Sound.GLASS, Hit.point);
            }
        }
    }

    void Object_Hit(RaycastHit Hit)
    {
        if (Hit.transform.gameObject.tag == "Glass")
        {
            GameObject G = Instantiate(Glass, Hit.point, Quaternion.FromToRotation(Vector3.forward, Hit.normal), Hit.transform);
            Destroy(G, 5f);
            Audio._3D_SOUND_PLAY(Sound.E_3D_Sound.GLASS, Hit.point);
        }
        if (Hit.transform.gameObject.tag == "Pillow")
        {
            GameObject G = Instantiate(Feather, Hit.point, Quaternion.FromToRotation(Vector3.forward, Hit.normal), SparkParent);
            Destroy(G, 10f);
        }
        if (Hit.transform.gameObject.tag == "Wood")
        {
            GameObject G = Instantiate(Wood, Hit.point, Quaternion.FromToRotation(Vector3.forward, Hit.normal), SparkParent);
            Destroy(G, 10f);
        }
        if(Hit.transform.gameObject.layer == 23)
        {
            Hit.transform.parent.GetChild(0).position = Hit.transform.position;
            Hit.transform.parent.GetChild(0).gameObject.SetActive(true);
            Transform Pa = Hit.transform.parent;
            Destroy(Pa.gameObject, 10);
            Destroy(Hit.transform.gameObject);
        }
    }
}
