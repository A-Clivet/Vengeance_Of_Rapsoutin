using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static S_UnitManager;

public class S_UnitManager : MonoBehaviour
{
    public S_GridManager grid;
    [SerializeField] private List<List<S_Tile>> gridList;
    [SerializeField] List<Unit> unitFormation = new();
    [SerializeField] private List<Unit> UnitLine;
    [SerializeField] private List<Unit> UnitColumn;

    public void Start()
    {
        gridList = grid.gridList;
    }

    public void CheckUnitFormation(Unit p_lastUnitMoved) {
        if(!unitFormation.Contains(p_lastUnitMoved)) unitFormation.Add(p_lastUnitMoved);

        //for each( /* multi tiled unit, get its closest tile to the grid’s [0,0] and check certain tiles depending on the size */ )

        for (int i = 0; i < unitFormation.Count; i++)
        {
            for (int j = 0; j < 4; j++) { 
                switch (j) // mettre en fonciton
                {
                    case 0: // Up
                        if (p_lastUnitMoved.tileY + 1 == grid.height) break;

                        if (gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY + 1].unit == null) break;

                        if (gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY].unit.SO_Unit.unitType == gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY + 1].unit.SO_Unit.unitType
                            &&
                            !unitFormation.Contains(gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY + 1].unit))
                        {
                            unitFormation.Add(gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY + 1].unit);
                            Debug.Log("up");
                            CheckUnitFormation(gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY + 1].unit);
                        }
                        break;

                    case 1: // right

                        if (p_lastUnitMoved.tileX + 1 == grid.width) break;

                        if (gridList[p_lastUnitMoved.tileX + 1][p_lastUnitMoved.tileY].unit == null) break;

                        if (gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY].unit.SO_Unit.unitType == gridList[p_lastUnitMoved.tileX + 1][p_lastUnitMoved.tileY].unit.SO_Unit.unitType
                            &&
                            !unitFormation.Contains(gridList[p_lastUnitMoved.tileX + 1][p_lastUnitMoved.tileY].unit))
                        {
                            unitFormation.Add(gridList[p_lastUnitMoved.tileX + 1][p_lastUnitMoved.tileY].unit);
                            Debug.Log("right");
                            CheckUnitFormation(gridList[p_lastUnitMoved.tileX + 1][p_lastUnitMoved.tileY].unit);
                        }
                        break;

                    case 2: // down

                        if (p_lastUnitMoved.tileY - 1 == -1) break;

                        if (gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY - 1].unit == null) break;

                        if (gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY].unit.SO_Unit.unitType == gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY - 1].unit.SO_Unit.unitType
                            &&
                            !unitFormation.Contains(gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY - 1].unit))
                        {
                            unitFormation.Add(gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY - 1].unit);
                            Debug.Log("down");
                            CheckUnitFormation(gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY - 1].unit);
                        }
                        break;

                    case 3: // left
                        if (p_lastUnitMoved.tileX - 1 == -1) break;

                        if (gridList[p_lastUnitMoved.tileX - 1][p_lastUnitMoved.tileY].unit == null) break;

                        if (gridList[p_lastUnitMoved.tileX][p_lastUnitMoved.tileY].unit.SO_Unit.unitType == gridList[p_lastUnitMoved.tileX - 1][p_lastUnitMoved.tileY].unit.SO_Unit.unitType
                            &&
                            !unitFormation.Contains(gridList[p_lastUnitMoved.tileX - 1][p_lastUnitMoved.tileY].unit))
                        {
                            unitFormation.Add(gridList[p_lastUnitMoved.tileX - 1][p_lastUnitMoved.tileY].unit);
                            Debug.Log("left");
                            CheckUnitFormation(gridList[p_lastUnitMoved.tileX - 1][p_lastUnitMoved.tileY].unit);
                        }
                        break;

                }
            }
        }
        UnitActivation(unitFormation);
    }


    //if(fusion de multi tiled unit possible ) UnitFusion(Unit bigUnit); 
    //if(unitFormation.count > 1) UnitActivation(unitFormation[], UOC, UOL);

    public void UnitActivation(List<Unit> p_UF) { /* refer to note UnitActivation */
        List<Unit> unitToDefend = new();
        List<Unit> unitToAttack = new();
        
        int currentIndexY = 0;
        int currentIndexX = 0;
        
        UnitOnColumn UOC = new();
        UnitOnLine UOL = new();
        
        UOC.units = new();
        UOC.X = new();
        UOC.bounds = new();

        UOL.units = new();
        UOL.Y = new();
        UOL.bounds = new();

        for (int i = 0; i < p_UF.Count; i++) {

            currentIndexX = UOC.X.FindIndex(X => X == p_UF[i].tileX);
            currentIndexY = UOL.Y.FindIndex(Y => Y == p_UF[i].tileY);

            //Debug.Log("#######");
            //Debug.Log(p_UF[i].tileX);
            //Debug.Log(p_UF[i].tileY);
            Debug.Log(";;;;;;;;;;");
            Debug.Log(UOC.bounds[currentIndexX]);
            Debug.Log(UOL.bounds[currentIndexY]);
            Debug.Log("/////");

            /* check if a List exists for this Line, then, check if the value of the unit’s Tile position is too far off the bounds, if it is, then a new list can be added since it’s disconnected from the initial one */
            if (!UOL.Y.Contains(p_UF[i].tileY) && (p_UF[i].tileX < UOL.bounds[currentIndexY].x - 1 || p_UF[i].tileX > UOL.bounds[currentIndexY].y + 1)){
                Debug.Log("Is added Line");
                UOL.units.Add(p_UF[i]);
                UOL.Y.Add(p_UF[i].tileY);
                UOL.bounds.Add(new Vector2Int(p_UF[i].tileX, p_UF[i].tileX));
            }
            else
            {
                if (UOL.bounds[currentIndexY].y > p_UF[i].tileX)
                {
                    UOL.bounds[currentIndexY] = new Vector2Int(p_UF[i].tileY, UOL.bounds[currentIndexY].y);
                }
                else if (UOL.bounds[currentIndexY].x < p_UF[i].tileX)
                {
                    UOL.bounds[currentIndexY] = new Vector2Int(UOL.bounds[currentIndexY].y, p_UF[i].tileY);
                }
            }
            Debug.Log(currentIndexX);
            /* check if a List exists for this Line, then, check if the value of the unit’s Tile position is too far off the bounds, if it is, then a new list can be added since it’s disconnected from the initial one */
            if (!UOC.X.Contains(p_UF[i].tileX) && (p_UF[i].tileY > UOC.bounds[currentIndexX].y + 1 || p_UF[i].tileY < UOC.bounds[currentIndexX].x - 1)) {
                Debug.Log("Is Added Column");
                UOC.units.Add(p_UF[i]);
                UOC.X.Add(p_UF[i ].tileX);
                UOC.bounds.Add(new Vector2Int(p_UF[i].tileY, p_UF[i].tileY));
            }
            else
            {
                if (UOL.bounds[currentIndexX].y > p_UF[i].tileY)
                {
                    UOL.bounds[currentIndexX] = new Vector2Int(p_UF[i].tileX, UOL.bounds[currentIndexX].y);
                }
                else if (UOL.bounds[currentIndexX].x < p_UF[i].tileY)
                {
                    UOL.bounds[currentIndexX] = new Vector2Int(UOL.bounds[currentIndexX].x, p_UF[i].tileX);
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

    public void Attack(List<Unit> p_attackingUnit) { /* function for what should be done when units are attacking */
        UnitLine = p_attackingUnit;
        for (int i = 0; i < p_attackingUnit.Count; i++)
        {
            Debug.Log(p_attackingUnit[i].tileX);
            Debug.Log(p_attackingUnit[i].tileY);
        }
    } 

    public void Defend(List<Unit> p_defendingUnit) { /* function for what should be done when units are defending */
        UnitColumn = p_defendingUnit;
        for (int i = 0; i < p_defendingUnit.Count; i++)
        {
            Debug.Log(p_defendingUnit[i].tileX);
            Debug.Log(p_defendingUnit[i].tileY);
        }
    }

    public struct UnitOnLine{
        public List<Unit> units;
        public List<int> Y;
        public List<Vector2Int> bounds;
        //public UnitOnLine(List<Unit> UnitOnLineunits, List<int> UnitOnLineY, List<Vector2Int> UnitOnLinebounds)
        //{
        //    UnitOnLineunits = new();
        //    UnitOnLineY = new();
        //    UnitOnLinebounds = new();
        //    units = UnitOnLineunits;
        //    Y = UnitOnLineY;
        //    bounds = UnitOnLinebounds;
        //}
    }

    public struct UnitOnColumn{
        public List<Unit> units;
        public List<int> X;
        public List<Vector2Int> bounds;
        //public UnitOnColumn(List<Unit> UnitOnLineunits, List<int> UnitOnLineX, List<Vector2Int> UnitOnLinebounds)
        //{
        //    UnitOnLineunits = new();
        //    UnitOnLineX = new();
        //    UnitOnLinebounds = new();
        //    units = UnitOnLineunits;
        //    X = UnitOnLineX;
        //    bounds = UnitOnLinebounds;
        //}
    }
}
