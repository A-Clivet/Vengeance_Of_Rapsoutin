using System.Collections.Generic;
using S_BehaviorTree;
using UnityEngine;

public class S_CheckNumberUnit : Node
{
    private S_UnitCall _unitCall;

    public S_CheckNumberUnit(S_UnitCall p_unitCall)
    {
        _unitCall = p_unitCall;
    }

    public override NodeState Evaluate()
    {
        _unitCall.CallAmountUpdate();

        if (_unitCall.callAmount >= 8)     //check si il y a au moins 8 unité dans sa reserve
        {
            pr_state = NodeState.SUCCESS;
            return pr_state;
        }

        pr_state = NodeState.FAILURE;
        return pr_state;
    }
}
