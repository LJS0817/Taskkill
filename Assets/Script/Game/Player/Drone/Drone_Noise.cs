using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drone_Noise : MonoBehaviour
{
    public float scrollSpeed;
    Vector3 savedOffset;
    Image render;

    void Start()
    {
        render = GetComponent<Image>();
        savedOffset = render.material.mainTextureOffset;
    }

    void Update()
    {
        if (!ChangeView.Pause)
        {
            float y = Mathf.Repeat(Time.time * scrollSpeed, 1);
            Vector2 offset = new Vector2(savedOffset.x, y);
            render.material.SetTextureOffset("_MainTex", offset);
        }
    }

    void OnDisable()
    {
        render.material.SetTextureOffset("_MainTex", savedOffset);
    }
}
