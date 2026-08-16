using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartridge : MonoBehaviour
{
    bool Turn;
    // Start is called before the first frame update
    void Start()
    {
        Turn = true;
        StartCoroutine(Disappear());
    }

    void Update()
    {
        if(Turn)
            transform.Rotate(new Vector3(25, 25, 25));
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            Turn = false;
        }
    }
}
