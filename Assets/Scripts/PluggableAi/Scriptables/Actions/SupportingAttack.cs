using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "pluggableAI/Actions/SupportingAttack")]
public class SupportingAttack : AbstractAction
{
    public override void Act(StateController controller)
    {
        SupportAttack(controller);
    }

    private void SupportAttack(StateController controller)
    {
        Debug.Log("we have the ball");
        Ball ball = controller.chaseTarget;
        if(ball.hasPlayer() && ball.GetHolderPlayer() != controller.aiController && ball.GetHolderPlayer().currentPlayerTeam == controller.aiController.currentPlayerTeam)
           {
           
                // DoSupportTeamMet();

                //controller.isSupportingAttack = true;
            controller.navMeshAgent.SetTarget(controller.waypointsAll[1]);
            controller.navMeshAgent.agent.isStopped = false;

        }
        
        else
        {
            controller.isSupportingAttack = false;
        }
    }
}
