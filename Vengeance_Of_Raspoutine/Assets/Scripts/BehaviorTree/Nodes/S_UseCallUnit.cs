using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class S_UseCallUnit: Node
{
    private S_UnitCall _unitCall;

    public S_UseCallUnit(S_UnitCall p_unitCall)
    {
        _unitCall = p_unitCall;
    }

    public override NodeState Evaluate()
    {
        _unitCall.UnitCalling();

        pr_state = NodeState.SUCCESS;
        return pr_state;
    }
}
