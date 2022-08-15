using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    public TPUC aiController;

    public Transform eyes;

    public State currentState;
    public State remainState;

    public List<Transform> waypointsAll;

    public int nextWaypoint;

    public AICharacterControl navMeshAgent;
    [HideInInspector]public Ball chaseTarget;
    [HideInInspector]public float stateTimeElapsed;
    public float aggroRange = 10;

    bool aiActive;
    public bool isAttacking = false;
    public bool isDefending = false;
    public bool isSupportingAttack = false;

    public void SetuupAi(List<Transform> waypoints)
    {
        waypointsAll = waypoints;
        aiActive = true;
        if (aiActive)
        {
            navMeshAgent.enabled = true;
        }
        else
        {
            navMeshAgent.enabled = false;
        }
    }
    private void Update()
    {
        currentState.UpdateState(this);
    }

    private void OnDrawGizmos()
    {
        if(currentState != null && eyes != null)
        {
            Gizmos.color = currentState.sceneGezmoColor;
            Gizmos.DrawWireSphere(transform.position, aggroRange);
        }
    }

    public void TransitionToState(State nextState)
    {
        if(nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }
    
    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }
    public void ThinkTime(float time)
    {
        StartCoroutine(Thinking(time));
    }

    private IEnumerator Thinking(float time)
    {
        yield return new WaitForSeconds(time);
    }
}