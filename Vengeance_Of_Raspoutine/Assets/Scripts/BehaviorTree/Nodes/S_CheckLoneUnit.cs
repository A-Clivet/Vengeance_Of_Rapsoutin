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


        foreach (List<Unit> u in _gridManager.AllUnitPerColumn)
        {
            _foundLoneUnitLine = true;
            if(u.Count <= 0) 
            {
                continue;
            }
            if (u[u.Count - 1].tileX - 1 >= 0 )
            {
                for (int i = u[u.Count - 1].tileY - 1; i < u[u.Count - 1].tileY + 2; i++)
                {
                    if(i<0 || i >= _gridManager.height)
                    {
                        continue;
                    }
                    if (_gridManager.gridList[u[u.Count - 1].tileX - 1][i].unit != null)
                    {
                        if (_gridManager.gridList[u[u.Count - 1].tileX - 1][i].unit.unitColor == u[u.Count - 1].unitColor)
                        {
                            _foundLoneUnitLine = false; break;
                        }
                    }
                }
                if (!_foundLoneUnitLine)
                {
                    continue;
                }
            }
            if (u[u.Count - 1].tileX + 1 <_gridManager.width)
            {
                for (int i = u[u.Count - 1].tileY - 1; i < u[u.Count - 1].tileY + 2; i++)
                {
                    if (i < 0 || i >= _gridManager.height)
                    {
                        continue;
                    }
                    if (_gridManager.gridList[u[u.Count - 1].tileX + 1][i].unit != null)
                    {
                        if (_gridManager.gridList[u[u.Count - 1].tileX + 1][i].unit.unitColor == u[u.Count - 1].unitColor)
                        {
                            _foundLoneUnitLine = false; break;
                        }
                    }
                }
                if (!_foundLoneUnitLine)
                {
                    continue;
                }
            }
            if(u[u.Count - 1].tileY - 1 >= 0)
            {
                if(u[u.Count - 2].unitColor == u[u.Count - 1].unitColor)
                {
                    _foundLoneUnitLine = false;
                    continue;
                }
            }
            _loneUnit=u[u.Count - 1];
            break;
        }

        if (_foundLoneUnitLine == true)
        {
            pr_state = NodeState.SUCCESS;
            parent.SetData("k_LoneUnit", _loneUnit);
            return pr_state;
        }

        pr_state = NodeState.FAILURE;
        return pr_state;
    }



    
}