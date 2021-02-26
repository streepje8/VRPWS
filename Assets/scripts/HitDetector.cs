using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{

    public ProgressionConditional HitTarget;
    public bool hasHit = false;

    //Detect collisions between the GameObjects with Colliders attached
    private void OnTriggerEnter(Collider other)
    {
        if (!hasHit)
        {
            HitTarget.Trigger();
        }
        hasHit = true;
    }
}
