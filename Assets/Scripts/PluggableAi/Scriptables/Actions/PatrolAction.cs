using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "pluggableAI/Actions/Patrol")]
public class PatrolAction : AbstractAction
{
    public override void Act(StateController controller)
    {
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        //controller.isPatrolling = false;
        if(controller.navMeshAgent.agent.remainingDistance <= controller.navMeshAgent.agent.stoppingDistance && !controller.navMeshAgent.agent.pathPending)
        {
            //controller.nextWaypoint = (controller.nextWaypoint + 1) % controller.waypointsAll.Count;
            controller.transform.LookAt(controller.aiController.AwayGoal);
        }

        controller.navMeshAgent.SetTarget(controller.waypointsAll[0]);
        controller.navMeshAgent.agent.isStopped = false;
        if (controller.aiController.hasBall)
        {
            controller.navMeshAgent.agent.destination = controller.aiController.AwayGoal.transform.position;
            controller.navMeshAgent.agent.isStopped = false;
            //attacking
            controller.isAttacking = true;
        }
        else
        {
            controller.isAttacking = false;
            controller.navMeshAgent.SetTarget(controller.waypointsAll[controller.nextWaypoint]);
            controller.navMeshAgent.agent.isStopped = false;
        }
    }
}
