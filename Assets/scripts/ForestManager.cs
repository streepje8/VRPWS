using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestManager : MonoBehaviour
{
    public Animator a;
    public ProgressionConditional lastWords;
    private bool hasTriggered = false;

    public void PlayAnimation()
    {
        a.Play("Forest");
    }

    private void Update()
    {
        if(a.GetCurrentAnimatorStateInfo(0).IsName("Done"))
        {
            if(!hasTriggered)
            {
                hasTriggered = true;
                lastWords.Trigger();
            }
        }
    }
}
