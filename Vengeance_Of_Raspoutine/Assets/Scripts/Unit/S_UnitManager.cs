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
        int lineCounter;

        int currentColorColumn = -1;
        int currentColorLine;

        //SO_Unit actualType = null;

        for (int i = 0; i < Mathf.Abs(grid.height); i++) // hateur
        {
            lineCounter = 0;
            currentColorLine = -1;
            UnitLine.Clear();
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

                    for(int k = 0; k < p_formationNumber; k++)
                    {
                        gridList[j - k][i].unit.state = 1;
                        UnitLine[UnitLine.Count - 1].Add(gridList[j - k][i].unit);
                    }
                }    
                else if (lineCounter >= p_formationNumber)
                {
                    gridList[j][i].unit.state = 1;
                    UnitLine[UnitLine.Count - 1].Add(gridList[j][i].unit);
                }
            }
            if (UnitLine.Count >= 1)
            {
                Defend(UnitLine);
            }
        }

        for (int i = 0; i < grid.width; i++) // largeur
        {
            currentColorColumn = -1; // -1 is not a value that a unitColor will be 
            columnCounter = 0;


            for (int j = 0; j < Mathf.Abs(grid.height); j++) // hauteur
            {

                if (gridList[i][j].unit == null)
                {
                    currentColorColumn = -1;
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
                    if (S_GameManager.Instance.isPlayer1Turn)
                    {
                        S_GameManager.Instance.player1CharacterXP.GainXP(5);
                    }
                    else
                    {
                        S_GameManager.Instance.player2CharacterXP.GainXP(5);
                    }


                }
            }
        }
        grid.AllUnitPerColumn = grid.UnitPriorityCheck();
    }


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