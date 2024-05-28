using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class S_UnitManager : MonoBehaviour
{
    public S_GridManager grid;
    private List<List<S_Tile>> gridList;
    [SerializeField] private List<Unit> unitFormation = new();
    public List<List<Unit>> UnitLine = new();
    public List<List<Unit>> UnitColumn = new();
    public Sprite defendImg;


    public void Start()
    {
        gridList = grid.gridList;
    }

    public void UnitCombo(int p_formationNumber)
    {

        int columnCounter = 0;
        int lineCounter = 0;

        //SO_Unit actualType = null;

        for (int i = 0; i < grid.width; i++) // largeur
        {
            for (int j = 0; j < Mathf.Abs(grid.height); j++) // hauteur
            {
                if (gridList[i][j].unit == null /* && unit state*/ )
                {
                    columnCounter = 0;
                    continue;
                }
                //if (gridList[i][j].unit.SO_Unit != null)
                //{
                //    columnCounter = 1;
                //    //tualType = gridList[i][j].unit.SO_Unit;
                //}
                else
                {
                    columnCounter++;
                }

                if(columnCounter == 3)
                {
                    UnitColumn.Add(new());
                    UnitColumn[UnitColumn.Count - 1].Add(gridList[i][j].unit);
                    UnitColumn[UnitColumn.Count - 1].Add(gridList[i][j-1].unit);
                    UnitColumn[UnitColumn.Count - 1].Add(gridList[i][j-2].unit);
                    columnCounter = 0;
                }
            }
            columnCounter = 0;
        }

        for (int i = 0; i < Mathf.Abs(grid.height); i++) // hateur
        {
            for (int j = 0; j < grid.width; j++) // largeur
            {
                if (gridList[j][i].unit == null)
                {
                    lineCounter = 0;
                    continue;
                }
                //if (gridList[j][i].unit.SO_Unit != null)
                //{
                //    lineCounter = 1;
                //    //actualType = gridList[j][i].unit.SO_Unit;
                //}
                else
                {
                    lineCounter++;
                }

                if (lineCounter == p_formationNumber)
                {
                    UnitLine.Add(new());
                    UnitLine[UnitLine.Count - 1].Add(gridList[j][i].unit);
                    UnitLine[UnitLine.Count - 1].Add(gridList[j - 1][i].unit);
                    UnitLine[UnitLine.Count - 1].Add(gridList[j - 2][i].unit);
                    lineCounter = 0;
                }
            }
            lineCounter = 0;
        }

        if(UnitLine.Count >= 1)
        {
            Defend(UnitLine);
        }
        if(UnitColumn.Count >= 1)
        {
            Attack(UnitColumn);
        }
    }



    //public void CheckUnitFormation(Unit p_lastUnitMoved) {

    //    Vector4 adjUnitList = new Vector4(0, 0, 0, 0);

    //    int unitPosX = p_lastUnitMoved.tileX;
    //    int unitPosY = p_lastUnitMoved.tileY;

    //    if (!unitFormation.Contains(gridList[unitPosX][unitPosY].unit))
    //    {
    //        unitFormation.Add(gridList[unitPosX][unitPosY].unit);
    //    }
    //    //for each( /* multi tiled unit, get its closest tile to the grid’s [0,0] and check certain tiles depending on the size */ )

    //    for (int i = 0; i < unitFormation.Count; i++)
    //    {
    //        for (int j = 0; j < 4; j++) { 
    //            switch (j) 
    //            {
    //                case 0: // Up
    //                    if (unitPosY + 1 == grid.height) break;

    //                    if (gridList[unitPosX][unitPosY + 1].unit == null) break;

    //                    if (gridList[unitPosX][unitPosY].unit.SO_Unit.unitType == gridList[unitPosX][unitPosY + 1].unit.SO_Unit.unitType
    //                        &&
    //                        !unitFormation.Contains(gridList[unitPosX][unitPosY + 1].unit))
    //                    {
    //                        unitFormation.Add(gridList[unitPosX][unitPosY + 1].unit);
    //                        adjUnitList.x = 1;
    //                    }
    //                    break;

    //                case 1: // right

    //                    if (unitPosX + 1 == grid.width) break;

    //                    if (gridList[unitPosX + 1][unitPosY].unit == null) break;

    //                    if (gridList[unitPosX][unitPosY].unit.SO_Unit.unitType == gridList[unitPosX + 1][unitPosY].unit.SO_Unit.unitType
    //                        &&
    //                        !unitFormation.Contains(gridList[unitPosX + 1][unitPosY].unit))
    //                    {
    //                        unitFormation.Add(gridList[unitPosX + 1][unitPosY].unit);
    //                        adjUnitList.y = 1;
    //                    }
    //                    break;

    //                case 2: // down

    //                    if (unitPosY - 1 == -1) break;

    //                    if (gridList[unitPosX][unitPosY - 1].unit == null) break;

    //                    if (gridList[unitPosX][unitPosY].unit.SO_Unit.unitType == gridList[unitPosX][unitPosY - 1].unit.SO_Unit.unitType
    //                        &&
    //                        !unitFormation.Contains(gridList[unitPosX][unitPosY - 1].unit))
    //                    {
    //                        unitFormation.Add(gridList[unitPosX][unitPosY - 1].unit);
    //                        adjUnitList.z = 1;
    //                    }
    //                    break;

    //                case 3: // left
    //                    if (unitPosX - 1 == -1) break;

    //                    if (gridList[unitPosX - 1][unitPosY].unit == null) break;

    //                    if (gridList[unitPosX][unitPosY].unit.SO_Unit.unitType == gridList[unitPosX - 1][unitPosY].unit.SO_Unit.unitType
    //                        &&
    //                        !unitFormation.Contains(gridList[unitPosX - 1][unitPosY].unit))
    //                    {
    //                        unitFormation.Add(gridList[unitPosX - 1][unitPosY].unit);
    //                        adjUnitList.w = 1;
    //                    }
    //                    break;

    //                default:
    //                    break;
    //            }
    //        }
    //    }
    //    for(int i = 0; i < 4 ; i++) 
    //    {
    //        if(adjUnitList.x == 1)
    //        {
    //            CheckUnitFormation(gridList[unitPosX][unitPosY + 1].unit);

    //        }
    //        if (adjUnitList.y == 1)
    //        {
    //            CheckUnitFormation(gridList[unitPosX + 1][unitPosY].unit);
    //        }
    //        if (adjUnitList.z == 1)
    //        {
    //            CheckUnitFormation(gridList[unitPosX][unitPosY - 1].unit);
    //        }
    //        if (adjUnitList.w == 1)
    //        {
    //            CheckUnitFormation(gridList[unitPosX -1 ][unitPosY].unit);
    //        }
    //    }
    //    if(p_lastUnitMoved == unitFormation.Last())
    //    {
    //        UnitActivation(unitFormation);
    //    }
    //}


    //if(fusion de multi tiled unit possible ) UnitFusion(Unit bigUnit); 
    //if(unitFormation.count > 1) UnitActivation(unitFormation[], UOC, UOL);

    //public void UnitActivation(List<Unit> p_UF) { /* refer to note UnitActivation */

    //    //int currentIndexY = 0;
    //    //int currentIndexX = 0;

    //    //UnitLine = new();
    //    //UnitColumn = new();
        
    //    //UnitOnColumn UOC = new();
    //    //UnitOnLine UOL = new();
        
    //    //UOC.units = new();
    //    //UOC.X = new();
    //    //UOC.bounds = new();

    //    //UOL.units = new();
    //    //UOL.Y = new();
    //    //UOL.bounds = new();

    //    //UOL.units.Insert(0,p_UF[0]);
    //    //UnitLine.Add(UOL.units[0]);
    //    //UOL.bounds.Insert(0,new Vector2Int(UOL.units[0].tileX, UOL.units[0].tileX));

    //    //UOC.units.Insert(0,p_UF[0]);
    //    //UnitColumn.Add(UOC.units[0]);
    //    //UOC.bounds.Insert(0,new Vector2Int(UOL.units[0].tileY, UOL.units[0].tileY));

    //    //int refIndexL = 0;
    //    //int refIndexC = 0;


    //    for (int i = 1; i < p_UF.Count; i++)
    //    {

    //        //    if (!UOL.units.Contains(p_UF[i]))
    //        //    {
    //        //        Debug.Log("Added unit line");

    //        //        UOL.units.Insert(i, p_UF[i]);

    //        //        if (!UOL.Y.Contains(p_UF[i].tileY))
    //        //        {
    //        //            UOL.Y.Add(UOL.units[i].tileY);
    //        //            refIndexL = UOL.Y.FindIndex(item => item == p_UF[i].tileY);
    //        //        }

    //        //        if (UOL.units[refIndexL].tileY != UOL.units[i].tileY) // à revoir 
    //        //        {
    //        //            Debug.Log("refIndexL : " + refIndexL);
    //        //            //UOL.y à faire je crois ? 
    //        //            UOL.bounds.Add(new Vector2Int(UOL.units[i].tileX, UOL.units[i].tileX));
    //        //            UnitLine.Add(UOL.units[i]);
    //        //        }
    //        //        else if (UOL.units[refIndexL].tileY == UOL.units[i].tileY)
    //        //        {
    //        //            if (UOL.Y[refIndexL] == p_UF[i].tileY)
    //        //            {
    //        //                if (UOL.bounds[refIndexL].x > p_UF[i].tileX - 1)
    //        //                {
    //        //                    // ajouter un UOL.Y ???
    //        //                    UOL.bounds.Add(new Vector2Int(UOL.units[i].tileX, UOL.units[i].tileX));
    //        //                }
    //        //                if (UOL.bounds[refIndexL].y < p_UF[i].tileX + 1)
    //        //                {
    //        //                    // ajouter un UOL.Y ???
    //        //                    UOL.bounds.Add(new Vector2Int(UOL.units[i].tileX, UOL.units[i].tileX));
    //        //                }


    //        //                if (UOL.bounds[refIndexL].x == p_UF[i].tileX - 1)
    //        //                {
    //        //                    UOL.bounds[refIndexL] = new Vector2Int(p_UF[i].tileX, UOL.bounds[refIndexL].y);
    //        //                }
    //        //                if (UOL.bounds[refIndexL].y == p_UF[i].tileX + 1)
    //        //                {
    //        //                    UOL.bounds[refIndexL] = new Vector2Int(UOL.bounds[refIndexL].x, p_UF[i].tileX);
    //        //                }
    //        //            }

    //        //            Debug.Log(UOL.bounds[refIndexL]);
    //        //            Debug.Log(UOL.units[i].tileX);
    //        //            Debug.Log(UOL.units[i].tileY);
    //        //            //if()
    //        //            UnitLine.Add(UOL.units[i]);
    //        //        }
    //        //    }

    //        //    if (!UOC.units.Contains(p_UF[i]))
    //        //    {
    //        //        Debug.Log("Added unit column");

    //        //        UOC.units.Insert(i, p_UF[i]);

    //        //        if (!UOC.X.Contains(p_UF[i].tileX))
    //        //        {
    //        //            UOC.X.Add(UOC.units[i].tileX);

    //        //            refIndexC = UOC.X.FindIndex(item => item == p_UF[i].tileX);
    //        //        }

    //        //        if (UOC.units[refIndexC].tileX != UOC.units[i].tileX)
    //        //        {
    //        //            Debug.Log(refIndexC);

    //        //            UOC.bounds.Add(new Vector2Int(UOC.units[i].tileY, UOC.units[i].tileY));

    //        //            UnitColumn.Add(UOC.units[i]);

    //        //        } 
    //        //        else if (UOC.units[refIndexC].tileX == UOC.units[i].tileX)
    //        //        {

    //        //            if (UOC.X[refIndexC] == p_UF[i].tileX)
    //        //            {
    //        //                if (UOC.bounds[refIndexC].x > p_UF[i].tileY - 1)
    //        //                {
    //        //                    UOC.bounds.Add(new Vector2Int(UOL.units[i].tileY, UOL.units[i].tileY));
    //        //                }
    //        //                if (UOC.bounds[refIndexC].y < p_UF[i].tileY + 1)
    //        //                {
    //        //                    UOC.bounds.Add(new Vector2Int(UOL.units[i].tileY, UOL.units[i].tileY));
    //        //                }

    //        //                if (UOC.bounds[refIndexC].x == p_UF[i].tileY - 1)
    //        //                {
    //        //                    UOC.bounds[refIndexC] = new Vector2Int(p_UF[i].tileY, UOC.bounds[refIndexC].y);
    //        //                }
    //        //                if (UOC.bounds[refIndexC].y == p_UF[i].tileY + 1)
    //        //                {
    //        //                    UOC.bounds[refIndexC] = new Vector2Int(UOC.bounds[refIndexC].x, p_UF[i].tileY);
    //        //                }
    //        //            }
    //        //            Debug.Log(UOC.bounds[refIndexC]);
    //        //            Debug.Log(UOC.units[i].tileX);
    //        //            Debug.Log(UOC.units[i].tileY);
    //        //            //if()
    //        //            UnitColumn.Add(UOC.units[i]);
    //        //        }
    //        //    }

    //        //    UOC.bounds.Add(new Vector2Int(0,0));
    //        //    UOL.bounds.Add(new Vector2Int(0,0));

    //        //    UOL.Y.Add(p_UF[i].tileY);
    //        //    UOC.X.Add(p_UF[i].tileX);

    //        //    currentIndexX = UOC.X.FindIndex(X => X == p_UF[i].tileX);
    //        //    currentIndexY = UOL.Y.FindIndex(Y => Y == p_UF[i].tileY);


    //        //    if (UOC.bounds[currentIndexX] == null)
    //        //    {
    //        //        UOC.bounds[currentIndexX] = new Vector2Int(p_UF[i].tileY, p_UF[i].tileY);
    //        //    }
    //        //    if (UOL.bounds[currentIndexY] == null)
    //        //    {
    //        //        UOL.bounds[currentIndexY] = new Vector2Int(p_UF[i].tileX, p_UF[i].tileX);
    //        //    }


    //        //    //Debug.Log("#######");
    //        //    //Debug.Log(p_UF[i].tileX);
    //        //    //Debug.Log(p_UF[i].tileY);
    //        //    Debug.Log(";;;;;;;;;;");
    //        //    Debug.Log(UOC.bounds[currentIndexX]);
    //        //    Debug.Log(UOL.bounds[currentIndexY]);
    //        //    Debug.Log("/////");


    //        //    /* check if a List exists for this Line, then, check if the value of the unit’s Tile position is too far off the bounds, if it is, then a new list can be added since it’s disconnected from the initial one */
    //        //    if (!UOL.Y.Contains(p_UF[i].tileY) && (p_UF[i].tileX < UOL.bounds[currentIndexY].x - 1 || p_UF[i].tileX > UOL.bounds[currentIndexY].y + 1)){
    //        //        Debug.Log("Is added Line");
    //        //        UOL.units.Add(p_UF[i]);
    //        //        UOL.bounds.Add(new Vector2Int(p_UF[i].tileX, p_UF[i].tileX));
    //        //    }
    //        //    else
    //        //    {
    //        //        if (UOL.bounds[currentIndexY].y > p_UF[i].tileX)
    //        //        {
    //        //            UOL.bounds[currentIndexY] = new Vector2Int(p_UF[i].tileY, UOL.bounds[currentIndexY].y);
    //        //        }
    //        //        else if (UOL.bounds[currentIndexY].x < p_UF[i].tileX)
    //        //        {
    //        //            UOL.bounds[currentIndexY] = new Vector2Int(UOL.bounds[currentIndexY].y, p_UF[i].tileY);
    //        //        }
    //        //    }
    //        //    Debug.Log(currentIndexX);
    //        //    /* check if a List exists for this Line, then, check if the value of the unit’s Tile position is too far off the bounds, if it is, then a new list can be added since it’s disconnected from the initial one */
    //        //    if (!UOC.X.Contains(p_UF[i].tileX) && (p_UF[i].tileY > UOC.bounds[currentIndexX].y + 1 || p_UF[i].tileY < UOC.bounds[currentIndexX].x - 1)) {
    //        //        Debug.Log("Is Added Column");
    //        //        UOC.units.Add(p_UF[i]);

    //        //        UOC.bounds.Add(new Vector2Int(p_UF[i].tileY, p_UF[i].tileY));
    //        //    }
    //        //    else
    //        //    {
    //        //        if (UOL.bounds[currentIndexX].y > p_UF[i].tileY)
    //        //        {
    //        //            UOL.bounds[currentIndexX] = new Vector2Int(p_UF[i].tileX, UOL.bounds[currentIndexX].y);
    //        //        }
    //        //        else if (UOL.bounds[currentIndexX].x < p_UF[i].tileY)
    //        //        {
    //        //            UOL.bounds[currentIndexX] = new Vector2Int(UOL.bounds[currentIndexX].x, p_UF[i].tileX);
    //        //        }
    //        //    }


    //        //}

    //        //for (int i = 0; i < UOL.Y.Count; i++)
    //        //{
    //        //    if (UOL.bounds[i].y - UOL.bounds[i].x < 3) continue; /* if the bounds have a difference of less than 3 then a link cannot be made */
    //        //    for (int j = UOL.bounds[i].x; j < UOL.bounds[i].y; j++)
    //        //    {
    //        //        unitToDefend.Add(gridList[UOL.bounds[i].x + j][UOL.Y[i]].unit);
    //        //    }
    //        //}
    //        //for (int i = 0; i < UOC.X.Count; i++)
    //        //{
    //        //    if (UOC.bounds[i].y - UOC.bounds[i].x < 3) continue; /* if the bounds have a difference of less than 3 then a link cannot be made */
    //        //    for (int j = UOC.bounds[i].x; j < UOC.bounds[i].y; j++)
    //        //    {
    //        //        unitToAttack.Add(gridList[UOC.X[i]][UOC.bounds[i].x + j].unit);
    //        //    }
    //    }
    //    Attack(UnitColumn);
    //    Defend(UnitLine);
    //} 
    public void Defend(List<List<Unit>> p_defendingUnit) { /* function for what should be done when units are defending */
        for (int i = 0; i < p_defendingUnit.Count; i++)
        {
            for (int j = 0; j < p_defendingUnit[i].Count; j++)
            {
                p_defendingUnit[i][j].state = 1;
                p_defendingUnit[i][j].spriteChange(defendImg);
                //if p_defendingUnit position = unitColumn
            }
        }
        UnitLine.Clear();
        p_defendingUnit.Clear();
    }
    public void Attack(List<List<Unit>> p_attackingUnit) { /* function for what should be done when units are attacking */
        for (int i = 0; i < p_attackingUnit.Count; i++)
        {
            for (int j = 0; j< p_attackingUnit[i].Count; j++)
            {
                Debug.Log(p_attackingUnit[i][j].state);
                p_attackingUnit[i][j].transform.localScale = new Vector3(0.6f, 0.6f, 1f);
                if (p_attackingUnit[i][j].state == 1)
                {
                    p_attackingUnit[i][j].transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                }
                p_attackingUnit[i][j].state = 2;
                Debug.Log(p_attackingUnit[i][j].state);
            }
        }
        UnitColumn.Clear();
        p_attackingUnit.Clear();
    } 

    



    public struct UnitOnLine{
        public List<Unit> units;
        public List<int> Y; // 3
        public List<Vector2Int> bounds; // (3,6) 
    }

    public struct UnitOnColumn{
        public List<Unit> units;
        public List<int> X;
        public List<Vector2Int> bounds;
    }
}