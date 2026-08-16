using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public Image S;
    bool IsDone = false;
    float T = 0f;
    AsyncOperation OP;


    public Light TV;
    public Renderer TV_light;

    float T_Light = 0f;
    float Limit_T;
    void Start()
    {
        StartCoroutine(StartLoad("GameScene"));

        Limit_T = Random.Range(0.1f, 1f);
    }

    void Update()
    {
        if (IsDone)
        {
            T += Time.deltaTime;
            S.fillAmount = T;

            if (T >= 5)
            {
                OP.allowSceneActivation = true;
            }

            T_Light += Time.deltaTime;
            if (T_Light >= Limit_T)
            {
                T_Light = 0f;
                Limit_T = Random.Range(0.1f, 1f);
                TV_light.material.SetColor("_EmissionColor", new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 1f));
                TV.color = TV_light.material.GetColor("_EmissionColor");
            }
        }
    }

    public IEnumerator StartLoad(string strSceneName)
    {
        yield return new WaitForSeconds(0.7f);

        OP = SceneManager.LoadSceneAsync(strSceneName);
        OP.allowSceneActivation = false;

        if (IsDone == false)
        {
            IsDone = true;

            while (OP.progress < 0.9f)
            {
                S.fillAmount = OP.progress;

                yield return true;
            }
        }
    }
}
