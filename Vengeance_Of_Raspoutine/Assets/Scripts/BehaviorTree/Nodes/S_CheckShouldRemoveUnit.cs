using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class S_CheckShouldRemoveUnit : Node
{
    private S_GridManager _gridManager;
    private Unit _unit;
    private S_RemoveUnit _removeUnit;

    public S_CheckShouldRemoveUnit(S_GridManager p_gridManager, Unit p_unit, S_RemoveUnit p_removeUnit)
    {
        _gridManager = p_gridManager;
        _unit = p_unit;
        _removeUnit = p_removeUnit;
    }

    public override NodeState Evaluate()
    {
        //code for the behavior here
        pr_state = NodeState.FAILURE;

        _removeUnit.DestroyUnit();

        //Debug.Log($"{this} : {state}");
        return pr_state;
    }
}
