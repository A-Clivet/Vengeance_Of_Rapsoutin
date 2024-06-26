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

        if (_unitCall.CallAmountUpdate() >= 8)     //Check if there are 8 or more unit in the storage
        {
            pr_state = NodeState.SUCCESS;
            return pr_state;
        }

        pr_state = NodeState.FAILURE;
        return pr_state;
    }
}
