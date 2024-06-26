using System.Collections.Generic;
using System.Linq;
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
        List<Unit> _inColumnUnit = new List<Unit>();
        for (int i = 0; i < _gridManager.unitManager.UnitColumn.Count; i++)
        {
            foreach (Unit u in _gridManager.unitManager.UnitColumn[i])
            {
                _inColumnUnit.Add(u);
            }
        }

        foreach (Unit u in _gridManager.unitList)
        {
            int yOfUnit = -1;
            if (_inColumnUnit.Contains(u))
            {
                continue;
            }
            _foundLoneUnitLine = true;
            for (int i = -1; i < 2; i++)
            {
                if (u.CheckUnitInProximity(out var unit, -1, i))
                {
                    if (unit.unitColor == u.unitColor && !(_inColumnUnit.Contains(unit)))
                    {
                        yOfUnit=i; break;
                    }
                }
            }
            for (int i = -1; i < 2; i++)
            {

                if (u.CheckUnitInProximity(out var unit2, 1, i))
                {
                    if (unit2.unitColor == u.unitColor && !(_inColumnUnit.Contains(unit2)))
                    {
                        if (yOfUnit==i)
                        {
                            _foundLoneUnitLine = false; break;
                        }
                    }
                }
            }
            if (!_foundLoneUnitLine)
            {
                continue;
            }


            if (u.CheckUnitInProximity(out var unit3, 0, -1))
            {
                if (unit3.unitColor == u.unitColor && !(_inColumnUnit.Contains(unit3)))
                {
                    _foundLoneUnitLine = false;
                    continue;
                }
            }
            _loneUnit = u;
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