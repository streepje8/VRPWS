
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class sun : MonoBehaviour
{
    public float offsetAngle = 0f;
    public Material skyboxday;
    public Material skyboxlower;
    public Material skyboxrise;
    public Material skyboxnight;
    public Material skybox;
    public bool test;
    public float testspeed = 0.05f;
    float currentSunAngle = 0f;
    public float i = 0;
    public float transitionspeed = 1f;
    public String lastTexture;
    public bool forceAngle = false;
    public float forceto = 0f;
    public Color riseCol;
    public Color lowerCol;
    public Color DayCol;
    public Color NightCol;

    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            currentSunAngle+= testspeed;
        } else if(forceAngle)
        {
            currentSunAngle = forceto;
        } else
        {
            float hourAngle = 360f / 24f; //18
            float minuteAngle = 360f / (24f * 60f); //37
            float secondAngle = 360f / (24f * 60f * 60f); //00
            DateTime currentTime = System.DateTime.Now;
            currentSunAngle = offsetAngle +
                currentTime.Hour * hourAngle +
                currentTime.Minute * minuteAngle +
                currentTime.Second * secondAngle;
        }
        while(currentSunAngle > 360)
        {
            currentSunAngle -= 360;
        }
        while (currentSunAngle < 0)
        {
            currentSunAngle += 360;
        }
        transform.rotation = Quaternion.Euler(currentSunAngle, 270f, 0f);
        if (currentSunAngle < 20f)
        {
            skybox.SetTexture("_MainTex", skyboxnight.mainTexture);
            if (skybox.GetTexture("_MainTex").name != lastTexture)
            {
                i = 0;
            }
            lastTexture = skybox.GetTexture("_MainTex").name;
            skybox.SetTexture("_MainTex2", skyboxrise.mainTexture);
            RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, riseCol, i);
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, new Color32(181, 123, 47, 255), 1f * Time.deltaTime);
        }
        if (currentSunAngle >= 20f && currentSunAngle < 180f)
        {
            skybox.SetTexture("_MainTex", skyboxrise.mainTexture);
            if (skybox.GetTexture("_MainTex").name != lastTexture)
            {
                i = 0;
            }
            lastTexture = skybox.GetTexture("_MainTex").name;
            skybox.SetTexture("_MainTex2", skyboxday.mainTexture);
            RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, DayCol, i);
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, new Color32(78, 170, 191, 255), 1f * Time.deltaTime);
        }
        if (currentSunAngle >= 180f && currentSunAngle < 190f)
        {
            skybox.SetTexture("_MainTex", skyboxday.mainTexture);
            if (skybox.GetTexture("_MainTex").name != lastTexture)
            {
                i = 0;
            }
            lastTexture = skybox.GetTexture("_MainTex").name;
            skybox.SetTexture("_MainTex2", skyboxlower.mainTexture);
            RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, lowerCol, i);
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, new Color32(181, 123, 47, 255), 1f * Time.deltaTime);
        }
        if (currentSunAngle >= 190f && currentSunAngle < 350f)
        {
            skybox.SetTexture("_MainTex", skyboxlower.mainTexture);
            if (skybox.GetTexture("_MainTex").name != lastTexture)
            {
                i = 0;
            }
            lastTexture = skybox.GetTexture("_MainTex").name;
            skybox.SetTexture("_MainTex2", skyboxnight.mainTexture);
            RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, NightCol, i);
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, NightCol, 1f * Time.deltaTime);
        }
        skybox.SetFloat("_Blend", i);
        if (i < 1) { i += (transitionspeed * Time.deltaTime); } else { i = 1; }
    }
}
