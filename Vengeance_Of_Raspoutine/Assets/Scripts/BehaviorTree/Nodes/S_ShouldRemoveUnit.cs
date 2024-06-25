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

    }

    public override NodeState Evaluate()
    {
        _unit = (Unit)GetData("k_LoneUnit");
        _unitComboColumn = (List<List<Unit>>)GetData("k_comboColumn");
        _unitComboLine = (List<List<Unit>>)GetData("k_comboLine");
        for (int i = 0; i < _unitComboColumn.Count; i++)
        {

            
            if (_unitComboColumn[i][0].CheckUnitInProximity(out var unit, 0, 2)&& _unitComboColumn[i][0].CheckUnitInProximity(out var unit2, 0, 1))
            {
                if (_unitComboColumn[i][0].unitColor == unit.unitColor && (unit2.state == 0 || unit2.state == 1))
                {
                    _removeUnit.RemoveUnitOnSpecificTile(unit2.actualTile[0]);
                    ClearData("k_LoneUnit");
                    ClearData("k_comboColumn");
                    ClearData("k_comboLine");
                    pr_state = NodeState.SUCCESS;
                    S_GameManager.Instance.ReduceActionPointBy1();
                    _gridManager.unitManager.UnitColumn.Clear();
                    _gridManager.unitManager.UnitLine.Clear();
                    return pr_state;
                }

/*                if (_unitComboColumn[i][_unitComboColumn[i].Count - 1].unitColor == _gridManager.gridList[_unitComboColumn[i][_unitComboColumn[i].Count - 1].tileX][_unitComboColumn[i][_unitComboColumn[i].Count - 1].tileY + 2].unit.unitColor)
                {
                    _removeUnit.RemoveUnitOnSpecificTile(_gridManager.gridList[_unitComboColumn[i][_unitComboColumn[i].Count - 1].tileX][_unitComboColumn[i][_unitComboColumn[i].Count - 1].tileY + 1]);
                    pr_state = NodeState.SUCCESS;
                    ClearData("k_LoneUnit");
                    ClearData("k_comboColumn");
                    ClearData("k_comboLine");
                    S_GameManager.Instance.ReduceActionPointBy1();
                    _gridManager.unitManager.UnitColumn.Clear();
                    _gridManager.unitManager.UnitLine.Clear();
                    return pr_state;
                }*/
            }
        }

        for (int i = 0; i <_unitComboLine.Count; i++)
        {



            if (_unitComboLine[i][0].CheckUnitInProximity(out var unit, 1, 1))
            {
                if (_unitComboLine[i][0].CheckUnitInProximity(out var unit2, 1)) 
                {
                    if (_unitComboLine[i][0].unitColor == unit.unitColor && (unit2.state == 0 || unit2.state == 1))
                    {
                        _removeUnit.RemoveUnitOnSpecificTile(unit2.actualTile[0]);
                        pr_state = NodeState.SUCCESS;
                        ClearData("k_LoneUnit");
                        ClearData("k_comboColumn");
                        ClearData("k_comboLine");
                        S_GameManager.Instance.ReduceActionPointBy1();
                        _gridManager.unitManager.UnitColumn.Clear();
                        _gridManager.unitManager.UnitLine.Clear();
                        return pr_state;
                    }
                } }
            else if (_unitComboLine[i][0].CheckUnitInProximity(out var unit2, -1, 1))
            {
                if (_unitComboLine[i][0].CheckUnitInProximity(out var unit3, -1))
                {
                    if (_unitComboLine[i][0].unitColor == unit2.unitColor)
                    {
                        _removeUnit.RemoveUnitOnSpecificTile(unit3.actualTile[0]);
                        pr_state = NodeState.SUCCESS;
                        ClearData("k_LoneUnit");
                        ClearData("k_comboColumn");
                        ClearData("k_comboLine");
                        S_GameManager.Instance.ReduceActionPointBy1();
                        _gridManager.unitManager.UnitColumn.Clear();
                        _gridManager.unitManager.UnitLine.Clear();
                        return pr_state;
                    }
                }
            }
        }
        pr_state = NodeState.FAILURE;
        ClearData("k_LoneUnit");
        ClearData("k_comboColumn");
        ClearData("k_comboLine");
        _gridManager.unitManager.UnitColumn.Clear();
        _gridManager.unitManager.UnitLine.Clear();
        return pr_state;
    }
}
