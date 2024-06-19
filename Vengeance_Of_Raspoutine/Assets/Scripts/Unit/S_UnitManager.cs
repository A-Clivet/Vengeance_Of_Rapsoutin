using System.Collections.Generic;
using UnityEngine;

public class S_UnitManager : MonoBehaviour
{
    [Header("Stats :")]
    [Tooltip("The number of Adrenaline had to the player who create a new formation (Attack & Defense), the number is multiplied by the number of unit there are in the formation")]
    public int numberOfAdrenalineToHadForEachUnitInFormation = 1;

    public S_GridManager grid;
    public List<List<Unit>> UnitLine = new();
    public List<List<Unit>> UnitColumn = new();
    public Sprite defendImg;

    // - Private variables - //
    // References
    S_GameManager _gameManager;
    S_CharacterAdrenaline _player1CharacterAdrenaline;
    S_CharacterAdrenaline _player2CharacterAdrenaline;

    private List<List<S_Tile>> gridList;

    private void Awake()
    {
        gridList = grid.gridList;

        // Setting up private variables
        _gameManager = S_GameManager.Instance;
    }

    private void Start()
    {
        // Setting up private variables
        _player1CharacterAdrenaline = _gameManager.player1CharacterAdrenaline;
        _player2CharacterAdrenaline = _gameManager.player2CharacterAdrenaline;
    }

