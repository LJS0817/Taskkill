using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    const int idx_3D = 2;
    const int idx_Drone = 5;

    public enum E_Sound
    {
        WALK,
        FIRE = 2,
        RELOAD_OUT,
        RELOAD_IN,
        RELOAD,
        LOW_POWER,
        UNLOCK,
        LOCK,
        SIT,
        CROUCH,
        GET_UP,
        PICK
    };

    public enum E_3D_Sound
    {
        GLASS,
        METAL
    }

    public enum E_Drone_Sound
    {
        FORWARD,
        BACKWARD,
        JUMP,
        LAND,
        HACKING
    }

    AudioSource[] Audio;
    AudioSource[] Audio_3D;
    AudioSource[] Drone_Audio;
    public AudioSource[] Before_Play;

    public Transform S;
    [HideInInspector]
    public AudioSource[] A;
    int Idx;

    int Stage;

    private void Start()
    {
        A = new AudioSource[S.childCount];
        for (int i = 0; i < A.Length; i++)
        {
            A[i] = S.GetChild(i).GetComponent<AudioSource>();
        }
        Idx = 0;
        A[Idx].Play();

        Audio = new AudioSource[transform.childCount - idx_3D - idx_Drone];
        for (int i = 0; i < Audio.Length; i++)
        {
            Audio[i] = transform.GetChild(i).GetComponent<AudioSource>();
        }

        Audio_3D = new AudioSource[idx_3D];
        for (int i = 0; i < Audio_3D.Length; i++)
        {
            Audio_3D[i] = transform.GetChild(i + Audio.Length).GetComponent<AudioSource>();
        }

        Drone_Audio = new AudioSource[idx_Drone];
        for (int i = 0; i < Drone_Audio.Length; i++)
        {
            Drone_Audio[i] = transform.GetChild(i + Audio.Length + Audio_3D.Length).GetComponent<AudioSource>();
        }
    }

    public void SOUND_UPDATE()
    {
        if (Stage < 5)
        {
            if (!A[Idx].isPlaying)
            {
                if (Idx >= A.Length - 1)
                {
                    Idx = 0;
                    A[Idx].Play();
                }
                else
                {
                    ++Idx;
                    A[Idx].Play();
                }
            }
        }
        else
        {
            if (A[Idx].isPlaying)
            {
                A[0].volume = A[1].volume = A[2].volume = Mathf.MoveTowards(A[2].volume, 0f, 0.02f);
                if (A[Idx].volume <= 0f)
                {
                    A[Idx].Stop();
                    A[3].Play();
                }
            }
            if (A[3].isPlaying)
            {
                if (A[3].volume < 1f)
                {
                    A[3].volume = Mathf.MoveTowards(A[3].volume, 1f, 0.02f);
                }
            }
        }
    }

    public void PLAY_BACKGROUND_MUSIC(int _Stage)
    {
        Stage = _Stage;
    }

    public void PLAY_SOUND(E_Sound s)
    {
        switch (s)
        {
            case E_Sound.WALK:
                Audio[Random.Range((int)E_Sound.WALK, (int)E_Sound.FIRE)].Play();
                break;
            default:
                Audio[(int)s].Play();
                break;
        }
    }

    public void PLAY_BEFORE_SOUND(int n)
    {
        Before_Play[n].Play();
    }

    public void _3D_SOUND_PLAY(E_3D_Sound s, Vector3 Pos)
    {
        Audio_3D[(int)s].transform.position = Pos;
        Audio_3D[(int)s].Play();
    }

    public void _DRONE_SOUND_PLAY(E_Drone_Sound s)
    {
        Drone_Audio[(int)s].Play();
    }

    public void STOP_PLAYING_DRONE(E_Drone_Sound s)
    {
        Drone_Audio[(int)s].Stop();
    }

    public bool IS_PLAYING(E_Drone_Sound s)
    {
        return Drone_Audio[(int)s].isPlaying;
    }

    public bool IS_PLAYING(E_3D_Sound s)
    {
        return Audio_3D[(int)s].isPlaying;
    }

    public bool IS_PLAYING(E_Sound s)
    {
        switch (s)
        {
            case E_Sound.WALK:
                return Audio[Random.Range((int)E_Sound.WALK, (int)E_Sound.FIRE)].isPlaying;
            default:
                return Audio[(int)s].isPlaying;
        }
    }
}
