using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSection : MonoBehaviour
{
    public int loadDistance;
    private GameObject player;
    private bool loaded = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("VRPLAYER");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > loadDistance)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).gameObject.SetActiveRecursively(false);
            }
            loaded = false;
        }
        else
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).gameObject.SetActiveRecursively(true);
            }
            loaded = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        if (loaded) { 
            Gizmos.color = Color.white; 
        } else
        {
            Gizmos.color = Color.grey;
        }
        Gizmos.DrawWireSphere(transform.position, loadDistance);
    }
}
