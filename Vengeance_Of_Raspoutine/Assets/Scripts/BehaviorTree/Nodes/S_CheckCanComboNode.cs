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
        _unitManager.UnitCombo(2, true);
        parent.SetData("k_comboColumn", _unitManager.UnitColumn);

        parent.SetData("k_comboLine", _unitManager.UnitLine);

        if (_unitManager.UnitColumn.Count > 0 || _unitManager.UnitLine.Count > 0)
        {
            pr_state = NodeState.SUCCESS;
            return pr_state;
        }

        pr_state = NodeState.FAILURE;
        ClearData("k_comboColumn");
        ClearData("k_comboLine");
        return pr_state;
    }
}