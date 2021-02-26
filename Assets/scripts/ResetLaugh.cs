using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLaugh : MonoBehaviour
{
    private float time = 0;
    public int DialogueID = 0;

    // Update is called once per frame
    void Update()
    {
        if(time >= 4f)
        {
            GetComponent<NPCInteractor>().currentDialogue = GetComponent<NPCInteractor>().Dialogues[DialogueID];
            GetComponent<NPCInteractor>().currentDialogue.reset();
            GetComponent<NPCInteractor>().hasPlayed = false;
            GetComponent<NPCInteractor>().isPlaying = false;
            time = 0f;
        } else
        {
            time += Time.deltaTime * (Random.Range(0f, 100f) / 100f);
        }
    }
}
