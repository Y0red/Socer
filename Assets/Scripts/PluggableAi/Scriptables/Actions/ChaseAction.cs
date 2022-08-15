using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "pluggableAI/Actions/chase action")]
public class ChaseAction : AbstractAction
{
    bool isAttacking = false;
    public override void Act(StateController controller)
    {
        Chase(controller);
    }

    private void Chase(StateController controller)
    {
        Ball ball = controller.chaseTarget;
        if (ball.hasPlayer())
        {
            if (ball.hasPlayer() && ball.GetHolderPlayer() != controller.aiController && ball.GetHolderPlayer().currentPlayerTeam != controller.aiController.currentPlayerTeam)
            {
                //deffending
                controller.isDefending = true;
            }
            else if(ball.hasPlayer() && ball.GetHolderPlayer() != controller.aiController && ball.GetHolderPlayer().currentPlayerTeam == controller.aiController.currentPlayerTeam)
            {
                // DoSupportTeamMet();

                    controller.isSupportingAttack = true;
            
            }
            else if(ball.hasPlayer() && ball.GetHolderPlayer() == controller.aiController && ball.GetHolderPlayer().currentPlayerTeam == controller.aiController.currentPlayerTeam)
            {
               controller.navMeshAgent.agent.destination = controller.aiController.AwayGoal.transform.position;
               controller.navMeshAgent.agent.isStopped = false;
                //attacking
               controller.isAttacking = true;
            }
        }
        else
        {
           // if (!isAttacking)
            {
                controller.navMeshAgent.agent.destination = controller.chaseTarget.transform.position;
                controller.navMeshAgent.agent.isStopped = false;
            }
        }
        
    }
}
