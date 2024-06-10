using System.Collections.Generic;
using S_BehaviorTree;
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
        bool _foundLoneUnitLine = false;
        
        
        for(int i = 0; i < _gridManager.AllUnitPerColumn.Count; i++)
        {
            _foundLoneUnitLine = false;

            for (int j = 1; j < 3; j++)
            {
                if (_gridManager.AllUnitPerColumn[i].Count - j >= 0)        //Allows to avoid a Out of Index Error
                {
                    if (_gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].SO_Unit.unitType == _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - j].SO_Unit.unitType)     //Allows to Check if the top Unit has any Unit of the same type below it
                    {
                        continue;
                    }

                    if (_gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1] != null)      //Allows
                    {
                        for (int h = 1; h < 3; h++)
                        {
                            if (_gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].tileX - h >= 0 && _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].tileX + h < _gridManager.width)
                            {
                                if (_gridManager.gridList[i - h][_gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].tileY].unit != null)
                                {
                                    if (_gridManager.gridList[i - h][_gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].tileY].unit.SO_Unit.unitType == _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].SO_Unit.unitType)
                                    {
                                        continue;
                                    }

                                    _foundLoneUnitLine = true;
                                    _loneUnit = _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1];       //Allows to _loneUnit to take the Values of the Lone Unit Found
                                    break;
                                }

                                if (_gridManager.gridList[i + h][_gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].tileY].unit != null)
                                {
                                    if (_gridManager.gridList[i + h][_gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].tileY].unit.SO_Unit.unitType == _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].SO_Unit.unitType)
                                    {
                                        continue;
                                    }

                                    _foundLoneUnitLine = true;
                                    _loneUnit = _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1];       //Allows to _loneUnit to take the Values of the Lone Unit Found
                                    break;
                                }

                                _foundLoneUnitLine = true;
                                _loneUnit = _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1];       //Allows to _loneUnit to take the Values of the Lone Unit Found
                                break;
                            }
                        }

                        if (_foundLoneUnitLine)
                        {
                            break;
                        }
                    }
                }

                if (_foundLoneUnitLine)
                {
                    break;
                }
            }
        }

        if (_foundLoneUnitLine == true)
        {
            pr_state = NodeState.SUCCESS;
            SetData("k_LoneUnit", _loneUnit);
            return pr_state;
        }

        pr_state = NodeState.FAILURE;
        return pr_state;
    }



    
}