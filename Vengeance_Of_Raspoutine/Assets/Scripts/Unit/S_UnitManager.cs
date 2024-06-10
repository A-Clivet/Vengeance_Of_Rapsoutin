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


    public void Awake()
    {
        gridList = grid.gridList;
    }

    public void UnitCombo(int p_formationNumber)
    {

        int columnCounter = 0;
        int lineCounter = 0;

        int currentColorColumn = -1;
        int currentColorLine = -1;

        //SO_Unit actualType = null;

        //for pour la grille, tu check si une untié à ça sizeY > 1 , if ( sur la sizeX  == 1 || 2 )

        for (int i = 0; i < grid.width; i++)//check list largeur
        {
            for (int j = 0; j < Mathf.Abs(grid.height); j++)//check list hauteur
            {
                if (gridList[i][j].unit == null) //si la case est vide, continue
                {
                    continue;
                }
                else // si la case contient une unité
                {
                    if (gridList[i][j].unit.sizeY > 1)// si l'unité prends plus d'une case de hauteur ce n'est pas un mur
                    {
                        if (gridList[i][j].unit.sizeX == 1 && gridList[i][j].unit.isChecked == false) //check si l'unite prends une ou deux case de large, ici l'unité en prends qu'une ( case )
                        {
                            gridList[i][j].unit.isChecked = true; // met le boolean à true pour dire que l'unité à été check et ne pas repasser dessus

                            if (j + 3 < grid.height && (gridList[i][j + 2].unit != null && gridList[i][j + 3].unit != null)) //Comparaison des prochaines case de la grille pour éviter le Out of Index
                            {
                                if (gridList[i][j].unit.unitColor == gridList[i][j + 2].unit.unitColor && gridList[i][j + 3].unit.unitColor == gridList[i][j].unit.unitColor)
                                {
                                    if (columnCounter == p_formationNumber) // mode attack 
                                    {
                                        UnitColumn.Add(new());

                                        gridList[i][j].unit.state = 2;
                                        gridList[i][j + 2].unit.DestroyFormation();
                                        gridList[i][j + 3].unit.DestroyFormation();

                                        grid.UnitPriorityCheck();
                                        columnCounter = 0;
                                    }
                                }

                            }
                        }
                        else // size x = 2
                        {
                            gridList[i][j].unit.isChecked = true;

                            if (j + 3 < grid.height && (gridList[i][j + 2].unit != null && gridList[i][j + 3].unit != null)) //Comparaison des prochaines case de la grille pour éviter le Out of Index
                            {
                                if (gridList[i][j].unit.unitColor == gridList[i][j + 2].unit.unitColor && gridList[i][j + 3].unit.unitColor == gridList[i][j].unit.unitColor && gridList[i + 1][j].unit.unitColor == gridList[i][j + 2].unit.unitColor && gridList[i + 1][j + 3].unit.unitColor == gridList[i][j].unit.unitColor)
                                {
                                    if (columnCounter == p_formationNumber) // mode attack 
                                    {
                                        UnitColumn.Add(new());

                                        gridList[i][j].unit.state = 2;
                                        gridList[i][j + 2].unit.DestroyFormation();
                                        gridList[i][j + 3].unit.DestroyFormation();
                                        gridList[i + 1][j + 2].unit.DestroyFormation();
                                        gridList[i + 1][j + 3].unit.DestroyFormation();

                                        grid.UnitPriorityCheck();
                                        columnCounter = 0;
                                    }
                                }


                            }
                        }
                    }
                }
            }
        }


        for (int i = 0; i < Mathf.Abs(grid.height); i++) // hateur
        {
            for (int j = 0; j < grid.width; j++) // largeur
            {
                if(gridList[j][i].unit == null)
                {
                    currentColorLine = -1; // -1 is not a value that a unitColor will be 
                    lineCounter = 0;
                    continue;
                }
                if (gridList[j][i].unit.state != 0)
                {
                    currentColorLine = -1;
                    lineCounter = 0;
                    continue;
                }
                if(gridList[j][i].unit.unitColor != currentColorLine)
                {
                    currentColorLine = gridList[j][i].unit.unitColor;
                    lineCounter = 1;
                    continue;
                }
                else
                {
                    lineCounter++;
                }

                if (lineCounter == p_formationNumber)
                {
                    UnitLine.Add(new());
                    for(int h  = 0; h < p_formationNumber; h++)
                    {
                        gridList[j - h][i].unit.state = 1;
                    }

                    for(int h = p_formationNumber - 1; h >= 0; h--)
                    {
                        UnitLine[UnitLine.Count - 1].Add(gridList[j - h][i].unit);
                    }

                    grid.AllUnitPerColumn = grid.UnitPriorityCheck();
                    currentColorLine = -1;
                    lineCounter = 0;
                }
                if (UnitLine.Count >= 1)
                {
                    Defend(UnitLine);
                }
                UnitLine.Clear();
            }
            currentColorLine = -1;
            lineCounter = 0;
        }

        for (int i = 0; i < grid.width; i++) // largeur
        {
            for (int j = 0; j < Mathf.Abs(grid.height); j++) // hauteur
            {

                if (gridList[i][j].unit == null)
                {
                    currentColorColumn = -1; // -1 is not a value that a unitColor will be 
                    columnCounter = 0;
                    continue;
                }
                if (gridList[i][j].unit.state != 0)
                {
                    currentColorColumn = -1; 
                    columnCounter = 0;
                    continue;
                }
                if (gridList[i][j].unit.unitColor != currentColorColumn)// add gridList[i][j].unt.unitType check
                {
                    currentColorColumn = gridList[i][j].unit.unitColor;
                    columnCounter = 1;
                    continue;
                }
                else
                {
                    columnCounter++;
                }

                if (columnCounter == p_formationNumber) // mode attack 
                {
                    UnitColumn.Add(new());

                    for (int h = 0; h < p_formationNumber; h++) 
                    {
                        gridList[i][j - h].unit.state = 2;
                        gridList[i][j - h].unit.gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 1f);  //temporary visual change to notices attacking units
                    }
                    
                    for (int h = p_formationNumber - 1; h >= 0; h--)
                    {
                        UnitColumn[UnitColumn.Count - 1].Add(gridList[i][j - h].unit);
                    }

                    for (int k=0;k< UnitColumn[UnitColumn.Count-1].Count;k++)
                    {
                        UnitColumn[UnitColumn.Count - 1][k].actualFormation = UnitColumn[UnitColumn.Count - 1];
                        UnitColumn[UnitColumn.Count - 1][k].formationIndex = k;
                    }
                    grid.AllUnitPerColumn = grid.UnitPriorityCheck();
                    columnCounter = 0;
                    currentColorColumn = -1;
                }
            }
            currentColorColumn = -1;
            columnCounter = 0;
        }
        
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

    public void AttackBuff(Unit unit)
    {
        unit.attack += 5;
    }

    public void DefenseBuff(Unit unit)
    {
        unit.defense += 5;
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