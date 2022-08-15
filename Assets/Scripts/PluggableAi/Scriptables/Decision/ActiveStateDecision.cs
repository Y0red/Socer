using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "pluggableAI/decisions/activestate")]
public class ActiveStateDecision : Decesion
{
    public override bool Decide(StateController controller)
    {
        bool chaseTargetIsActive = controller.isAttacking;
        return chaseTargetIsActive;
    }
}