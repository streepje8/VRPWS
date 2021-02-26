using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AACManager : MonoBehaviour
{

    public enum AACState
    {
        Idle,
        AttackFly,
        AttackNormal,
        CounterNormal
    }

    public AACState currentState;

    public float AACtime = 180f;
    public float AirAttackDuration = 30f;
    public float CounterAttackDuration = 10f;
    public float NormalAttackDuration = 5f;

    
    public NavMeshAgent Agent;
    public Transform Center;
    public Transform Player; 


    public ParticleSystem particleSys;
    public GameObject VFX;
    public GameObject VFX2;
    public GameObject VFX3;
    public GameObject MagicBullet;
    public Animator animator;
    public float cooldown = 10f;
    public ProgressionConditional resetPlayerOutside;
    public ProgressionConditional AACDOOD;
    public GameObject teleporter;
    private float bulletCooldown = 0f;
    public bool triggeronce = false;


    [System.Obsolete]

    void Update()
    {
        if(resetPlayerOutside.isTriggered)
        {
            teleporter.SetActive(false);
            Player.position = new Vector3(-200.37f, 225.47f, -108.91f);
            resetPlayerOutside.Reset();
        }
        if (AACtime > 0)
        {
            AACtime -= Time.deltaTime;
        } else
        {
            currentState = AACState.Idle;
            Agent.isStopped = true;
            if(!triggeronce)
            {
                triggeronce = true;
                AACDOOD.Trigger();
            }
        }

        float Timer = 60f;
        if (Timer > 0)
        {
            if (Timer > 0)
            {
                currentState = AACState.AttackNormal;
                Agent.SetDestination(Player.position);
                Agent.speed = 2f;
            }
            if (Timer > 10)
            {
                currentState = AACState.CounterNormal;
                Agent.SetDestination(Player.position);
                Agent.speed = 1f;
            }
            if (Timer > 30)
            {
                Agent.SetDestination(Center.position);
                if (transform.position == Center.position)
                {
                    Agent.isStopped = true;
                    currentState = AACState.AttackFly;
                }
            }
            if (Timer > 40)
            {
                currentState = AACState.CounterNormal;
                Agent.SetDestination(Player.position);
                Agent.speed = 1f;
            }
            if (Timer > 55)
            {
                currentState = AACState.AttackNormal;
                Agent.SetDestination(Player.position);
                Agent.speed = 2f;
            }
            if(currentState != AACState.Idle) { 
                Timer -= Time.deltaTime;
            }
        }
        else
        {
            Timer = 60;
        }
        switch(currentState)
        {
            case AACState.AttackFly: //30 sec
                particleSys.startSpeed = -10;
                particleSys.emissionRate = 40;
                VFX.transform.localScale = Vector3.Lerp(VFX.transform.localScale, new Vector3(1, 1, 1), Time.deltaTime);
                VFX2.transform.localScale = Vector3.Lerp(VFX2.transform.localScale, new Vector3(0, 0, 0), Time.deltaTime);
                VFX3.transform.localScale = Vector3.Lerp(VFX3.transform.localScale, new Vector3(0, 0, 0), Time.deltaTime);
                animator.SetBool("MidAir", true);
                if (bulletCooldown > cooldown) {
                    bulletCooldown = 0f;
                    GameObject bullet = GameObject.Instantiate(MagicBullet);
                    bullet.transform.position = new Vector3(-231.7013f + (Random.Range(40,50) * Random.Range(-1, 1)), 296 + Random.Range(0,20), -97.9f + (Random.Range(40,50) * Random.Range(-1,1)));
                    bullet.GetComponent<MagicAttack>().onStart();
                } else
                {
                    bulletCooldown += Time.deltaTime * (Random.Range(0f, 100f) / 100f);
                }
                break;

            case AACState.AttackNormal: //5 sec
                particleSys.startSpeed = -0.94f;
                particleSys.emissionRate = 10;
                VFX.transform.localScale = Vector3.Lerp(VFX.transform.localScale, new Vector3(0, 0, 0), Time.deltaTime);
                VFX2.transform.localScale = Vector3.Lerp(VFX2.transform.localScale, new Vector3(0,0,0), Time.deltaTime);
                VFX3.transform.localScale = Vector3.Lerp(VFX3.transform.localScale, new Vector3(1, 1, 1), Time.deltaTime);
                animator.SetBool("MidAir", false);
                break;

            case AACState.CounterNormal: //10 sec
                particleSys.startSpeed = -0.95f;
                particleSys.emissionRate = 4;
                VFX.transform.localScale = Vector3.Lerp(VFX.transform.localScale, new Vector3(0, 0, 0), Time.deltaTime);
                VFX2.transform.localScale = Vector3.Lerp(VFX2.transform.localScale, new Vector3(1, 1, 1), Time.deltaTime);
                VFX3.transform.localScale = Vector3.Lerp(VFX3.transform.localScale, new Vector3(0, 0, 0), Time.deltaTime);
                animator.SetBool("MidAir", false);
                animator.SetBool("Counter", true);
                break;

            case AACState.Idle:
                particleSys.startSpeed = -0.94f; 
                particleSys.emissionRate = 4;
                VFX.transform.localScale = Vector3.Lerp(VFX.transform.localScale, new Vector3(0, 0, 0), Time.deltaTime);
                VFX2.transform.localScale = Vector3.Lerp(VFX2.transform.localScale, new Vector3(0,0,0), Time.deltaTime);
                VFX3.transform.localScale = Vector3.Lerp(VFX3.transform.localScale, new Vector3(0, 0, 0), Time.deltaTime);
                animator.SetBool("MidAir", false);
                break;
            
        }    
    }
}
  