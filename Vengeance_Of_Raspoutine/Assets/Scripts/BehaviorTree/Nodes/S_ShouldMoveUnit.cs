using System.Collections.Generic;
using S_BehaviorTree;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_ShouldMoveUnit : Node
{
    private S_GridManager _gridManager;
    private Unit _unit;
    private List<List<Unit>> _unitColumn;
    private List<List<Unit>> _unitLine;

    public S_ShouldMoveUnit(S_GridManager p_gridManager)
    {
        _gridManager = p_gridManager;
    }

    public override NodeState Evaluate()
    {
        _unit = (Unit)GetData("k_LoneUnit");
        _unitColumn = (List<List<Unit>>)GetData("k_comboColumn");
        _unitLine = (List<List<Unit>>)GetData("k_comboLine");
        for (int i =  0; i < _unitColumn.Count; i++)
        {
            if (_unitColumn[i][0].tileY + 1 < _gridManager.height)
            {
                if (_gridManager.gridList[_unitColumn[i][_unitColumn[i].Count - 1].tileX][_unitColumn[i][0].tileY + 1].unit == null)
                {
                    if (_unit.unitColor == _unitColumn[i][0].unitColor)
                    {

                        _unit.ActionMoveToTile(_gridManager.gridList[_unitColumn[i][0].tileX][0]);
                        pr_state = NodeState.SUCCESS;
                        _gridManager.unitManager.UnitColumn.Clear();
                        _gridManager.unitManager.UnitLine.Clear();
                        ClearData("k_LoneUnit");
                        ClearData("k_comboColumn");
                        ClearData("k_comboLine");
                        return pr_state;

                    }
                }
            }
        }

        for (int i = 0; i < _unitLine.Count; i++)
        {
            if (_unitLine[i][0].tileX + 1 < _gridManager.width)
            {
                if (_gridManager.gridList[_unitLine[i][0].tileX + 1][_unitLine[i][0].tileY].unit == null)
                {
                    if (_unit.unitColor == _unitLine[i][0].unitColor)
                    {
                        _unit.ActionMoveToTile(_gridManager.gridList[_unitLine[i][0].tileX + 1][0]);
                        pr_state = NodeState.SUCCESS;
                        _gridManager.unitManager.UnitColumn.Clear();
                        _gridManager.unitManager.UnitLine.Clear();
                        ClearData("k_LoneUnit");
                        ClearData("k_comboColumn");
                        ClearData("k_comboLine");
                        return pr_state;
                    }
                }

            }
            if (_unitLine[i][_unitLine[i].Count-1].tileX - 1 >= 0)
            {
                if (_gridManager.gridList[_unitLine[i][_unitLine[i].Count - 1].tileX - 1][_unitLine[i][_unitLine[i].Count - 1].tileY].unit == null)
                {
                    if (_unit.unitColor == _unitLine[i][0].unitColor)
                    {
                        _unit.ActionMoveToTile(_gridManager.gridList[_unitLine[i][_unitLine[i].Count - 1].tileX - 1][0]);
                        pr_state = NodeState.SUCCESS;
                        _gridManager.unitManager.UnitColumn.Clear();
                        _gridManager.unitManager.UnitLine.Clear();
                        ClearData("k_LoneUnit");
                        ClearData("k_comboColumn");
                        ClearData("k_comboLine");
                        return pr_state;
                    }
                }
            }
        }
        pr_state = NodeState.FAILURE;
        return pr_state;
    }
}