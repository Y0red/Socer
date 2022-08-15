using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "pluggableAI/decisions/patrollDecision")]
public class PatrolDecision : Decesion
{
    public override bool Decide(StateController controller)
    {
        bool isPatrolling = controller.isSupportingAttack;
        return isPatrolling;
    }
}
