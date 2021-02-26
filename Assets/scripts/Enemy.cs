using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HP;
    private int totalHP = 0;


    // Start is called before the first frame update
    void Start()
    {
        totalHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
