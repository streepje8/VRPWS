using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private Vector3 startPOS;
    public float bumpdistance;

    void Start()
    {
        startPOS = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 B = startPOS;
        B.y += Mathf.Sin(Time.time) * bumpdistance;
        transform.position = B;
    }
}
