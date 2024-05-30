using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_CheckShouldMoveUnit : Node
{
    private S_GridManager _gridManager;
    private Unit _unit;

    public S_CheckShouldMoveUnit(S_GridManager p_gridManager, Unit p_unit)
    {
        _gridManager = p_gridManager;
        _unit = p_unit;
    }

    public override NodeState Evaluate()
    {
        _unit.MoveToTile()
        pr_state = NodeState.FAILURE;



        return pr_state;
    }

    public void GetData(string p_keyLoneUnit)
    {
        object value = null;
        if (pr_dataContext.TryGetValue(p_keyLoneUnit, out value))
            return value;

        Node node = parent;
        while (node != null)
        {
            value = node.GetData(p_keyLoneUnit);
            if (value != null)
                return value;
            node = node.parent;
        }
        return null;
    }
}