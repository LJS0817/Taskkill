using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anything : MonoBehaviour
{
    public Player_UI ui;

    public void Weapon_Change()
    {
        ui.Change_Layer();
    }
}
