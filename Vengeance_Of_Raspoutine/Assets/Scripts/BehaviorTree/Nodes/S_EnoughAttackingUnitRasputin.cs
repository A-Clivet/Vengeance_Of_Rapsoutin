using S_BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnoughAttackingUnitRasputin : Node
{
    private S_UnitManager _unitManager;

    public S_EnoughAttackingUnitRasputin(S_UnitManager p_unitManager)
    {
        _unitManager = p_unitManager;
    }

    public override NodeState Evaluate()
    {
        if(_unitManager.UnitColumn.Count > 2)
        {
            pr_state = NodeState.SUCCESS;
            return pr_state;
        }

        pr_state = NodeState.FAILURE;

        return pr_state;
    }

}
