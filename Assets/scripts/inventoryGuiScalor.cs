using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventoryGuiScalor : MonoBehaviour
{
    public Transform Player;

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position,Player.position) < 7f)
      {
        float x = 1;
            float y = Mathf.Lerp(transform.localScale.y, 1f, 2 * Time.deltaTime);
            if (y > 0.95f)
            {
                y = 1f;
            }
            transform.localScale = new Vector3(x, y, 1);
        } else
        {
            float y = Mathf.Lerp(transform.localScale.y, 0f, 2 * Time.deltaTime);
            float x = (y < 0.05f) ? 0f : 1f;
            if(y < 0.1f)
            {
                y = 0f;
            }
            transform.localScale = new Vector3(x, y, 1);
        }
    }
}
