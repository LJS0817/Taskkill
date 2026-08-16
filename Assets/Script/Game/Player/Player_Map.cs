using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Map : MonoBehaviour
{
    public Transform Player;
    public Transform StartPos;
    public int Stage;
    public int Up;

    public Alarm Ala;

    //public RenderTexture T;
    //private byte[] imageByte;

    //public Camera[] Cam;

    private void Start()
    {
        Up = 2;
        Stage = -1;
        //try
        //{
        //    System.IO.File.Delete("D:/Project/Project(2)/Assets/Sprite/Game/TV/CCTV/");
        //}catch(System.IO.DirectoryNotFoundException ex)
        //{

        //}
    }

    private void Update()
    {
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);

    //        Camera activeCamera = Cam[2];

    //        // Initialize and render
    //        RenderTexture rt = new RenderTexture(T.width, T.height, 24);
    //        activeCamera.targetTexture = rt;
    //        activeCamera.Render();
    //        RenderTexture.active = rt;

    //        // Read pixels
    //        tex.ReadPixels(new Rect(0, 0, T.width, T.height), 0, 0);
    //        tex.Apply();
    //        imageByte = tex.EncodeToPNG();

    //        // Clean up
    //        activeCamera.targetTexture = T;
    //        RenderTexture.active = null; // added to avoid erro
    //        DestroyImmediate(rt);

    //        System.IO.File.WriteAllBytes("D:/Project/Project (2)/Assets/Sprite/Game/TV/CCTV/" + Time.time.ToString() + ".png", imageByte);
    //    }
    //}

    public void Stage_Up()
    {
        if(Up == 2)
        {
            if (Stage == StartPos.childCount - 1)
            {
                Up = 4;
                return;
            }
            else
            {
                Up = 3;
            }
            if (Stage > -1)
                StartPos.GetChild(Stage).gameObject.SetActive(false);
            ++Stage;
            StartPos.GetChild(Stage).gameObject.SetActive(true);

            Player.position = StartPos.GetChild(Stage).GetChild(3).GetChild(0).position;
            Player.rotation = StartPos.GetChild(Stage).GetChild(3).GetChild(0).rotation;
        }
    }

    public bool Check_Fire(bool Gun)
    {
        if (Input.GetMouseButtonUp(0) && Gun)
        {
            if (!Ala.Alert && Stage == 2)
            {
                Ala.Alert = true;
                Ala.Sound.Play();
                return true;
            }
        }
        return false;
    }
}
