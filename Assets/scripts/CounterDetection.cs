using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterDetection : MonoBehaviour
{
    private HandGui handGui;
    private AACManager AACManager;
    private BoxCollider Hitbox;
    private bool DamageTaken = false;
    private void Start()
    {
        GameObject PlayerHand = GameObject.Find("GUI");
        handGui = PlayerHand.GetComponent<HandGui>();
        GameObject AAC = GameObject.Find("AAC");
        AACManager = AAC.GetComponent<AACManager>();
        Hitbox = GetComponent<BoxCollider>();
        
    }
    private void Update()
    {
        
        var AACstate = AACManager.AACState.CounterNormal;
        if (AACstate != AACManager.AACState.CounterNormal) {
            Hitbox.enabled = true;
            DamageTaken = false;
        } else
        {
            Hitbox.enabled = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        var AACstate = AACManager.AACState.CounterNormal;
        if (AACstate == AACManager.AACState.CounterNormal)
        {
            if (collision.collider.tag == "Weapon")
            {
                if (DamageTaken)
                {
                    handGui.HP -= 8.0f;
                    if (handGui.HP <= 10)
                    {
                        handGui.HP = 10f;
                    }
                }
                DamageTaken = true;
            }
        }
    }
}

