using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnitManager;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private List<List<S_Tile>> gridList;


    public void CheckUnitFormation(Unit p_lastUnitMoved) { /* refer to note CheckUnitFormation() */
        List<Unit> unitFormation = new();
        unitFormation.Add(p_lastUnitMoved);

        //for each( /* multi tiled unit, get its closest tile to the grid’s [0,0] and check certain tiles depending on the size */ )
        
        for (int i = 0; i < unitFormation.Count; i++)
        {
            for (int j = 0; j < 3; j++) { 
                switch (j)
                {
                    case 0: /* Up */
                        if (gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY].unit.SO_Unit.unitType == gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY + 1].unit.SO_Unit.unitType 
                            && 
                            !unitFormation.Contains(gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY + 1].unit))
                        {
                            unitFormation.Add(gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY].unit);
                        }
                        break;
                    case 1:
                        if(gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY].unit.SO_Unit.unitType == gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY + 1].unit.SO_Unit.unitType
                                                    &&
                                                    !unitFormation.Contains(gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY + 1].unit))
                        {
                            unitFormation.Add(gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY].unit);
                        }
                        break;

                    case 2:
                        if(gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY].unit.SO_Unit.unitType == gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY + 1].unit.SO_Unit.unitType
                                                    &&
                                                    !unitFormation.Contains(gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY + 1].unit))
                        {
                            unitFormation.Add(gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY].unit);
                        }
                        break;
                    case 3:
                        if (gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY].unit.SO_Unit.unitType == gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY + 1].unit.SO_Unit.unitType
                            &&
                            !unitFormation.Contains(gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY + 1].unit))
                        {
                            unitFormation.Add(gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY].unit);
                        }
                        break;
                }
            }
        }
    }

    //if(fusion de multi tiled unit possible ) UnitFusion(Unit bigUnit); 
    //if(unitFormation.count > 1) UnitActivation(unitFormation[], UOC, UOL);

    public void UnitActivation(List<Unit> p_UF, UnitOnColumn p_UOC, UnitOnLine p_UOL) { /* refer to note UnitActivation */
        List<Unit> unitToDefend = new();
        List<Unit> unitToAttack = new();
        int currentIndexY;
        int currentIndexX;
        p_UOC = new();
        p_UOL = new();

        /* add the first, since this fucntion isn’t called if the initial unit is secluded */
        p_UOL.Y.Add(p_UF[0].tileY);
        p_UOL.bounds.Add(new Vector2Int(p_UF[0].tileX, p_UF[0].tileX));
        p_UOC.X.Add(p_UF[0].tileX);
        p_UOC.bounds.Add(new Vector2Int(p_UF[0].tileY, p_UF[0].tileY));

        for (int i = 1; i < p_UF.Count; i++) {

            currentIndexY = p_UOL.Y.FindIndex(item => item == p_UF[i].tileY);
            currentIndexX = p_UOC.X.FindIndex(item => item == p_UF[i].tileX);

            p_UOC.units.Add(p_UF[i]);
            p_UOL.units.Add(p_UF[i]);
            /* check if a List exists for this Line, then, check if the value of the unit’s Tile position is two far off the bounds, if it is, then a new list can be added since it’s disconnected from the initial one */
            if (!p_UOL.Y.Contains(p_UF[i].tileY) && (p_UF[i].tileX < p_UOL.bounds[currentIndexY].x - 1 || p_UF[i].tileX > p_UOL.bounds[currentIndexY].y + 1)){
                p_UOL.Y.Add(p_UF[i].tileY);
                p_UOL.bounds.Add(new Vector2Int(p_UF[i].tileX, p_UF[i].tileX));
            }
            else
            {
                if (p_UOL.bounds[currentIndexY].y > p_UF[i].tileY)
                {
                    p_UOL.bounds[currentIndexY] = new Vector2Int(p_UF[i].tileY, p_UOL.bounds[currentIndexY].y);
                }
                else if (p_UOL.bounds[currentIndexY].y < p_UF[i].tileY)
                {
                    p_UOL.bounds[currentIndexY] = new Vector2Int(p_UOL.bounds[currentIndexY].y, p_UF[i].tileY);
                }
            }
            /* check if a List exists for this Line, then, check if the value of the unit’s Tile position is two far off the bounds, if it is, then a new list can be added since it’s disconnected from the initial one */
            if (!p_UOC.X.Contains(p_UF[i].tileX) && (p_UF[i].tileY > p_UOC.bounds[p_UOC.X.FindIndex(item => item == p_UF[i].tileX)].y + 1 || p_UF[i].tileY < p_UOC.bounds[p_UOC.X.FindIndex(item => item == p_UF[i].tileX)].x - 1)) {
                p_UOC.X.Add(p_UF[i].tileY);
                p_UOC.bounds.Add(new Vector2Int(p_UF[i].tileY, p_UF[i].tileY));
            }
            else
            {
                if (p_UOL.bounds[currentIndexY].x > p_UF[i].tileX)
                {
                    p_UOL.bounds[currentIndexY] = new Vector2Int(p_UF[i].tileX, p_UOL.bounds[currentIndexY].x);
                }
                else if (p_UOL.bounds[currentIndexY].x < p_UF[i].tileX)
                {
                    p_UOL.bounds[currentIndexY] = new Vector2Int(p_UOL.bounds[currentIndexY].x, p_UF[i].tileX);
                }
            }
        }
        for (int i = 0; i < p_UOL.Y.Count; i++)
        {
            if (p_UOL.bounds[i].y - p_UOL.bounds[i].x < 3) continue; /* if the bounds have a difference of less than 3 then a link cannot be made */
            for (int j = p_UOL.bounds[i].x; j < p_UOL.bounds[i].y; j++)
            {
                unitToDefend.Add(gridList[p_UOL.bounds[i].x + j][p_UOL.Y[i]].unit);
            }
        }
        for (int i = 0; i < p_UOC.X.Count; i++)
        {
            if (p_UOC.bounds[i].y - p_UOC.bounds[i].x < 3) continue; /* if the bounds have a difference of less than 3 then a link cannot be made */
            for (int j = p_UOC.bounds[i].x; j < p_UOC.bounds[i].y; j++)
            {
                unitToAttack.Add(gridList[p_UOC.X[i]][p_UOC.bounds[i].x + j].unit);
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
