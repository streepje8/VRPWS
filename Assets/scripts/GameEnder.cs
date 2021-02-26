using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GameEnder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SteamVR_Fade.Start(Color.white,2f);
        GameObject.Find("VRPLAYER").GetComponent<VRcontroller>().m_WalkSpeed = 0f;
        GameObject.Find("VRPLAYER").GetComponent<VRcontroller>().m_RunSpeed = 0f;
    }
}
