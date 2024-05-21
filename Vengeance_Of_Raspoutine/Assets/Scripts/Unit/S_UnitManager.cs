using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnitManager;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private List<List<S_Tile>> gridList;


    public void CheckUnitFormation(Unit lastUnitMoved) { /* refer to note CheckUnitFormation() */
        List<Unit> unitFormation = new();
        unitFormation.Add(lastUnitMoved);

        //for each( /* multi tiled unit, get its closest tile to the grid’s [0,0] and check certain tiles depending on the size */ )
        
        for (int i = 0; i < unitFormation.Count; i++)
        {
            for (int j = 0; j < 3; j++) { 
                switch (j)
                {
                    case 0: /* Up */
                        if (gridList[lastUnitMoved.tileX][lastUnitMoved.tileY].unit.SO_Unit.unitType == gridList[lastUnitMoved.tileX][lastUnitMoved.tileY + 1].unit.SO_Unit.unitType 
                            && 
                            !unitFormation.Contains(gridList[lastUnitMoved.tileX][lastUnitMoved.tileY + 1].unit))
                        {
                            unitFormation.Add(gridList[lastUnitMoved.tileX][lastUnitMoved.tileY].unit);
                        }
                        break;
                    case 1:
                        if(gridList[lastUnitMoved.tileX][lastUnitMoved.tileY].unit.SO_Unit.unitType == gridList[lastUnitMoved.tileX][lastUnitMoved.tileY + 1].unit.SO_Unit.unitType
                                                    &&
                                                    !unitFormation.Contains(gridList[lastUnitMoved.tileX][lastUnitMoved.tileY + 1].unit))
                        {
                            unitFormation.Add(gridList[lastUnitMoved.tileX][lastUnitMoved.tileY].unit);
                        }
                        break;

                    case 2:
                        if(gridList[lastUnitMoved.tileX][lastUnitMoved.tileY].unit.SO_Unit.unitType == gridList[lastUnitMoved.tileX][lastUnitMoved.tileY + 1].unit.SO_Unit.unitType
                                                    &&
                                                    !unitFormation.Contains(gridList[lastUnitMoved.tileX][lastUnitMoved.tileY + 1].unit))
                        {
                            unitFormation.Add(gridList[lastUnitMoved.tileX][lastUnitMoved.tileY].unit);
                        }
                        break;
                    case 3:
                        if (gridList[lastUnitMoved.tileX][lastUnitMoved.tileY].unit.SO_Unit.unitType == gridList[lastUnitMoved.tileX][lastUnitMoved.tileY + 1].unit.SO_Unit.unitType
                            &&
                            !unitFormation.Contains(gridList[lastUnitMoved.tileX][lastUnitMoved.tileY + 1].unit))
                        {
                            unitFormation.Add(gridList[lastUnitMoved.tileX][lastUnitMoved.tileY].unit);
                        }
                        break;
                }
            }
        }
    }

    //if(fusion de multi tiled unit possible ) UnitFusion(Unit bigUnit); 
    //if(unitFormation.count > 1) UnitActivation(unitFormation[], UOC, UOL);

    public void UnitActivation(List<Unit> UF, UnitOnColumn UOC, UnitOnLine UOL) { /* refer to note UnitActivation */
        List<Unit> unitToDefend = new();
        List<Unit> unitToAttack = new();
        int currentIndexY;
        int currentIndexX;
        UOC = new();
        UOL = new();

        /* add the first, since this fucntion isn’t called if the initial unit is secluded */
        UOL.Y.Add(UF[0].tileY);
        UOL.bounds.Add(new Vector2Int(UF[0].tileX, UF[0].tileX));
        UOC.X.Add(UF[0].tileX);
        UOC.bounds.Add(new Vector2Int(UF[0].tileY, UF[0].tileY));

        for (int i = 1; i < UF.Count; i++) {

            currentIndexY = UOL.Y.FindIndex(item => item == UF[i].tileY);
            currentIndexX = UOC.X.FindIndex(item => item == UF[i].tileX);

            UOC.units.Add(UF[i]);
            UOL.units.Add(UF[i]);
            /* check if a List exists for this Line, then, check if the value of the unit’s Tile position is two far off the bounds, if it is, then a new list can be added since it’s disconnected from the initial one */
            if (!UOL.Y.Contains(UF[i].tileY) && (UF[i].tileX < UOL.bounds[currentIndexY].x - 1 || UF[i].tileX > UOL.bounds[currentIndexY].y + 1)){
                UOL.Y.Add(UF[i].tileY);
                UOL.bounds.Add(new Vector2Int(UF[i].tileX, UF[i].tileX));
            }
            else
            {
                if (UOL.bounds[currentIndexY].y > UF[i].tileY)
                {
                    UOL.bounds[currentIndexY] = new Vector2Int(UF[i].tileY, UOL.bounds[currentIndexY].y);
                }
                else if (UOL.bounds[currentIndexY].y < UF[i].tileY)
                {
                    UOL.bounds[currentIndexY] = new Vector2Int(UOL.bounds[currentIndexY].y, UF[i].tileY);
                }
            }
            /* check if a List exists for this Line, then, check if the value of the unit’s Tile position is two far off the bounds, if it is, then a new list can be added since it’s disconnected from the initial one */
            if (!UOC.X.Contains(UF[i].tileX) && (UF[i].tileY > UOC.bounds[UOC.X.FindIndex(item => item == UF[i].tileX)].y + 1 || UF[i].tileY < UOC.bounds[UOC.X.FindIndex(item => item == UF[i].tileX)].x - 1)) {
                UOC.X.Add(UF[i].tileY);
                UOC.bounds.Add(new Vector2Int(UF[i].tileY, UF[i].tileY));
            }
            else
            {
                if (UOL.bounds[currentIndexY].x > UF[i].tileX)
                {
                    UOL.bounds[currentIndexY] = new Vector2Int(UF[i].tileX, UOL.bounds[currentIndexY].x);
                }
                else if (UOL.bounds[currentIndexY].x < UF[i].tileX)
                {
                    UOL.bounds[currentIndexY] = new Vector2Int(UOL.bounds[currentIndexY].x, UF[i].tileX);
                }
            }
        }
        for (int i = 0; i < UOL.Y.Count; i++)
        {
            if (UOL.bounds[i].y - UOL.bounds[i].x < 3) continue; /* if the bounds have a difference of less than 3 then a link cannot be made */
            for (int j = UOL.bounds[i].x; j < UOL.bounds[i].y; j++)
            {
                unitToDefend.Add(gridList[UOL.bounds[i].x + j][UOL.Y[i]].unit);
            }
        }
        for (int i = 0; i < UOC.X.Count; i++)
        {
            if (UOC.bounds[i].y - UOC.bounds[i].x < 3) continue; /* if the bounds have a difference of less than 3 then a link cannot be made */
            for (int j = UOC.bounds[i].x; j < UOC.bounds[i].y; j++)
            {
                unitToAttack.Add(gridList[UOC.X[i]][UOC.bounds[i].x + j].unit);
            }
        }
        Attack(unitToAttack);
        Defend(unitToDefend);
    } 

    public void Attack(List<Unit> attackingUnit) { /* function for what should be done when units are attacking */ 
    
    } 

    public void Defend(List<Unit> defendingUnit) { /* function for what should be done when units are defending */
    
    }

    public struct UnitOnLine{
        public List<Unit> units;
        public List<int> Y;
        public List<Vector2Int> bounds;
        public UnitOnLine(List<Unit> UnitOnLineunits, List<int> UnitOnLineY, List<Vector2Int> UnitOnLinebounds)
        {
            UnitOnLineunits = new List<Unit>();
            UnitOnLineY = new List<int>();
            UnitOnLinebounds = new List<Vector2Int>();
            units = UnitOnLineunits;
            Y = UnitOnLineY;
            bounds = UnitOnLinebounds;
        }
    }

    public struct UnitOnColumn{
        public List<Unit> units;
        public List<int> X;
        public List<Vector2Int> bounds;
        public UnitOnColumn(List<Unit> UnitOnLineunits, List<int> UnitOnLineX, List<Vector2Int> UnitOnLinebounds)
        {
            UnitOnLineunits = new List<Unit>();
            UnitOnLineX = new List<int>();
            UnitOnLinebounds = new List<Vector2Int>();
            units = UnitOnLineunits;
            X = UnitOnLineX;
            bounds = UnitOnLinebounds;
        }
    }
}
