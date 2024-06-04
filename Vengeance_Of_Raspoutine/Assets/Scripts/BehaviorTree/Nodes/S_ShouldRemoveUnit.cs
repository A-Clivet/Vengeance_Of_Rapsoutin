using System.Collections.Generic;
using S_BehaviorTree;
using TMPro;
using UnityEngine;

public class S_ShouldRemoveUnit : Node
{
    private S_GridManager _gridManager;
    private S_RemoveUnit _removeUnit;
    private Unit _unit;
    private List<List<Unit>> _unitComboColumn;
    private List<List<Unit>> _unitComboLine;

    public S_ShouldRemoveUnit(S_GridManager p_gridManager, S_RemoveUnit p_removeUnit)
    {
        _gridManager = p_gridManager;
        _removeUnit = p_removeUnit;
        _unit = (Unit)GetData("k_loneUnit");
        _unitComboColumn = (List<List<Unit>>)GetData("k_comboColumn");
        _unitComboLine = (List<List<Unit>>)GetData("k_comboLine");
    }

    public override NodeState Evaluate()
    {
        for (int i = 0; i <= _unitComboColumn.Count; i++)
        {
            if (_unitComboColumn[i][0].tileY - 2 < 0)
            {
                continue;
            }

            if (_unitComboColumn[i][_unitComboColumn[i].Count - 1].tileY + 2 >= _gridManager.height)
            {
                continue;
            }

            if (_unitComboColumn[i][0].SO_Unit.unitType == _gridManager.gridList[_unitComboColumn[i][0].tileX][_unitComboColumn[i][0].tileY - 2].unit.SO_Unit.unitType)
            {
                _removeUnit.RemoveUnitAI(_gridManager.gridList[_unitComboColumn[i][0].tileX][_unitComboColumn[i][0].tileY - 1]);
                pr_state = NodeState.SUCCESS;
                return pr_state;
            }

            if (_unitComboColumn[i][_unitComboColumn[i].Count - 1].SO_Unit.unitType == _gridManager.gridList[_unitComboColumn[i][_unitComboColumn[i].Count - 1].tileX][_unitComboColumn[i][_unitComboColumn[i].Count - 1].tileY + 2].unit.SO_Unit.unitType)
            {
                _removeUnit.RemoveUnitAI(_gridManager.gridList[_unitComboColumn[i][_unitComboColumn[i].Count - 1].tileX][_unitComboColumn[i][_unitComboColumn[i].Count - 1].tileY + 1]);
                pr_state = NodeState.SUCCESS;
                return pr_state;
            }
        }

        for (int i = 0; i <= _unitComboLine.Count; i++)
        {
            if (_unitComboLine[i][0].tileY + 1 >= _gridManager.height || _unitComboLine[i][0].tileX - 1 < 0)
            {
                continue;
            }

            if ( _unitComboLine[i][0].tileX + 1 >= _gridManager.width)
            {
                continue;
            }

            if (_unitComboLine[i][0].SO_Unit.unitType == _gridManager.gridList[_unitComboLine[i][0].tileX - 1][_unitComboLine[i][0].tileY + 1].unit.SO_Unit.unitType)
            {
                _removeUnit.RemoveUnitAI(_gridManager.gridList[_unitComboLine[i][0].tileX -1 ][_unitComboLine[i][0].tileY]);
                pr_state = NodeState.SUCCESS;
                return pr_state;
            }

            if (_unitComboLine[i][_unitComboLine[i].Count - 1].SO_Unit.unitType == _gridManager.gridList[_unitComboLine[i][_unitComboLine[i].Count - 1].tileX + 1][_unitComboLine[i][_unitComboLine[i].Count - 1].tileY + 1].unit.SO_Unit.unitType)
            {
                _removeUnit.RemoveUnitAI(_gridManager.gridList[_unitComboLine[i][_unitComboLine[i].Count - 1].tileX + 1][_unitComboLine[i][_unitComboLine[i].Count - 1].tileY]);
                pr_state = NodeState.SUCCESS;
                return pr_state;
            }
        }
        pr_state = NodeState.FAILURE;
        return pr_state;
    }
}
