using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_Sync : MonoBehaviour
{
    Image P;
    Text T;
    public Image I;
    public AudioSource Audio;

    // Start is called before the first frame update
    void Start()
    {
        if(transform.parent != null)
            P = transform.parent.GetComponent<Image>();
        T = GetComponent<Text>();
    }

    private void Update()
    {

    }

    public void Sync(bool Press)
    {
        if (Press)
        {
            if (P != null)
                T.color = P.color;
        }
        else
        {
            if(T.name != "Dont")
                Audio.Play();
            T.color = Color.white;
            if(T.name == "Start")
            {
                Logo.Loading = true;
            }
            else if (T.name == "Setting" || T.name == "Credits" || T.name == "Exit")
            {
                I.gameObject.SetActive(true);
            }
            else if (T.name == "Keep")
            {
                I.gameObject.SetActive(false);
            }
            else if(T.name == "Yes")
            {
                Application.Quit();
            }
            else
            {
                return;
            }
        }
    }
}
