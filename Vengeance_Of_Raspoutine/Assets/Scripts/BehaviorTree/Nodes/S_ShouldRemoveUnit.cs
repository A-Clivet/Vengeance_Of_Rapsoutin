using System.Collections.Generic;
using BehaviorTree;
using TMPro;
using UnityEngine;

public class S_ShouldRemoveUnit : Node
{
    private S_GridManager _gridManager;
    private S_RemoveUnit _removeUnit;
    private Unit _unit;
    private List<List<Unit>> _unitColumn;
    private List<List<Unit>> _unitLine;

    public S_ShouldRemoveUnit(S_GridManager p_gridManager, S_RemoveUnit p_removeUnit)
    {
        _gridManager = p_gridManager;
        _removeUnit = p_removeUnit;
        _unit = (Unit)GetData("k_loneUnit");
        _unitColumn = (List<List<Unit>>)GetData("k_comboColumn");
        _unitLine = (List<List<Unit>>)GetData("k_comboLine");
    }

    public override NodeState Evaluate()
    {
        bool _foundAnnoyingUnitColumn = true;
        bool _foundAnnoyingUnitLine = true;

        for (int i = 0; i <= _gridManager.AllUnitPerColumn.Count; i++)
        {
            _foundAnnoyingUnitColumn = false;
            _foundAnnoyingUnitLine = false;

            for (int j = 2; j < 4; j++)
            {
                if (_gridManager.AllUnitPerColumn[i].Count - j < 0)
                {
                    if (_gridManager.gridList[_unitColumn[i][_unitColumn[i].Count - 1].tileX][_unitColumn[i][_unitColumn[i].Count - 1].tileY + 1] == null)
                    {
                        if (_gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].SO_Unit.unitType == _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - j].SO_Unit.unitType)
                        {
                            continue;
                        }
                        _foundAnnoyingUnitColumn = true;
                    }
                }
            }

            for (int j = 2; j < 4; j++)
            {
                if (_gridManager.gridList[i - j].Count - 2 < 0 || _gridManager.gridList[i + j].Count - 2 > _gridManager.gridList.Count - 1 || _gridManager.gridList[i - j][_gridManager.AllUnitPerColumn.Count - 2].unit != null || _gridManager.gridList[i + j][_gridManager.AllUnitPerColumn.Count - 2].unit != null)
                {
                    if (_gridManager.gridList[_unitColumn[i][_unitColumn[i].Count - j].tileX][_unitColumn[i][_unitColumn[i].Count - j].tileY + 1] == null)
                    {
                        if (_gridManager.AllUnitPerColumn[i - j][_gridManager.AllUnitPerColumn[i].Count - 2].SO_Unit.unitType == _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].SO_Unit.unitType)
                        {
                            continue;
                        }
                    }

                    if (_gridManager.gridList[_unitColumn[i][_unitColumn[i].Count - j].tileX][_unitColumn[i][_unitColumn[i].Count - j].tileY + 1] == null)
                    {
                        if (_gridManager.AllUnitPerColumn[i + j][_gridManager.AllUnitPerColumn[i].Count - 2].SO_Unit.unitType == _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].SO_Unit.unitType)
                        {
                            continue;
                        }
                    }
                    _foundAnnoyingUnitLine = true;
                }
            }
            if (_foundAnnoyingUnitLine == true && _foundAnnoyingUnitColumn == true)
            {
                _removeUnit.RemoveUnitAI(_gridManager.gridList[_unitLine[i][_unitColumn[i].Count - 1].tileX][_unitLine[i][_unitLine[i].Count - 1].tileY + 1]);
                pr_state = NodeState.SUCCESS;
            }
        }

        

        pr_state = NodeState.FAILURE;
        return pr_state;
    }
}
