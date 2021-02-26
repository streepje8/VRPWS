using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BowWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    public Material TipMaterial;
    [ColorUsage(true, true)]
    public Color defaultColor;
    [ColorUsage(true, true)]
    public Color chargedColor;
    public SteamVR_ActionSet ac;
    public SteamVR_Action_Boolean skill;
    public SteamVR_Action_Vibration haptics;
    public float chargespeed;
    public float charge = 0f;
    public GameObject GUI;
    public Interactable inter;
    public ProgressionConditional usingBow;
    public ProgressionConditional weaponPicked;
    private bool hasTriggered = false;
    public ProgressionConditional magicUsed;
    public ProgressionConditional ryiaTalked;
    private bool hasTriggeredTwo = false;

    void Start()
    {
        GUI = GameObject.Find("GUI");
        inter = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inter.inHand)
        {
            usingBow.Trigger();
            if (!hasTriggered)
            {
                hasTriggered = true;
                weaponPicked.Trigger();
            }
        }
        if (skill.GetState(SteamVR_Input_Sources.Any)) //swordskill
        {
            haptics.Execute(0, 0.5f, 50, 0.2f, SteamVR_Input_Sources.Any);
            if (!hasTriggeredTwo && ryiaTalked.isTriggered)
            {
                hasTriggeredTwo = true;
                magicUsed.Trigger();
            }
            GUI.GetComponent<HandGui>().currentRegSpeed = 0f;
            if (GUI.GetComponent<HandGui>().Energy > 25f * Time.deltaTime) //normally the mana check
            {
                GUI.GetComponent<HandGui>().Energy -= 25f * Time.deltaTime;
                if (charge + chargespeed < 100)
                {
                    charge += chargespeed;
                }
                else
                {
                    if (charge < 100)
                    {
                        charge = 100;
                    }
                }
            }
            else
            {
                if (charge > 0f)
                {
                    charge--;
                    if (charge <= 1f)
                    {
                        charge = 0f;
                    }
                }
            }
        }
        else
        {
            GUI.GetComponent<HandGui>().currentRegSpeed = GUI.GetComponent<HandGui>().regainSpeed;
            if (charge > 0f)
            {
                charge--;
                if (charge <= 1f)
                {
                    charge = 0f;
                }
            }
        }
        float visualcharge = Mathf.Pow(2, (charge / 10) - 10) * 100;
        TipMaterial.SetColor("_Color", Color.Lerp(defaultColor, chargedColor, visualcharge / 100f));
    }
}
