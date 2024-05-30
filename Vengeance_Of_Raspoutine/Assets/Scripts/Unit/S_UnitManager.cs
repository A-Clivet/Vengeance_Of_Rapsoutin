using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class S_UnitManager : MonoBehaviour
{
    public S_GridManager grid;
    private List<List<S_Tile>> gridList;
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

        Debug.Log(grid.name);
        //for pour la grille, tu check si une untié à ça sizeY > 1 , if ( sur la sizeX  == 1 || 2 )

        for (int i = 0; i < grid.width; i++)//largeur
        {
            for (int j = 0; j < Mathf.Abs(grid.height); j++)//hauteur
            {
                if (gridList[i][j].unit == null)
                {
                    continue;
                }
                else
                {
                    if (gridList[i][j].unit.sizeY > 1)
                    {
                        if (gridList[i][j].unit.sizeX == 1 && gridList[i][j].unit.isChecked == false)
                        {
                            //l'unite est une petite
                            gridList[i][j].unit.isChecked = true;
                            if (j + 3 < grid.height && (gridList[i][j + 2].unit != null && gridList[i][j + 3].unit != null))
                            {
                                // j+3 case la plus éloignée possible, doit être dans l'index
                                if (gridList[i][j].unit.SO_Unit.unitColor == gridList[i][j + 2].unit.SO_Unit.unitColor && gridList[i][j].unit.SO_Unit.unitColor == gridList[i][j + 3].unit.SO_Unit.unitColor) gridList[i][j].unit.state = 2;
                            }
                        }
                        else
                        {
                            //l'unité est une élite
                        }
                    }
                }
            }
        }


        for (int i = 0; i < Mathf.Abs(grid.height); i++) // hateur
        {
            for (int j = 0; j < grid.width; j++) // largeur
            {
                if (gridList[j][i].unit == null || (gridList[j][i].unit != null && gridList[j][i].unit.state != 0))
                {
                    lineCounter = 0;
                    continue;
                }
                //if (gridList[j][i].unit.SO_Unit != null) (gridList[i][j].unit != null && gridList[i][j].unit.state != 0)
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
                    gridList[j][i].unit.state = 1;
                    gridList[j - 1][i].unit.state = 1;
                    gridList[j - 2][i].unit.state = 1;

                    Debug.Log("Unité en position : (" + j + "," + i + ")  is in state : " + gridList[j][i].unit.state);
                    Debug.Log("Unité en position : (" + (j - 1) + "," + i + ")  is in state : " + gridList[j - 1][i].unit.state);
                    Debug.Log("Unité en position : (" + (j - 2) + "," + i + ")  is in state : " + gridList[j - 2][i].unit.state);

                    UnitLine[UnitLine.Count - 1].Add(gridList[j - 2][i].unit);
                    UnitLine[UnitLine.Count - 1].Add(gridList[j - 1][i].unit);
                    UnitLine[UnitLine.Count - 1].Add(gridList[j][i].unit);
                    grid.UnitPriorityCheck();

                    lineCounter = 0;
                }
                if (UnitLine.Count >= 1)
                {
                    Defend(UnitLine);
                }
                UnitLine.Clear();
            }
            lineCounter = 0;
        }
        for (int i = 0; i < grid.width; i++) // largeur
        {
            for (int j = 0; j < Mathf.Abs(grid.height); j++) // hauteur
            {
                if (gridList[i][j].unit == null || (gridList[i][j].unit != null && gridList[i][j].unit.state != 0))
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

                if (columnCounter == p_formationNumber)
                {
                    UnitColumn.Add(new());

                    gridList[i][j].unit.state = 2;
                    gridList[i][j - 1].unit.state = 2;
                    gridList[i][j - 2].unit.state = 2;

                    Debug.Log("Unité en position : (" + i + "," + j + ")  is in state : " + gridList[i][j].unit.state);
                    Debug.Log("Unité en position : (" + i + "," + (j - 1) + ")  is in state : " + gridList[i][j - 1].unit.state);
                    Debug.Log("Unité en position : (" + i + "," + (j - 2) + ")  is in state : " + gridList[i][j - 2].unit.state);

                    UnitColumn[UnitColumn.Count - 1].Add(gridList[i][j - 2].unit);
                    UnitColumn[UnitColumn.Count - 1].Add(gridList[i][j - 1].unit);
                    UnitColumn[UnitColumn.Count - 1].Add(gridList[i][j].unit);
                    grid.UnitPriorityCheck();
                    columnCounter = 0;
                }
            }
            columnCounter = 0;
        }
        //if(UnitColumn.Count >= 1)
        //{
        //    Attack(UnitColumn);
        //}
    }


    public void Defend(List<List<Unit>> p_defendingUnit)
    { /* function for what should be done when units are defending */
        for (int i = 0; i < p_defendingUnit.Count; i++)
        {
            for (int j = 0; j < p_defendingUnit[i].Count; j++)
            {
                p_defendingUnit[i][j].spriteChange(defendImg);
                //if p_defendingUnit position = unitColumn
            }
        }
    }

    public void AttackBuff()
    {
        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < Mathf.Abs(grid.height); j++)
            {
                if (gridList[i][j].unit != null)
                {
                    gridList[i][j].unit.attack += 5;
                }
            }
        }
    }

    public void DefenseBuff()
    {
        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < Mathf.Abs(grid.height); j++)
            {
                if (gridList[i][j].unit != null)
                {
                    gridList[i][j].unit.defense += 5;
                }
            }
        }
    }

    //public struct UnitOnLine{
    //    public List<Unit> units;
    //    public List<int> Y; // 3
    //    public List<Vector2Int> bounds; // (3,6) 
    //}

    //public struct UnitOnColumn{
    //    public List<Unit> units;
    //    public List<int> X;
    //    public List<Vector2Int> bounds;
    //}
}