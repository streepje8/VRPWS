using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandGui : MonoBehaviour
{
    public float HP;
    public float Energy;

    public float dotMin = 0.6f;
    public GameObject cam;
    public Slider HPBar;
    public Slider ENERGYBar;
    public float regainSpeed;
    public float currentRegSpeed = 0f;
    public RawImage[] rawImages = new RawImage[5];
    public LineRenderer lineRenderer;
    public Material Line;
    public Vector3 currentQuest = new Vector3(0,0,0);
    public bool hasQuest;

    public Color LineColor;


    // Update is called once per frame
    void Update()
    {
        float GuiDotCam = Mathf.Clamp(Vector3.Dot(transform.forward,(cam.transform.position - transform.position).normalized), 0f,1f);
        LineColor.a = Mathf.Clamp(GuiDotCam - 0.25f,0,1f);
        if (!hasQuest)
        {
            currentQuest = transform.position;
        }
        Line.SetColor("_Color",LineColor);
        Vector3[] pos = { transform.position, currentQuest};
        lineRenderer.SetPositions(pos);
        rawImages[0].color = new Color(rawImages[0].color.r, rawImages[0].color.g, rawImages[0].color.b, GuiDotCam);
        rawImages[1].color = new Color(rawImages[1].color.r, rawImages[1].color.g, rawImages[1].color.b, GuiDotCam);
        rawImages[2].color = new Color(rawImages[2].color.r, rawImages[2].color.g, rawImages[2].color.b, GuiDotCam);
        rawImages[3].color = new Color(rawImages[3].color.r, rawImages[3].color.g, rawImages[3].color.b, GuiDotCam);
        rawImages[4].color = new Color(rawImages[4].color.r, rawImages[4].color.g, rawImages[4].color.b, GuiDotCam);
        if ((Energy + (currentRegSpeed * Time.deltaTime)) < 500)
        {
            Energy += currentRegSpeed * Time.deltaTime;
        } else
        {
            Energy = 500;
        }
        if((HP + (0.5f * Time.deltaTime)) < 100)
        {
            HP += 0.5f * Time.deltaTime;
        } else
        {
            HP = 100;
        }
        HPBar.value = Mathf.Lerp(HPBar.value, HP, 0.1f);
        ENERGYBar.value = Mathf.Lerp(ENERGYBar.value, Energy / 5, 0.1f);
    }
}
