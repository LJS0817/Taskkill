using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    AudioSource Audio;

    public Dropdown Mode;
    public Dropdown DP;
    public Dropdown RE_DP;
    public Dropdown Audio_Mode;

    Color c;
    bool ChangeColor;

    public Text Dis;
    public GameObject D;

    public Text Au;
    public GameObject A;

    public Slider BGM;

    public Image ui;

    public GameObject Setting_Image;
    public GameObject Setting;

    public Image Done;
    public Image Done_Aim;

    public GameObject Reticle;

    public Dropdown Reticle_Type;

    public GameObject Aim;
    [HideInInspector]
    public Image[] Vary_Aim;
    [HideInInspector]
    public int AimIndex;

    public Dropdown Aim_Color;
    readonly string[] color = { "white", "green", "yellow", "cyan" };

    public GameObject Quit;

    public GameObject Credit;

    Color STRING_COLOR(string s)
    {
        return (Color)typeof(Color).GetProperty(s.ToLowerInvariant()).GetValue(null, null);
    }

    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();

        ChangeColor = true;
        c = Dis.color;

        DP.options.Clear();
        for (int i = 0; i < QualitySettings.names.Length; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = QualitySettings.names[i];
            DP.options.Add(option);
        }
        DP.value = QualitySettings.GetQualityLevel();

        RE_DP.options.Clear();
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = Screen.resolutions[i].width.ToString() + " X " + Screen.resolutions[i].height.ToString();
            RE_DP.options.Add(option);
            if (Screen.resolutions[i].width == Screen.width && Screen.resolutions[i].height == Screen.height)
            {
                RE_DP.value = i;
            }
        }

        Mode.options.Clear();
        for (int i = 0; i < 4; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = ((FullScreenMode)i).ToString();
            Mode.options.Add(option);
        }
        Mode.value = (int)Screen.fullScreenMode;

        Audio_Mode.options.Clear();
        for (int i = 1; i < 8; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            string s = ((AudioSpeakerMode)i).ToString();
            s = s.Replace("Mode", "");
            option.text = s.Replace("point", ".");
            Audio_Mode.options.Add(option);
        }
        Audio_Mode.value = (int)AudioSettings.GetConfiguration().speakerMode;

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameScene")
        {
            Aim_Color.options.Clear();
            for (int i = 0; i < color.Length; i++)
            {
                Dropdown.OptionData option = new Dropdown.OptionData();
                option.text = color[i];
                Aim_Color.options.Add(option);
            }
            Aim_Color.value = 0;

            AimIndex = 0;

            Reticle_Type.options.Clear();
            string[] _s = { "Default", "CrossHairs", "Attached CrossHairs" };
            for (int i = 0; i < 3; i++)
            {
                Dropdown.OptionData option = new Dropdown.OptionData();
                option.text = _s[i];
                Reticle_Type.options.Add(option);
            }
            Reticle_Type.value = 0;
            Vary_Aim = new Image[_s.Length];
            for (int i = 0; i < Vary_Aim.Length; i++)
            {
                Vary_Aim[i] = Aim.transform.GetChild(i).GetComponent<Image>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameScene")
        {
            if (Player_UI.Loading_done)
            {
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    if (!Setting_Image.activeInHierarchy)
                    {
                        Setting_Image.SetActive(true);
                        ChangeView.Pause = true;
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                        Player_Sound();
                    }
                    else
                    {
                        Setting_Done();
                    }
                }
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Setting_Done();
            }
        }
        if (Setting_Image.activeInHierarchy)
        {
            if (ChangeColor)
            {
                if (D.activeInHierarchy && Dis.color.a != 1f)
                {
                    Dis.color = new Color(Dis.color.r, Dis.color.g, Dis.color.b, Mathf.MoveTowards(Dis.color.a, 1f, 0.03f));
                }
                else if (Dis.color.a != 0.4f)
                {
                    Dis.color = new Color(Dis.color.r, Dis.color.g, Dis.color.b, Mathf.MoveTowards(Dis.color.a, 0.4f, 0.03f));
                }
                if (A.activeInHierarchy && Au.color.a != 1f)
                {
                    Au.color = new Color(Au.color.r, Au.color.g, Au.color.b, Mathf.MoveTowards(Au.color.a, 1f, 0.03f));
                }
                else if (Au.color.a != 0.4f)
                {
                    Au.color = new Color(Au.color.r, Au.color.g, Au.color.b, Mathf.MoveTowards(Au.color.a, 0.4f, 0.03f));
                }
                ui.color = new Color(Mathf.MoveTowards(ui.color.r, c.r, 0.02f), Mathf.MoveTowards(ui.color.g, c.g, 0.02f), Mathf.MoveTowards(ui.color.b, c.b, 0.02f), 1f);
                Done.color = new Color(Mathf.MoveTowards(Done.color.r, c.r, 0.02f), Mathf.MoveTowards(Done.color.g, c.g, 0.02f), Mathf.MoveTowards(Done.color.b, c.b, 0.02f), 1f);
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameScene")
                {
                    Done_Aim.color = new Color(Mathf.MoveTowards(Done_Aim.color.r, c.r, 0.02f), Mathf.MoveTowards(Done_Aim.color.g, c.g, 0.02f), Mathf.MoveTowards(Done_Aim.color.b, c.b, 0.02f), 1f);
                }
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameScene")
                {
                    if (ui.color == c && Done.color == c && Done_Aim.color == c)
                    {
                        ChangeColor = false;
                    }
                }
                else
                {
                    if (ui.color == c && Done.color == c)
                    {
                        ChangeColor = false;
                    }
                }
            }
        }
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameScene")
        {
            if (Reticle.activeInHierarchy)
            {
                if (Vary_Aim[AimIndex].color.r != STRING_COLOR(color[Aim_Color.value]).r || Vary_Aim[AimIndex].color.g != STRING_COLOR(color[Aim_Color.value]).g
                    || Vary_Aim[AimIndex].color.b != STRING_COLOR(color[Aim_Color.value]).b)
                {
                    Vary_Aim[AimIndex].color = new Color(STRING_COLOR(color[Aim_Color.value]).r, STRING_COLOR(color[Aim_Color.value]).g,
                        STRING_COLOR(color[Aim_Color.value]).b, Vary_Aim[AimIndex].color.a);
                }
            }
        }
    }


    public void SetQuality()
    {
        QualitySettings.SetQualityLevel(DP.value, true);
        if(Setting_Image.activeInHierarchy)
            Player_Sound();
    }

    public void Resolution()
    {
        Screen.SetResolution(Screen.resolutions[RE_DP.value].width, Screen.resolutions[RE_DP.value].height, (FullScreenMode)Mode.value);
        if (Setting_Image.activeInHierarchy)
            Player_Sound();
    }

    public void Audio_Mode_Change()
    {
        AudioConfiguration a = AudioSettings.GetConfiguration();
        a.speakerMode = (AudioSpeakerMode)(Audio_Mode.value + 1);
        AudioSettings.Reset(a);
        if (Setting_Image.activeInHierarchy)
            Player_Sound();
    }

    public void Change_Audio()
    {
        AudioListener.volume = BGM.value;
    }


    public void Setting_Done()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameScene")
        {
            if (!Setting.activeInHierarchy && !Reticle.activeInHierarchy && Setting_Image.activeInHierarchy)
            {
                Player_Sound();
                Setting_Image.SetActive(false);
                ChangeView.Pause = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                return;
            }
            else if (Setting.activeInHierarchy && !Reticle.activeInHierarchy)
            {
                Player_Sound();
                Setting.SetActive(false);
                return;
            }
            else if (Setting.activeInHierarchy && Reticle.activeInHierarchy)
            {
                Player_Sound();
                Reticle.SetActive(false);
                return;
            }
        }
        else
        {
            if (Setting_Image.activeInHierarchy)
            {
                Player_Sound();
                Setting_Image.SetActive(false);
                return;
            }
            else if(Credit.activeInHierarchy)
            {
                Credit.SetActive(false);
                Player_Sound();
                return;
            }
            else
            {
                Quit.SetActive(!Quit.activeInHierarchy);
                Player_Sound();
                return;
            }
        }
    }

    public void UI_CLICK(bool _display)
    {
        Player_Sound();
        if (_display)
        {
            c = Dis.color;
            D.SetActive(true);
            A.SetActive(false);
        }
        else
        {
            D.SetActive(false);
            A.SetActive(true);
            c = Au.color;
        }
        ChangeColor = true;
    }

    public void Set_Menu()
    {
        Player_Sound();
        Reticle.SetActive(true);
    }

    public void ChooseWhat()
    {
        Player_Sound();
        if (AimIndex != Reticle_Type.value)
        {
            Vary_Aim[AimIndex].gameObject.SetActive(false);
            Vary_Aim[Reticle_Type.value].gameObject.SetActive(true);
            AimIndex = Reticle_Type.value;
        }
    }

    public void Click_Menu(int n)
    {
        if (n == 0)
        {
            Setting_Image.SetActive(false);
            ChangeView.Pause = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Player_Sound();
        }
        else if (n == 1)
        {
            Setting.SetActive(true);
            Player_Sound();
        }
        else
        {
            Player_Sound();
            Application.Quit();
        }
    }

    public void Player_Sound()
    {
        Audio.Play();
    }
}
