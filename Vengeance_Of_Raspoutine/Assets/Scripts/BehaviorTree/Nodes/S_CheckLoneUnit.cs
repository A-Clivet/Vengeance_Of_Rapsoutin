using System.Collections.Generic;
using BehaviorTree;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_CheckLoneUnit : Node
{
    private S_GridManager _gridManager;
    private Unit _loneUnit;

    public S_CheckLoneUnit(S_GridManager p_gridManager)
    {
        _gridManager = p_gridManager;
    }

    public override NodeState Evaluate()
    {
        bool _foundLoneUnitColumn = false;
        bool _foundLoneUnitLine = false;
        
        
        for(int i = 0; i < _gridManager.AllUnitPerColumn.Count; i++)
        {
            _foundLoneUnitColumn = false;
            _foundLoneUnitLine = false;

            for (int j = 2; j < 4; j++)
            {
                if (_gridManager.AllUnitPerColumn[i].Count - j < 0)
                {
                    if (_gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].SO_Unit.unitType == _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - j].SO_Unit.unitType)
                    {
                        continue;
                    }
                    _foundLoneUnitColumn = true;
                }
            }

            for(int j = 2; j < 4; j++) 
            {
                if (_gridManager.gridList[i - j].Count - 2 < 0 || _gridManager.gridList[i + j].Count - 2 > _gridManager.gridList.Count - 1 || _gridManager.gridList[i - j][_gridManager.AllUnitPerColumn.Count - 2].unit != null || _gridManager.gridList[i + j][_gridManager.AllUnitPerColumn.Count - 2].unit != null)
                {
                    if (_gridManager.AllUnitPerColumn[i - j][_gridManager.AllUnitPerColumn[i].Count - 2].SO_Unit.unitType == _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].SO_Unit.unitType)
                    {
                        continue;
                    }

                    if (_gridManager.AllUnitPerColumn[i + j][_gridManager.AllUnitPerColumn[i].Count - 2].SO_Unit.unitType == _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].SO_Unit.unitType)
                    {
                        continue;
                    }

                    _foundLoneUnitLine = true;
                }
            }

            if (_foundLoneUnitColumn == true && _foundLoneUnitLine == true)
            {
                _loneUnit = _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1];
            }
        }
            
        if (_foundLoneUnitColumn == true && _foundLoneUnitLine == true)
        {
            pr_state = NodeState.SUCCESS;
            SetData("k_LoneUnit", _loneUnit);
            return pr_state;
        }

        pr_state = NodeState.FAILURE;
        return pr_state;
    }

    
}