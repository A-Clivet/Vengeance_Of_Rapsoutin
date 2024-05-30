using System.Collections.Generic;
using BehaviorTree;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_CheckLoneUnit : Node
{
    private S_UnitManager _unitManager;

    public S_CheckLoneUnit(S_UnitManager p_unitManager)
    {
        _unitManager = p_unitManager;
    }

    public override NodeState Evaluate()
    {
        _unitManager.grid;
        _unitManager.UnitCombo(1);
        if (_unitManager.UnitColumn.Count > 0 || _unitManager.UnitLine.Count > 1)
        {
            pr_state = NodeState.SUCCESS;
            return pr_state;
        }

        pr_state = NodeState.FAILURE;

        return pr_state;
    }

    public void SetData(string p_keyLoneUnit, _unitManager)
    {
        pr_dataContext[p_keyLoneUnit] = _unitManager.;
    }
}