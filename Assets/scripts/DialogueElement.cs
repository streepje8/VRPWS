using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueElement : ScriptableObject
{
    [SerializeField]
    public bool isCondition = false;
    [SerializeField]
    public bool isDelay = false;
    [SerializeField]
    public float Time = 0f;
    [SerializeField]
    public ProgressionConditional conditional;
    [SerializeField]
    public AudioClip VoiceLine;
    [SerializeField]
    public AudioClip VoiceLineFalse;

    override public String ToString()
    {
        if (VoiceLine != null)
        {
            if (conditional != null)
            {
                return "Element[" + isCondition + "," + conditional.name + "," + VoiceLine.name + "," + VoiceLineFalse.name + "]";
            }
            else
            {
                return "Element[" + isCondition + "," + VoiceLine.name + "]";
            }
        }
        else
        {
            return "Element[" + isCondition + "]";
        }
    }
}
