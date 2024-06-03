using System.Collections.Generic;
using BehaviorTree;
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
        _unit = (Unit)GetData("k_loneUnit");
        _unitColumn = (List<List<Unit>>)GetData("k_comboColumn");
        _unitLine = (List<List<Unit>>)GetData("k_comboLine");
    }

    public override NodeState Evaluate()
    {
        for (int i =  0; i < _unitColumn.Count; i++)
        {
            if (_gridManager.gridList[_unitColumn[i][_unitColumn[i].Count - 1].tileX][_unitColumn[i][_unitColumn[i].Count - 1].tileY + 1] == null)
            {
                _unit.ActionMoveToTile(_gridManager.gridList[_unitColumn[i][_unitColumn[i].Count - 1].tileX][_unitColumn[i][_unitColumn[i].Count - 1].tileY + 1]);
                pr_state = NodeState.SUCCESS;
            }
        }

        for (int i = 0; i < _unitLine.Count; i++)
        {
            if (_gridManager.gridList[_unitLine[i][_unitLine[i].Count - 1].tileX - 1][_unitLine[i][_unitLine[i].Count - 1].tileY] == null)
            {
                _unit.ActionMoveToTile(_gridManager.gridList[_unitLine[i][_unitColumn[i].Count - 1].tileX][_unitLine[i][_unitLine[i].Count - 1].tileY + 1]);
                pr_state = NodeState.SUCCESS;
            }

            if (_gridManager.gridList[_unitLine[i][_unitLine[i].Count - 1].tileX + 1][_unitLine[i][_unitLine[i].Count - 1].tileY] == null)
            {
                _unit.ActionMoveToTile(_gridManager.gridList[_unitLine[i][_unitColumn[i].Count - 1].tileX][_unitLine[i][_unitLine[i].Count - 1].tileY + 1]);
                pr_state = NodeState.SUCCESS;
            }
        }

        pr_state = NodeState.FAILURE;
        return pr_state;
    }
}