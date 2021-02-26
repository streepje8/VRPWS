using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.localScale = GameObject.Find("InventoryGUI").transform.localScale;
    }
}
