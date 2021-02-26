using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkuBlocku : MonoBehaviour
{
    public GameObject blocks;
    public float speed;

    private void Update()
    {
        if(GetComponent<LinkuStartu>().started)
        {
            blocks.transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }
}
