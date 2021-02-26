using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelePad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = new Vector3(-236.08f, 300.58f, -98.80438f);
    }
}
