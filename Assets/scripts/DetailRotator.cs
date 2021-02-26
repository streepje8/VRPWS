﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailRotator : MonoBehaviour
{
    public Vector3 rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
