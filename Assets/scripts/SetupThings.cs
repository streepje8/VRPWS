using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupThings : MonoBehaviour
{
    public ProgressionConditional ryiatalked;
    public NPCInteractor npci;

    // Update is called once per frame
    void Update()
    {
        if(npci.Dialogues.IndexOf(npci.currentDialogue) == 1)
        {
            ryiatalked.Trigger();
        }
    }
}
