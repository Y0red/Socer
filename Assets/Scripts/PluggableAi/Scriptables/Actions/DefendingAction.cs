using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "pluggableAI/Actions/Deffend")]
public class DefendingAction : AbstractAction
{
    public override void Act( StateController controller)
    {
        Defend(controller);
    }
    private void Defend(StateController controller)
    {
        // DoDefending();
        if(controller.chaseTarget != null && controller.isDefending)
        {
            Ball ball = controller.chaseTarget;
            if(ball.GetHolderPlayer() != null)
            {
                controller.navMeshAgent.agent.destination = ball.GetHolderPlayer().transform.position;
                controller.navMeshAgent.agent.isStopped = false;

                if (controller.navMeshAgent.agent.remainingDistance < 1 && !controller.aiController.isSliding && !ball.GetHolderPlayer().isAi)
                {
                    controller.aiController.DoTackle();
                }
            }
            else
            {
                controller.isDefending = false;
            }
        }
        
    }
}
