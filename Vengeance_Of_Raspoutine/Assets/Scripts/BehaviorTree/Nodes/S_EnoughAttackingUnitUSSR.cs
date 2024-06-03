using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class S_EnoughAttackingUnitUSSR : Node
{
    private S_GridManager _gridManager;

    public S_EnoughAttackingUnitUSSR(S_GridManager p_gridManager)
    {
        _gridManager = p_gridManager;
    }

    public override NodeState Evaluate()
    {
        if (_gridManager.enemyGrid.unitManager.UnitColumn.Count > 2)
        {
            pr_state = NodeState.SUCCESS;
            return pr_state;
        }

        pr_state = NodeState.FAILURE;

        return pr_state;
    }
}
