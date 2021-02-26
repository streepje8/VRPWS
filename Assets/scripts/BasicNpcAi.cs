
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class BasicNpcAi : MonoBehaviour
{
    [SerializeField]
    public Transform TestGoal;

    public ThirdPersonCharacter Character;
    public NavMeshAgent AgentNPC;

    void Start()
    {
        AgentNPC = GetComponent<NavMeshAgent>();
        AgentNPC.enabled = false;
        NavMeshHit closestHit;
        if (NavMesh.SamplePosition(gameObject.transform.position, out closestHit, 500f, NavMesh.AllAreas))
        {
            transform.position = closestHit.position;
            AgentNPC.enabled = true;
            AgentNPC.destination = transform.position;
        }
        else
        {
            Debug.LogError("Could not find position on NavMesh!");
        }
        AgentNPC.updateRotation = false;
    }

    private void Update()
    {
        setDestination(TestGoal.position);
        if (AgentNPC.remainingDistance > AgentNPC.stoppingDistance)
        {
            Character.Move(AgentNPC.desiredVelocity, false, false);
        } else
        {
            Character.Move(Vector3.zero, false, false);
        }
    }

    void setDestination(Vector3 togo)
    {
        Vector3 destination = togo;
        NavMeshHit closestHit;
        if (NavMesh.SamplePosition(destination, out closestHit, 500f, NavMesh.AllAreas))
        {
            destination = closestHit.position;
        }
        else
        {
            Debug.LogError("Could not find position on NavMesh for destination!");
        }
        GetComponent<NavMeshAgent>().destination = destination;
    }
}
