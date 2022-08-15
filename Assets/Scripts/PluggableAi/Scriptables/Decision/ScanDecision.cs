using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "pluggableAI/decisions/Scan")]
public class ScanDecision : Decesion
{
    public override bool Decide(StateController controller)
    {
        bool noEnemyInSight = Scan(controller);
        return noEnemyInSight;
    }

    private bool Scan(StateController controller)
    {
        controller.navMeshAgent.agent.isStopped  = true;
        controller.transform.Rotate(0, .2f * Time.deltaTime, 0);
        Debug.Log("rot");
        return controller.CheckIfCountDownElapsed(.5f);
    }
}
