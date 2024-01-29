using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class TestEnemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public ThirdPersonCharacter character;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.transform.position);

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity, false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }
    }
}
