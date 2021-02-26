using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterRotation : MonoBehaviour
{
    public Vector3 Rotationspeed;

    private void Start()
    {
        transform.Rotate(90, 0, 0);
    }
    void Update()
    {
        transform.Rotate(Rotationspeed * Time.deltaTime);
    }
}
