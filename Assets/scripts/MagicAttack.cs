using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    public float Speed = 4f;
    private HandGui handGui;
    public void onStart()
    {
        GameObject Playerhand = GameObject.Find("GUI");
        handGui = Playerhand.GetComponent<HandGui>();
        transform.LookAt(new Vector3(-235.5f + Random.Range(-20,20), 300f + Random.Range(-4, 4), -99.7f + Random.Range(-20, 20)));
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        if(Vector3.Distance(transform.position,new Vector3(-231.7013f, 296f, -97.9f)) > 50f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        handGui.HP -= 5;
    }
}
