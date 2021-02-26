using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public HandGui Gui;
    public GameObject endGame;
    public enum StoryFase
    {
        Spawned,
        InTown,
        AtLeo,
        ToWaterfall,
        ToCastle,
        AtCastle,
        BossFight,
        Escape
    }

    public static StoryFase currentFase;

    // Update is called once per frame
    void Update()
    {
        switch(currentFase)
        {
            case StoryFase.Spawned:
                Gui.hasQuest = true;
                Gui.currentQuest = Vector3.Lerp(Gui.currentQuest,new Vector3(-132.2539f, 212.19f, 197.27f),5f * Time.deltaTime);
                break;
            case StoryFase.InTown:
                Gui.hasQuest = true;
                Gui.currentQuest = Vector3.Lerp(Gui.currentQuest, new Vector3(-219.48f, 210.44f, 167.01f), 5f * Time.deltaTime);
                break;
            case StoryFase.AtLeo:
                Gui.hasQuest = true;
                Gui.currentQuest = Vector3.Lerp(Gui.currentQuest, new Vector3(-221.7f, 211.29f, 240.71f), 5f * Time.deltaTime);
                break;
            case StoryFase.ToWaterfall:
                Gui.hasQuest = true;
                Gui.currentQuest = Vector3.Lerp(Gui.currentQuest, new Vector3(235.88f, 205.03f, 247.52f), 5f * Time.deltaTime);
                break;
            case StoryFase.ToCastle:
                Gui.hasQuest = true;
                Gui.currentQuest = Vector3.Lerp(Gui.currentQuest, new Vector3(-235.5f, 280.0185f, -99.7f), 5f * Time.deltaTime); 
                break;
            case StoryFase.Escape:
                Gui.hasQuest = true;
                Gui.currentQuest = Vector3.Lerp(Gui.currentQuest, new Vector3(149.1841f, 216.5551f, 43.53893f), 5f * Time.deltaTime);
                endGame.SetActive(true);
                break;
        }
    }
}