    public void UnitCombo(int p_formationNumber, bool p_isIAUsingThisFunction = false)
    {
        int columnCounter = 0;
        int lineCounter = 0;

        int currentColorColumn = -1;
        int currentColorLine = -1;

        //SO_Unit actualType = null;

        //for pour la grille, tu check si une untié à ça sizeY > 1 , if ( sur la sizeX  == 1 || 2 )

        for (int i = 0; i < grid.width; i++)//check list largeur
        {
            for (int j = 0; j < grid.AllUnitPerColumn[i].Count; j++)
            {

                if (grid.AllUnitPerColumn[i][j].state == 0)
                {
                    int columnCounter = 0;
                    int lineCounter = 0;
                    if (grid.AllUnitPerColumn[i][j].sizeY < 2)
                    {
                        //check unit in column
                        for(int k = 0; k < p_formationNumber; k++)
                        {
                            if (gridList[i][grid.AllUnitPerColumn[i][j].tileY].tileY + k >= grid.height || j + k >= grid.AllUnitPerColumn[i].Count)
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
                        if(columnCounter == p_formationNumber)
                        {
                            UnitColumn.Add(new());

                            for (int k = 0; k < p_formationNumber; k++)
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

                    // We had Adrenaline to the player who created this formation
                    if (!p_isIAUsingThisFunction)
                    {
                        AddAdrenalineToThePlayerWhoForm(numberOfAdrenalineToHadForEachUnitInFormation * p_formationNumber);
                    }

                    gridList[j][i].unit.state = 1;
                    gridList[j - 1][i].unit.state = 1;
                    gridList[j - 2][i].unit.state = 1;

                    UnitLine[UnitLine.Count - 1].Add(gridList[j - 2][i].unit);
                    UnitLine[UnitLine.Count - 1].Add(gridList[j - 1][i].unit);
                    UnitLine[UnitLine.Count - 1].Add(gridList[j][i].unit);
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

                    // We had Adrenaline to the player who created this formation
                    if (!p_isIAUsingThisFunction)
                    {
                        AddAdrenalineToThePlayerWhoForm(numberOfAdrenalineToHadForEachUnitInFormation * p_formationNumber);
                    }

                    gridList[i][j].unit.state = 2;
                    gridList[i][j - 1].unit.state = 2;
                    gridList[i][j - 2].unit.state = 2;
                    
                    //temporary visual change to notices attacking units

                    gridList[i][j].unit.gameObject.transform.localScale = new Vector3(0.6f,0.6f,1f);
                    gridList[i][j - 1].unit.gameObject.transform.localScale = new Vector3(0.6f,0.6f,1f);
                    gridList[i][j - 2].unit.gameObject.transform.localScale = new Vector3(0.6f,0.6f,1f);
                    
                    UnitColumn[UnitColumn.Count - 1].Add(gridList[i][j - 2].unit);
                    UnitColumn[UnitColumn.Count - 1].Add(gridList[i][j - 1].unit);
                    UnitColumn[UnitColumn.Count - 1].Add(gridList[i][j].unit);
                    for (int k=0;k< UnitColumn[UnitColumn.Count-1].Count;k++)
                    {
                        UnitColumn[UnitColumn.Count - 1][k].actualFormation = UnitColumn[UnitColumn.Count - 1];
                        UnitColumn[UnitColumn.Count - 1][k].formationIndex = k;
                    }
                    grid.AllUnitPerColumn = grid.UnitPriorityCheck();

                    if (S_GameManager.Instance.isPlayer1Turn)
                    {
                        S_GameManager.Instance.player1CharacterXP.GainXP(5);
                    }
                    else
                    {
                        //Debug.Log("Unit Pos in UnitPerColumn (" + grid.AllUnitPerColumn[i][j].tileX + "," + grid.AllUnitPerColumn[i][j].tileY + ")");
                        if (grid.AllUnitPerColumn[i][j].tileY + p_formationNumber >= grid.height)
                        {
                            break;
                        }
                        if(grid.AllUnitPerColumn[i][j].sizeX == 1)
                        {
                            for (int k = 1; k < p_formationNumber; k++)
                            {
                                if (( j + k >= grid.AllUnitPerColumn[i].Count) || grid.gridList[i][grid.AllUnitPerColumn[i][j].tileY].tileY + k >= Mathf.Abs(grid.height)  || (grid.AllUnitPerColumn[i][j + k] == null))
                                {
                                    continue;
                                }
                                if (grid.AllUnitPerColumn[i][j + k].state == 0 && grid.AllUnitPerColumn[i][j].unitColor == grid.AllUnitPerColumn[i][j + k].unitColor && grid.AllUnitPerColumn[i][j + k].SO_Unit.sizeY < 2)
                                {
                                    columnCounter++;
                                }
                                else
                                {
                                    columnCounter = 0;
                                    break;
                                }
                            }
                            if (columnCounter == p_formationNumber - 1)
                            {
                                gridList[i][j].unit.state = 2;
                                for (int k = 1; k < p_formationNumber; k++)
                                {
                                    
                                    gridList[i][j + k].grid.unitList.Remove(gridList[i][j + k].unit);
                                    gridList[i][j + k].grid.AllUnitPerColumn[gridList[i][j + k].tileX].Remove(gridList[i][j + k].unit);
                                    gridList[i][j + k].unit = null;
                                    foreach (Unit unit in grid.AllUnitPerColumn[i])
                                    {
                                        unit.MoveToTile(gridList[i][j + k]);
                                    }
                                    Destroy(gridList[i][j + k].gameObject);
                                }
                            }
                        }
                        else
                        {
                            for (int k = 1; k < p_formationNumber; k++)
                            {
                                if ( i + 1 >= grid.width || grid.gridList[i][grid.AllUnitPerColumn[i][j].tileY].tileY + k >= grid.height || grid.gridList[i + 1][grid.AllUnitPerColumn[i][j].tileY].tileY + k >= grid.height || j + k >= grid.AllUnitPerColumn.Count ||
                                    grid.gridList[i][j].unit == null || grid.gridList[i][j + k].unit == null || grid.gridList[i + 1][j + k].unit == null)
                                {
                                    continue;
                                }
                                if (grid.gridList[i][grid.AllUnitPerColumn[i][j].tileY + k].unit.state == 0 && grid.gridList[i][grid.AllUnitPerColumn[i][j].tileY].unit.unitColor == grid.gridList[i][grid.AllUnitPerColumn[i][j].tileY + k].unit.unitColor && grid.gridList[i][grid.AllUnitPerColumn[i][j].tileY + k].unit.SO_Unit.sizeY < 2 &&
                                    grid.gridList[i + 1][grid.AllUnitPerColumn[i][j].tileY + k].unit.state == 0 && grid.gridList[i][grid.AllUnitPerColumn[i][j].tileY].unit.unitColor == grid.gridList[i + 1][grid.AllUnitPerColumn[i][j].tileY + k].unit.unitColor && grid.gridList[i + 1][grid.AllUnitPerColumn[i][j].tileY + k].unit.SO_Unit.sizeY < 2
                                    )
                                {
                                    columnCounter++;
                                }
                                else
                                {
                                    columnCounter = 0;
                                    break;
                                }
                            }
                            if (columnCounter == p_formationNumber - 1)
                            {
                                gridList[i][j].unit.state = 2;
                                for (int k = 1; k < p_formationNumber; k++)
                                {
                                    gridList[i][j + k].grid.unitList.Remove(gridList[i][j + k].unit);
                                    gridList[i][j + k].grid.AllUnitPerColumn[gridList[i][j + k].tileX].Remove(gridList[i][j + k].unit);
                                    gridList[i][j + k].unit = null;
                                    gridList[i + 1][j + k].grid.unitList.Remove(gridList[i][j + k].unit);
                                    gridList[i + 1][j + k].grid.AllUnitPerColumn[gridList[i][j + k].tileX].Remove(gridList[i][j + k].unit);
                                    gridList[i + 1][j + k].unit = null;
                                    foreach (Unit unit in grid.AllUnitPerColumn[i])
                                    {
                                        unit.MoveToTile(gridList[i][j + k]);
                                    }
                                    gridList[i + 1][j + k].unit = null;
                                    foreach (Unit unit in grid.AllUnitPerColumn[i + 1])
                                    {
                                        unit.MoveToTile(gridList[i][j + k]);
                                    }
                                    Destroy(gridList[i][j + k].gameObject);
                                    Destroy(gridList[i + 1][j + k].gameObject);
                                }
                            }
                        }
                    }
                }
            }
        }
        grid.UnitPriorityCheck();
    }

    //public void UnitCombo(int p_formationNumber)
    //{

    //    int columnCounter = 0;
    //    int lineCounter = 0;

    //    int currentColorColumn = -1;
    //    int currentColorLine = -1;

    //    //SO_Unit actualType = null;

    //    //for pour la grille, tu check si une untié à ça sizeY > 1 , if ( sur la sizeX  == 1 || 2 )

    //    for (int i = 0; i < grid.width; i++)//check list largeur
    //    {
    //        for (int j = 0; j < Mathf.Abs(grid.height); j++)//check list hauteur
    //        {
    //            if (gridList[i][j].unit == null) //si la case est vide, continue
    //            {
    //                continue;
    //            }
    //            else // si la case contient une unité
    //            {
    //                if (gridList[i][j].unit.sizeY > 1)// si l'unité prends plus d'une case de hauteur ce n'est pas un mur
    //                {
    //                    if (gridList[i][j].unit.sizeX == 1 && gridList[i][j].unit.isChecked == false) //check si l'unite prends une ou deux case de large, ici l'unité en prends qu'une ( case )
    //                    {
    //                        gridList[i][j].unit.isChecked = true; // met le boolean à true pour dire que l'unité à été check et ne pas repasser dessus

    //                        if (j + 3 < grid.height && (gridList[i][j + 2].unit != null && gridList[i][j + 3].unit != null)) //Comparaison des prochaines case de la grille pour éviter le Out of Index
    //                        {
    //                            if (gridList[i][j].unit.unitColor == gridList[i][j + 2].unit.unitColor && gridList[i][j + 3].unit.unitColor == gridList[i][j].unit.unitColor)
    //                            {
    //                                UnitColumn.Add(new());

    //                                gridList[i][j].unit.state = 2;
    //                                gridList[i][j + 2].unit.DestroyFormation(); // function to remove
    //                                gridList[i][j + 3].unit.DestroyFormation(); // function to remove

    //                                grid.UnitPriorityCheck();
    //                            }
    //                        }
    //                    }
    //                    else // size x = 2
    //                    {
    //                        gridList[i][j].unit.isChecked = true;

    //                        if (j + 3 < grid.height && (gridList[i][j + 2].unit != null && gridList[i][j + 3].unit != null) && (gridList[i + 1][j + 2].unit != null && gridList[i + 1][j + 3].unit != null)) //Comparaison des prochaines case de la grille pour éviter le Out of Index
    //                        {
    //                            if (gridList[i][j].unit.unitColor == gridList[i][j + 2].unit.unitColor && gridList[i][j + 3].unit.unitColor == gridList[i][j].unit.unitColor && gridList[i + 1][j].unit.unitColor == gridList[i][j + 2].unit.unitColor && gridList[i + 1][j + 3].unit.unitColor == gridList[i][j].unit.unitColor)
    //                            {
    //                                UnitColumn.Add(new());

    //                                gridList[i][j].unit.state = 2;
    //                                gridList[i][j + 2].unit.DestroyFormation();
    //                                gridList[i][j + 3].unit.DestroyFormation();
    //                                gridList[i + 1][j + 2].unit.DestroyFormation();
    //                                gridList[i + 1][j + 3].unit.DestroyFormation();

    //                                grid.UnitPriorityCheck();
    //                            }
    //                        }
    //                    }
    //                    return;
    //                }
    //            }
    //        }
    //    }


    //    for (int i = 0; i < Mathf.Abs(grid.height); i++) // hateur
    //    {
    //        for (int j = 0; j < grid.width; j++) // largeur
    //        {
    //            if (gridList[j][i].unit == null)
    //            {
    //                currentColorLine = -1; // -1 is not a value that a unitColor will be 
    //                lineCounter = 0;
    //                continue;
    //            }
    //            if (gridList[j][i].unit.state != 0)
    //            {
    //                currentColorLine = -1;
    //                lineCounter = 0;
    //                continue;
    //            }
    //            if (gridList[j][i].unit.unitColor != currentColorLine)
    //            {
    //                currentColorLine = gridList[j][i].unit.unitColor;
    //                lineCounter = 1;
    //                continue;
    //            }
    //            else
    //            {
    //                lineCounter++;
    //            }

    //            if (lineCounter == p_formationNumber)
    //            {
    //                UnitLine.Add(new());
    //                gridList[j][i].unit.state = 1;
    //                gridList[j - 1][i].unit.state = 1;
    //                gridList[j - 2][i].unit.state = 1;

    //                UnitLine[UnitLine.Count - 1].Add(gridList[j - 2][i].unit);
    //                UnitLine[UnitLine.Count - 1].Add(gridList[j - 1][i].unit);
    //                UnitLine[UnitLine.Count - 1].Add(gridList[j][i].unit);
    //                grid.AllUnitPerColumn = grid.UnitPriorityCheck();
    //                currentColorLine = -1;
    //                lineCounter = 0;
    //            }
    //            if (UnitLine.Count >= 1)
    //            {
    //                Defend(UnitLine);
    //            }
    //            UnitLine.Clear();
    //        }
    //        currentColorLine = -1;
    //        lineCounter = 0;
    //    }

    //    for (int i = 0; i < grid.width; i++) // largeur
    //    {
    //        for (int j = 0; j < Mathf.Abs(grid.height); j++) // hauteur
    //        {

    //            if (gridList[i][j].unit == null)
    //            {
    //                currentColorColumn = -1; // -1 is not a value that a unitColor will be 
    //                columnCounter = 0;
    //                continue;
    //            }
    //            if (gridList[i][j].unit.state != 0)
    //            {
    //                currentColorColumn = -1;
    //                columnCounter = 0;
    //                continue;
    //            }
    //            if (gridList[i][j].unit.unitColor != currentColorColumn)// add gridList[i][j].unt.unitType check
    //            {
    //                currentColorColumn = gridList[i][j].unit.unitColor;
    //                columnCounter = 1;
    //                continue;
    //            }
    //            else
    //            {
    //                columnCounter++;
    //            }

    //            if (columnCounter == p_formationNumber) // mode attack 
    //            {
    //                UnitColumn.Add(new());

    //                gridList[i][j].unit.state = 2;
    //                gridList[i][j - 1].unit.state = 2;
    //                gridList[i][j - 2].unit.state = 2;

    //                //temporary visual change to notices attacking units

    //                gridList[i][j].unit.gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
    //                gridList[i][j - 1].unit.gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
    //                gridList[i][j - 2].unit.gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 1f);

    //                UnitColumn[UnitColumn.Count - 1].Add(gridList[i][j - 2].unit);
    //                UnitColumn[UnitColumn.Count - 1].Add(gridList[i][j - 1].unit);
    //                UnitColumn[UnitColumn.Count - 1].Add(gridList[i][j].unit);
    //                for (int k = 0; k < UnitColumn[UnitColumn.Count - 1].Count; k++)
    //                {
    //                    UnitColumn[UnitColumn.Count - 1][k].actualFormation = UnitColumn[UnitColumn.Count - 1];
    //                    UnitColumn[UnitColumn.Count - 1][k].formationIndex = k;
    //                }
    //                grid.AllUnitPerColumn = grid.UnitPriorityCheck();
    //                columnCounter = 0;
    //                currentColorColumn = -1;
    //            }
    //        }
    //        currentColorColumn = -1;
    //        columnCounter = 0;
    //    }

    //}


    public void Defend(List<List<Unit>> p_defendingUnit)
    { /* function for what should be done when units are defending */
        for (int i = 0; i < p_defendingUnit.Count; i++)
        {
            for (int j = 0; j < p_defendingUnit[i].Count; j++)
            {
                p_defendingUnit[i][j].spriteChange(defendImg);

                p_defendingUnit[i][j].defense = 4;
                p_defendingUnit[i][j].attack = 0;

                //if p_defendingUnit position = unitColumn
            }
        }
        //If we are here then it significate that we've created a wall combo. Then we check if we removed a unit before to avoid removing a action point
        if (S_RemoveUnit.Instance.removing)
        {
            S_GameManager.Instance.IncreaseActionPointBy1();
            S_RemoveUnit.Instance.removing = false;
        }
    }

    public void AttackBuff(GameObject GOunit)
    {
        GOunit.GetComponent<Unit>().attack += 1;
    }

    public void DefenseBuff(GameObject GOunit)
    {
        GOunit.GetComponent<Unit>().defense += 1;
    }

    void AddAdrenalineToThePlayerWhoForm(int p_adrenalineToHad)
    {
        // Security
        if (_player1CharacterAdrenaline == null || _player2CharacterAdrenaline == null)
        {
            _player1CharacterAdrenaline = _gameManager.player1CharacterAdrenaline;
            _player2CharacterAdrenaline = _gameManager.player2CharacterAdrenaline;
        }

        // Adding adrenaline for the player who played
        if (_gameManager.currentTurn == S_GameManager.TurnEmun.Player1Turn)
            _player1CharacterAdrenaline.currentAdrenaline += p_adrenalineToHad;

        else if (_gameManager.currentTurn == S_GameManager.TurnEmun.Player2Turn)
            _player2CharacterAdrenaline.currentAdrenaline += p_adrenalineToHad;
    }
}