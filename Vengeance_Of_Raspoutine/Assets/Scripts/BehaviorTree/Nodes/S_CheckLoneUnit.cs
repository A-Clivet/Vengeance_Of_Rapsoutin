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
        
        
        for(int i = 0; i < _gridManager.AllUnitPerColumn.Count; i++)    //Allows to go through the grid by Collumns
        {
            _foundLoneUnitLine = false;

            for (int j = 1; j < 3; j++)
            {
                if (_gridManager.AllUnitPerColumn[i].Count - j >= 0)        //Allows to go through the grid by Lines and avoid the get out of the list
                {
                    if (_gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].SO_Unit.unitType == _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - j].SO_Unit.unitType && _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].unitColor== _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - j].unitColor)     //Allows to Check if the last Unit has any Unit of the same type below it
                    {
                        continue;
                    }

                    if (_gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1] != null)      //Allows to check if the last unit tile of the column is not empty
                    {
                        for (int h = 1; h < 3; h++)
                        {
                            if (_gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].tileX - h >= 0 && _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].tileX + h < _gridManager.width)     //Allows to check to grid line by line and avoid to get out of the grid
                            {
                                if (_gridManager.gridList[i - h][_gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].tileY].unit != null)      //
                                {
                                    if (_gridManager.gridList[i - h][_gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].tileY].unit.SO_Unit.unitType == _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].SO_Unit.unitType && _gridManager.gridList[i - h][_gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].tileY].unit.unitColor == _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].unitColor)      //Checks if the unit Below the current unit that we are on is the same type
                                    {
                                        continue;
                                    }

                                    _foundLoneUnitLine = true;
                                    _loneUnit = _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1];       //Allows _loneUnit to take the Values of the lone unit found
                                    break;
                                }

                                if (_gridManager.gridList[i + h][_gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].tileY].unit != null)      //Allows to check both sides of the unit we are checking and if they are not null
                                {
                                    if (_gridManager.gridList[i + h][_gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].tileY].unit.SO_Unit.unitType == _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].SO_Unit.unitType && _gridManager.gridList[i + h][_gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].tileY].unit.unitColor == _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1].unitColor)      //Checks if the unit we are currently checking has any unit of of the same type next to it (Left and Right)
                                    {
                                        continue;
                                    }

                                    _foundLoneUnitLine = true;
                                    _loneUnit = _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1];       //Allows _loneUnit to take the Values of the lone unit found
                                    break;
                                }

                                _foundLoneUnitLine = true;
                                _loneUnit = _gridManager.AllUnitPerColumn[i][_gridManager.AllUnitPerColumn[i].Count - 1];       //Allows _loneUnit to take the Values of the lone unit found
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