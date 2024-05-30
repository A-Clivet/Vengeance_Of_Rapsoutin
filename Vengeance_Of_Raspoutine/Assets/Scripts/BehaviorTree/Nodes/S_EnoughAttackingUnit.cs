using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnoughAttackingUnit : Node
{
    private Unit _unit;

    public S_EnoughAttackingUnit(Unit p_unit)
    {
        _unit = p_unit;
    }

    public override NodeState Evaluate()
    {
        if(_unit.attack > 6)
        {
            pr_state = NodeState.SUCCESS;
            return pr_state;
        }

        pr_state = NodeState.FAILURE;

        return pr_state;
    }

}
