using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "pluggableAI/decisions/look")]
public class LookDecisions : Decesion
{
    public override bool Decide(StateController controller)
    {
        bool targetVisible = Look(controller);
        return targetVisible;
    }
   
    private bool Look(StateController controller)
    {
        //RaycastHit hit;

        //Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.aggroRange, Color.green);
        //Debug.DrawRay(controller.eyes.position, -controller.eyes.forward.normalized * controller.aggroRange, Color.green);

        //if (Physics.SphereCast(controller.transform.position, controller.aggroRange, controller.transform.forward, out hit, controller.aggroRange) && hit.collider.CompareTag("Ball"))
        // {
        //     controller.chaseTarget = hit.transform.GetComponent<Ball>();
        //     return true;
        // }
        //else if (Physics.SphereCast(controller.transform.position, controller.aggroRange, -controller.transform.forward, out hit, controller.aggroRange) && hit.collider.CompareTag("Ball"))
        //{
        //    controller.chaseTarget = hit.transform.GetComponent<Ball>();
        //    return true;
        //}
        if (Vector3.Distance(controller.transform.position, controller.aiController.ball.transform.position) <= controller.aggroRange && !controller.aiController.isShooting && !controller.aiController.isPassing)
        {
            controller.chaseTarget = controller.aiController.ball;


            return true;
        }
        else
        {
            return false;
        }
    }
}
