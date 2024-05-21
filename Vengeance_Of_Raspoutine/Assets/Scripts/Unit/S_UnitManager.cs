using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitManager;

public class UnitManager : MonoBehaviour
{

    public void CheckUnitFormation(Unit lastunitMoved) { /* refer to note CheckUnitFormation() */ 
    
    }

    public void UnitActivation(List<Unit> UF, UnitOnColumn UOC, UnitOnLine UOL) { /* refer to note UnitActivation */
    
    } 

    public void Attack(List<Unit> attackingUnit) { /* function for what should be done when units are attacking */ 
    
    } 

    public void Defend(List<Unit> defendingUnit) { /* function for what should be done when units are defending */
    
    }

    public struct UnitOnLine{
        public List<Unit> units;
        public List<int> X;
        public List<Vector2> bounds;
        public UnitOnLine(List<Unit> UnitOnLineunits, List<int> UnitOnLineX, List<Vector2> UnitOnLinebounds)
        {
            UnitOnLineunits = new List<Unit>();
            UnitOnLineX = new List<int>();
            UnitOnLinebounds = new List<Vector2>();
            units = UnitOnLineunits;
            X = UnitOnLineX;
            bounds = UnitOnLinebounds;
        }
    }

    public struct UnitOnColumn{
        public List<Unit> units;
        public List<int> Y;
        public List<Vector2> bounds;
        public UnitOnColumn(List<Unit> UnitOnLineunits, List<int> UnitOnLineY, List<Vector2> UnitOnLinebounds)
        {
            UnitOnLineunits = new List<Unit>();
            UnitOnLineY = new List<int>();
            UnitOnLinebounds = new List<Vector2>();
            units = UnitOnLineunits;
            Y = UnitOnLineY;
            bounds = UnitOnLinebounds;
        }
    }
}
