using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "pluggableAI/decisions/activestate2")]
public class ActiveStateDecision2 : Decesion
{
    public override bool Decide(StateController controller)
    {
        bool hasBall = controller.aiController.hasBall;
        return hasBall;
    }
}
