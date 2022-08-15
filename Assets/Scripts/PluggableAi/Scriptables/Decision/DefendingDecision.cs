using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "pluggableAI/decisions/defendingstate")]
public class DefendingDecision : Decesion
{
    public override bool Decide(StateController controller)
    {
        bool isDefending = controller.isDefending;
        return isDefending;
    }
}
