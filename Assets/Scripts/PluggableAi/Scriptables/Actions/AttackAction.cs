using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "pluggableAI/Actions/Attack")]
public class AttackAction : AbstractAction
{
    public override void Act(StateController controller)
    {
        Attackk(controller);
    }

    private void Attackk(StateController controller)
    {
        controller.navMeshAgent.agent.destination = controller.aiController.AwayGoal.transform.position;
        controller.navMeshAgent.agent.isStopped = false;


        if (controller.navMeshAgent.agent.remainingDistance <= 25 && controller.aiController.hasBall)
        {
            //controller.navMeshAgent.agent.isStopped = true;

            controller.transform.LookAt(controller.aiController.AwayGoal);

            controller.aiController.Shoot();
           //controller.ThinkTime(5f);
            Debug.Log("shooting");
            controller.isAttacking = false;
        }
    }
}
