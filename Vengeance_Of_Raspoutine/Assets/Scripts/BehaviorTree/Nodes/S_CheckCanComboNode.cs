using System.Collections.Generic;
using S_BehaviorTree;
using Unity.VisualScripting;
using UnityEngine;

public class S_CheckCanComboNode : Node
{
    private S_UnitManager _unitManager;

    public S_CheckCanComboNode(S_UnitManager p_unitManager)
    {
        _unitManager = p_unitManager;
    }

    public override NodeState Evaluate()
    {
        _unitManager.UnitCombo(2);

        if (_unitManager.UnitColumn.Count > 0)
        {
            SetData("k_comboColumn", _unitManager.UnitColumn);
            pr_state = NodeState.SUCCESS;
            return pr_state;
        }

        if (_unitManager.UnitLine.Count > 0)
        {
            SetData("k_comboLine", _unitManager.UnitLine);
            pr_state = NodeState.SUCCESS;
            return pr_state;
        }

        pr_state = NodeState.FAILURE;
        return pr_state;
    }
}