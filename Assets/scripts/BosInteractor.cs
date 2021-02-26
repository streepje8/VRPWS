using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;
using static StoryManager;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(VRMBlendShapeProxy))]
public class BosInteractor : MonoBehaviour
{
    public List<Dialogue> Dialogues;
    public List<ProgressionConditional> ConditionalTriggers;
    public float engageDistance;
    public VRMBlendShapeProxy mouth;
    public float opennessMultiplier = 1f;
    public float openness = 0;
    public bool hasPlayed = false;
    public bool isPlaying = false;
    [Range(0, 1)]
    public float SyncSensitivty = 0.3f;
    public string BlendKeyName = "A";
    public bool autoBlink = true;
    private GameObject Player;
    private AudioSource Source;
    private float totalActivity = 0;
    private float BlinkTime = 0f;
    [HideInInspector]
    public Dialogue currentDialogue;
    public StoryFase faseAfterDialogue;
    public AudioSource audioTown;
    public AudioSource audioForest;
    public ForestManager fm;
    bool DoFinishActions = false;
    public bool isDead = false;
    public NPCInteractor aacBOS;
    public ProgressionConditional onStab;

    // Start is called before the first frame update
    void Start()
    {
        mouth = GetComponent<VRMBlendShapeProxy>();
        Player = GameObject.Find("VRPLAYER");
        Source = GetComponent<AudioSource>();
        currentDialogue = Dialogues[0];
        currentDialogue.reset();
        audioTown = GameObject.Find("AudioTown").GetComponent<AudioSource>();
        audioForest = GameObject.Find("AudioForest").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) <= engageDistance && !isPlaying)
        {
            isPlaying = true;
            Source.clip = currentDialogue.currentClip();
            Source.Play();
        }
        if (Source.isPlaying)
        {
            audioTown.volume = 0;
            audioForest.volume = 0;
            float[] spectrum = new float[256];

            AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

            totalActivity = 0;
            for (int i = 1; i < spectrum.Length - 1; i++)
            {
                totalActivity += spectrum[i];
                //Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1) * 10, new Vector3(Mathf.Log(i), spectrum[i] - 10, 1) * 10, Color.green);
            }
            totalActivity = Mathf.Clamp(((totalActivity - 0.2f) * 30) / 40 * opennessMultiplier, 0, 1);
            openness = totalActivity;
            if (openness < SyncSensitivty)
            {
                openness = 0;
            }
        }
        else
        {
            openness = 0;
        }
        if (isPlaying && !Source.isPlaying)
        {
            if (!currentDialogue.isDone())
            {
                currentDialogue.nextClip();
                isPlaying = false; //Reset
            }
            else
            {
                switch(Dialogues.IndexOf(currentDialogue))
                {
                    case 0:
                        if (!DoFinishActions)
                        {
                            DoFinishActions = true;
                            aacBOS.engageDistance = 999f;
                            onStab.isTriggered = true;
                            fm.PlayAnimation();
                        }
                        break;
                    case 1:
                        //Do nothing
                        break;
                    case 2:
                        if (!DoFinishActions)
                        {
                            hasPlayed = true;
                            DoFinishActions = true;
                            StoryManager.currentFase = StoryFase.AtCastle;
                            isDead = true;
                            GetComponent<Animator>().Play("Sterf");
                            audioTown.volume = 1f;
                            audioForest.volume = 0.183f;
                        }
                        break;
                }
            }
        }
        foreach (ProgressionConditional pc in ConditionalTriggers)
        {
            if (pc.isTriggered)
            {
                DoFinishActions = false;
                currentDialogue = Dialogues[pc.TriggerDialogue];
                currentDialogue.reset();
                isPlaying = false;
                pc.Reset();
            }
        }
        if(isDead)
        {
            mouth.ImmediatelySetValue("Blink", 0.625f);
            mouth.ImmediatelySetValue("Sorrow", 1f);
        }
        if (autoBlink && !isDead)
        {
            if (BlinkTime >= 3f)
            {
                BlinkTime = 0f;
                mouth.ImmediatelySetValue("Blink", 1);
            }
            else
            {
                BlinkTime += Time.deltaTime * (Random.Range(0f, 100f) / 100f);
            }
            float blinkvalue = Mathf.Lerp(mouth.GetValue(BlendShapeKey.CreateUnknown("Blink")), 0f, 7f * Time.deltaTime);
            if (blinkvalue < 0.01f)
            {
                blinkvalue = 0;
            }
            mouth.ImmediatelySetValue("Blink", blinkvalue);
        }
        mouth.ImmediatelySetValue(BlendKeyName, openness);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 1, 1);
        Gizmos.DrawWireSphere(transform.position, engageDistance);
    }
}
