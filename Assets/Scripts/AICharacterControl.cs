using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(TPC))]
public class AICharacterControl : MonoBehaviour
{
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] TPC character;
    Transform target;

    private void Start()
    {
        agent.updateRotation = false;
        agent.updatePosition = true;
    }

    private void Update()
    {
        if (target != null)
            agent.SetDestination(target.position);

        if (agent.remainingDistance > agent.stoppingDistance)
            character.Move(agent.desiredVelocity);
        else
            character.Move(Vector3.zero);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
