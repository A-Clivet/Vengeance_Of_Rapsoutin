using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    [Header("Movements :")]
    public int speed;
    private bool _isMoving = false;
    private Vector3 _posToMove;

    [Header("References :")]
    public SO_Unit SO_Unit; //Unit.SO_Unit.
    public int attack;
    public int defense;
    public int state = 0;
    public int turnCharge;
    public List<Unit> actualFormation = null;
    public int formationIndex = 0;
    //UnitDisplay
    public int unitColor;
    public Sprite unitSprite;
    public S_Tile actualTile;
    public GameObject highlight;
    public S_GridManager grid;
    public GameObject statsCanvas;
    private S_GridManager enemyGrid;
    private S_UnitManager unitManager;

    public int tileX;
    public int tileY;
    public int sizeX;
    public int sizeY;
    public bool isChecked;
    public bool mustAttack = false;
    private bool _willLoseActionPoints = false;


    private void Awake()
    {

        int randomNumber = Random.Range(0, 1);
        gameObject.GetComponent<SpriteRenderer>().sprite = SO_Unit.unitSprite[randomNumber];
        sizeX = SO_Unit.sizeX;
        sizeY = SO_Unit.sizeY;
        attack = SO_Unit.attack;
        defense = SO_Unit.defense;
        turnCharge = SO_Unit.unitTurnCharge;
        if(sizeY == 2)
        {
            unitColor = Random.Range(0,3);
        }
        else
        {
            unitColor = SO_Unit.unitColor;
        }
        speed = 10;
    }

    // destroy the formation in good order to avoid removing itself before the others

    /*This function is the lerp from one position to another
    it also handles the recursive parts of the attack of a formation alternating between moving to the position and
    calling attackAnotherUnit() to see the next position to go.*/
    private IEnumerator LerpMove()
    {
        float t = 0;
        if (!_isMoving)
        {
            _isMoving = true;
        }

        while (Vector3.Distance(transform.position, new Vector3(_posToMove.x, _posToMove.y, -1)) >= 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(_posToMove.x, _posToMove.y, -1), t);
            yield return new WaitForEndOfFrame();
            if (mustAttack)
            {
                t = t + Time.deltaTime / 100;
            }
            else
            {
                t = t + Time.deltaTime / 5;
            }

        }
        _isMoving = false;
        transform.position = new Vector3(_posToMove.x, _posToMove.y, -1);
        if (mustAttack)
        {
            AttackPlayer();
        }
    }

    //IS ABSOLUTELY NEEDED TO BE CALLED WHEN A UNIT IS INSTANTIATED
    //AND THE REFERENCE TO HIS OWN TILE IS KNOWN
    public void OnSpawn(S_Tile p_tile)
    {
        grid = p_tile.grid;
        grid.unitList.Add(this);
        actualTile = p_tile;
        p_tile.unit = this;
        tileX = p_tile.tileX;
        tileY = p_tile.tileY;
        unitManager = p_tile.grid.unitManager;
        enemyGrid = grid.enemyGrid;
        for (int i = 0; i < sizeX; i++) 
        {
            grid.AllUnitPerColumn[tileX+ i].Add(this);
        }
        
    }

    public void DestroyFormation()
    {
        unitManager.UnitColumn.Remove(actualFormation);
        foreach (Unit u in actualFormation)
        {
            if (u != this)
            {
                grid.AllUnitPerColumn[u.tileX].Remove(u);
                Destroy(u.gameObject);
                u.StopAllCoroutines();

            }
        }
        grid.AllUnitPerColumn[tileX].Remove(this);
        for (int i = 0; i < unitManager.UnitColumn.Count; i++)
        {
            if (unitManager.UnitColumn[i][0].turnCharge <= 0)
            {
                Destroy(gameObject);
                return;
            }
        }
        for (int i = 0; i < grid.enemyGrid.unitManager.UnitColumn.Count; i++)
        {
            if (grid.enemyGrid.unitManager.UnitColumn[i][0].turnCharge <= 0)
            {
                Destroy(gameObject);
                return;
            }
        }
        S_GameManager.Instance.EndTurn();
        Destroy(gameObject);
        

    }

    private void OnDestroy()
    {
        grid.totalUnitAmount -= 1;
    }
    //launch the attack of all formation and begin the recursion of the attack

    public void AttackCharge()
    {
        if (state == 2) turnCharge--;

        if (turnCharge <= 0)
        {
            if (actualFormation.Count > 1)
            {

                //remove virtually the units from their own grid and tile, they do not exists anymore for their grid and respective tiles.
                for (int i = 0; i < actualFormation.Count; i++)
                {

                    actualFormation[i].mustAttack = true;
                    actualFormation[i].turnCharge = 0;
                    actualFormation[i].actualTile.unit = null;
                    grid.unitList.Remove(actualFormation[i]);
                    actualFormation[i]._posToMove = new Vector3(transform.position.x, -(grid.startY + grid.height * actualTile.transform.localScale.y) + transform.localScale.y * i, -1);
                    actualFormation[i].StartCoroutine(LerpMove());
                }
            }
        }
    }

    public void AttackPlayer()
    {
        ReducePlayerHp();
        actualFormation[0].DestroyFormation();
    }

    public void ReducePlayerHp(){ // needs rework

        AudioManager.instance.PlayOneShot(FMODEvents.instance.Impact, this.transform.position);

        if (S_GameManager.Instance.isPlayer1Turn)
        {
            S_GameManager.Instance.player2CharacterHealth.currentHP -= attack;
        }
        else
        {
            S_GameManager.Instance.player1CharacterHealth.currentHP -= attack;
        }
    }

    /* is called by the unit that killed it, can be used to check if units are kill by the enemy attack*/
    public void TakeDamage(int p_damage)
    {
        if (actualFormation.Count > 0)
        {
            attack -= p_damage;

            if (attack <= 0)
            {
                if (sizeY == 1)
                {
                    for (int j = 0; j < actualFormation.Count; j++)
                    {
                        if (actualFormation[j] != this)
                        {
                            actualFormation[j].actualTile.unit = null;
                            grid.AllUnitPerColumn[actualFormation[j].tileX].Remove(actualFormation[j]);
                            Destroy(actualFormation[j].gameObject);
                        }
                        actualFormation[j].actualTile.unit = null;
                        grid.unitList.Remove(actualFormation[j]);
                        grid.AllUnitPerColumn[actualFormation[j].tileX].Remove(actualFormation[j]);
                        grid.unitList.Remove(this);
                        grid.AllUnitPerColumn[tileX].Remove(this);
                        Destroy(gameObject);
                    }
                }
            }
            return;
        }

        defense -= p_damage;

        if (defense <= 0)
        {
            actualTile.unit = null;
            grid.unitList.Remove(this);
            grid.totalUnitAmount -= 1;

            if (S_GameManager.Instance.isPlayer1Turn)
            {
                S_GameManager.Instance.player1CharacterXP.GainXP(5);
            }

            else
            {
                S_GameManager.Instance.player2CharacterXP.GainXP(5);
            }

            grid.AllUnitPerColumn[tileX].Remove(this);
            Destroy(gameObject);
        }
    }

    // Same as MoveToTile but use a action point
    public void ActionMoveToTile(S_Tile p_tile)
    {
        if ((p_tile.tileX == 7 && sizeX == 2) || (grid.gridList[p_tile.tileX][4] != null && sizeY == 2))
        {
            p_tile.tileX--;
        }
        if (S_GameManager.Instance.isPlayer1Turn)
        {
            S_GameManager.Instance.UnitCallOnOff(1, true);
        }
        else if (!S_GameManager.Instance.isPlayer1Turn)
        {
            S_GameManager.Instance.UnitCallOnOff(2, true);
        }
        if (tileX != p_tile.tileX)
        {
            _willLoseActionPoints = true;
        }

        int lineToGoTo = 0;

        if (sizeX == 1)
        {
            for (int i = (Mathf.Abs(grid.height) - sizeY); i > -1; i--) // start from the top of the column 
            {
                if (p_tile.grid.gridList[p_tile.tileX][i].unit == null || p_tile.grid.gridList[p_tile.tileX][i].unit == this) // if it doesn't hit any unit 
                {
                    lineToGoTo = i;
                }
                else
                {
                    break;
                }
            }


            switch ((sizeX, sizeY))
            {
                case (1, 1):

                    actualTile.unit = null;

                    actualTile = p_tile.grid.gridList[p_tile.tileX][lineToGoTo];

                    p_tile.grid.gridList[actualTile.tileX][actualTile.tileY].unit = this;

                    break;


                case (1, 2):


                    p_tile.grid.gridList[actualTile.tileX][actualTile.tileY + 1].unit = null;
                    actualTile.unit = null;

                    actualTile = p_tile.grid.gridList[p_tile.tileX][lineToGoTo];

                    p_tile.grid.gridList[actualTile.tileX][actualTile.tileY].unit = this;
                    p_tile.grid.gridList[actualTile.tileX][actualTile.tileY + 1].unit = this;

                    break;

                default:

                    break;
            }

            grid.unitSelected = null;
            tileX = actualTile.tileX;
            tileY = actualTile.tileY;

            _posToMove = actualTile.transform.position;

            StartCoroutine(LerpMove());

            foreach (Unit unit in grid.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        else // if unit is sizeX ==  2
        {
            int x = p_tile.tileX;
            if (x == grid.width - 1)
            {
                x--;
            }

            for (int i = (Mathf.Abs(grid.height) - sizeY); i > -1; i--) // check columns from the end to get the first time it would hit a unit 
            {
                if ((p_tile.grid.gridList[x][i].unit == null || p_tile.grid.gridList[x][i].unit == this) && (p_tile.grid.gridList[x + 1][i].unit == null || p_tile.grid.gridList[x + 1][i].unit == this))
                {
                    lineToGoTo = i;
                }
                else
                {
                    break;
                }
            }

            //clears tiles 
            p_tile.grid.gridList[actualTile.tileX + 1][actualTile.tileY].unit = null;
            p_tile.grid.gridList[actualTile.tileX][actualTile.tileY + 1].unit = null;
            p_tile.grid.gridList[actualTile.tileX + 1][actualTile.tileY + 1].unit = null;
            actualTile.unit = null;

            actualTile = grid.gridList[x][lineToGoTo];

            //set unit to tiles 
            actualTile.unit = this;
            p_tile.grid.gridList[actualTile.tileX + 1][actualTile.tileY].unit = this;
            p_tile.grid.gridList[actualTile.tileX][actualTile.tileY + 1].unit = this;
            p_tile.grid.gridList[actualTile.tileX + 1][actualTile.tileY + 1].unit = this;

            grid.unitSelected = null;
            tileX = actualTile.tileX;
            tileY = actualTile.tileY;
            _posToMove = actualTile.transform.position;

            StartCoroutine(LerpMove());

            foreach (Unit unit in grid.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        if (_willLoseActionPoints)
        {
            S_GameManager.Instance.ReduceActionPointBy1();
            _willLoseActionPoints = false;
        }
    }
    /*Move the unit to the top of the row of unit corresponding at the tile clicked if possible
    then deselect the unit*/
    public void MoveToTile(S_Tile p_tile)
    {
        int lineToGoTo = 0;

        if (sizeX == 1)
        {

            switch ((sizeX, sizeY))
            {
                case (1, 1):

                    for (int i = (Mathf.Abs(grid.height) - sizeY); i > -1; i--) // start from the top of the column 
                    {
                        if (grid.gridList[p_tile.tileX][i].unit == null || grid.gridList[p_tile.tileX][i].unit == this) // if it doesn't hit any unit 
                        {
                            lineToGoTo = i;
                        }
                        else
                        {
                            break;
                        }
                    }


                    actualTile.unit = null;

                    actualTile = grid.gridList[p_tile.tileX][lineToGoTo];

                    actualTile.unit = this;

                    break;


                case (1, 2):

                    for (int i = (Mathf.Abs(grid.height) - sizeY); i > -1; i--) // start from the top of the column 
                    {
                        if (grid.gridList[p_tile.tileX][i].unit == null || grid.gridList[p_tile.tileX][i].unit == this) // if it doesn't hit any unit 
                        {
                            lineToGoTo = i;
                        }
                        else
                        {
                            break;
                        }
                    }

                    grid.gridList[actualTile.tileX][actualTile.tileY + 1].unit = null;
                    actualTile.unit = null;
                    
                    actualTile = grid.gridList[p_tile.tileX][lineToGoTo];

                    actualTile.unit = this;
                    grid.gridList[actualTile.tileX][actualTile.tileY + 1].unit = this;

                    break;

                default:

                    break;
            }

            grid.unitSelected = null;
            tileX = actualTile.tileX;
            tileY = actualTile.tileY;

            _posToMove = actualTile.transform.position;

            StartCoroutine(LerpMove());

            foreach (Unit unit in grid.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        else // if unit is sizeX ==  2
        {
            int x = p_tile.tileX;
            if (x == grid.width - 1)
            {
                x--;
            }

            for (int i = (Mathf.Abs(grid.height) - sizeY); i > -1; i--) // check columns from the end to get the first time it would hit a unit 
            {
                if ((grid.gridList[x][i].unit == null || grid.gridList[x][i].unit == this) && (grid.gridList[x + 1][i].unit == null || p_tile.grid.gridList[x + 1][i].unit == this))
                {
                    lineToGoTo = i;
                }
                else
                {
                    break;
                }
            }

            //clears tiles 
            grid.gridList[actualTile.tileX + 1][actualTile.tileY].unit = null;
            grid.gridList[actualTile.tileX][actualTile.tileY + 1].unit = null;
            grid.gridList[actualTile.tileX + 1][actualTile.tileY + 1].unit = null;
            actualTile.unit = null;

            actualTile = grid.gridList[x][lineToGoTo];

            //set unit to tiles 
            actualTile.unit = this;
            grid.gridList[actualTile.tileX + 1][actualTile.tileY].unit = this;
            grid.gridList[actualTile.tileX][actualTile.tileY + 1].unit = this;
            grid.gridList[actualTile.tileX + 1][actualTile.tileY + 1].unit = this;

            grid.unitSelected = null;
            tileX = actualTile.tileX;
            tileY = actualTile.tileY;
            _posToMove = actualTile.transform.position;

            StartCoroutine(LerpMove());

            foreach (Unit unit in grid.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    public void ForceOnTile(S_Tile p_tile) // force a unit on input tile 
    {
        if (p_tile.unit != null)
        {
            if (p_tile.tileY + 1 < grid.height)
            {
                ForceOnTile(grid.gridList[p_tile.tileX][p_tile.tileY + 1]);
            }
            else
            {
                Debug.Log("Well... Fuck");
                return;

            }
        }

        switch ((sizeX, sizeY))
        {
            case (1, 1):

                actualTile.unit = null;

                actualTile = p_tile;

                actualTile.unit = this;

                break;


            case (1, 2):

                if (grid.gridList[p_tile.tileX][p_tile.tileY + 1] == null)
                {

                    grid.gridList[actualTile.tileX][actualTile.tileY + 1].unit = null;
                    actualTile.unit = null;

                    actualTile = p_tile;

                    actualTile.unit = this;
                    grid.gridList[actualTile.tileX][actualTile.tileY + 1].unit = this;
                }
                else
                {
                    if (p_tile.tileY + 2 < grid.height)
                    {
                        ForceOnTile(grid.gridList[p_tile.tileX][p_tile.tileY + 2]);
                    }
                    else
                    {
                        Debug.Log("Well... Fuck");
                        return;

                    }
                }
                break;

            case (2, 2):

                int x = p_tile.tileX;
                if (x == grid.width - 1)
                {
                    p_tile = grid.gridList[p_tile.tileX - 1][p_tile.tileY];
                }

                if (grid.gridList[p_tile.tileX][p_tile.tileY + 1] == null && grid.gridList[p_tile.tileX + 1][p_tile.tileY] == null && grid.gridList[p_tile.tileX + 1][p_tile.tileY + 1] == null)
                {
                    //clears tiles 
                    grid.gridList[actualTile.tileX + 1][actualTile.tileY].unit = null;
                    grid.gridList[actualTile.tileX][actualTile.tileY + 1].unit = null;
                    grid.gridList[actualTile.tileX + 1][actualTile.tileY + 1].unit = null;
                    actualTile.unit = null;

                    actualTile = p_tile;

                    //set unit to tiles 
                    actualTile.unit = this;
                    grid.gridList[actualTile.tileX + 1][actualTile.tileY].unit = this;
                    grid.gridList[actualTile.tileX][actualTile.tileY + 1].unit = this;
                    grid.gridList[actualTile.tileX + 1][actualTile.tileY + 1].unit = this;

                }
                else
                {
                    if (p_tile.tileY + 2 < grid.height)
                    {
                        ForceOnTile(grid.gridList[p_tile.tileX][p_tile.tileY + 2]);
                    }
                    else
                    {
                        Debug.Log("Well... Fuck");
                        return;

                    }
                }
                break;

            default:

                break;
        }
        tileX = p_tile.tileX;
        tileY = p_tile.tileY;
        _posToMove = p_tile.transform.position;
        StartCoroutine(LerpMove());

    }

    //Used to reorganize the Units by state
    public void SwitchUnit(S_Tile p_tile)
    {
        foreach (S_Tile tile in p_tile.grid.gridList[p_tile.tileX])
        {
            if (tile.unit == null)
            {
                actualTile = tile;
                actualTile.unit = this;
                grid.unitSelected = null;
                tileX = tile.tileX;
                tileY = tile.tileY;
                _posToMove = tile.transform.position;

                if (!_isMoving)
                {
                    _isMoving = true;
                    StartCoroutine(LerpMove());
                }
                foreach (Unit unit in grid.unitList)
                {
                    unit.GetComponent<BoxCollider2D>().enabled = true;
                }
                break;
            }
        }
    }

    //get the last unit of the column corresponding to the tile clicked
    public void SelectUnit()
    {
        if (S_GameManager.Instance.isPlayer1Turn)
        {
            S_GameManager.Instance.UnitCallOnOff(1, false);
        }
        else
        {
            S_GameManager.Instance.UnitCallOnOff(2, false);
        }
        if (!grid.isSwapping )
        {
            if (actualTile.tileY >= grid.gridList[actualTile.tileX].Count - 1)
            {
                grid.unitSelected = this;

                foreach (Unit unit in grid.unitList)
                {
                    unit.GetComponent<BoxCollider2D>().enabled = false;
                }

            }
            switch (sizeX, sizeY)
            {
                case (1, 1):

                        grid.gridList[actualTile.tileX][actualTile.tileY + sizeY].unit.SelectUnit();

                    break;

                case (1, 2):

                        grid.gridList[actualTile.tileX][actualTile.tileY + sizeY].unit.SelectUnit();

                    break;

                case(2,2):

                        grid.gridList[actualTile.tileX][actualTile.tileY + sizeY].unit.SelectUnit();

                    break;

                default:

                    grid.unitSelected = this;
                    foreach (Unit unit in grid.unitList)
                    {
                        unit.GetComponent<BoxCollider2D>().enabled = false;
                    }

                    break;
            }
        }
        else 
        {
            if (grid.unitSelected == null)
            {
                switch (sizeX)
                {
                    default:

                        return;

                    case 1:

                        if (grid.gridList[tileX][tileY + 1].unit != null)
                            grid.unitSelected = this;

                        break;

                    case 2:

                        if (grid.gridList[tileX][tileY + 2].unit != null && grid.gridList[tileX + 1][tileY + 2].unit != null)
                        {
                            grid.unitSelected = this;
                        }
                        break;

                }
            }
            else if (grid.unitSelected != this)
            {
                grid.SwapUnits(grid.unitSelected.actualTile, this.actualTile, grid.unitSelected, this);
                _posToMove = actualTile.transform.position;
                grid.unitSelected._posToMove = grid.unitSelected.actualTile.transform.position;
                StartCoroutine(LerpMove());
                StartCoroutine(grid.unitSelected.LerpMove());
                S_UnitCallButtonHandler.Instance.HandleUnitCallButtonInteraction(S_GameManager.Instance.isPlayer1Turn, true);
                grid.unitSelected = null;
                S_GameManager.Instance.ReduceSwapCounter(S_GameManager.Instance.isPlayer1Turn);
                if (S_GameManager.Instance.isPlayer1Turn)
                {
                    if (S_GameManager.Instance.swapCounterP1 == 0)
                    {
                        S_SwapButtonsHandler.Instance.player1SwapButton.interactable = false;
                    }
                }
                else
                {
                    if (S_GameManager.Instance.swapCounterP2 == 0)
                    {
                        S_SwapButtonsHandler.Instance.player2SwapButton.interactable = false;
                    }
                }

                S_GameManager.Instance.ReduceActionPointBy1();
            }
        }
    }


    //Align the Unit with the collumn overed by the mouse to previsualize where you're aiming
    public void VisualizePosition(S_Tile p_tile)
    {
        _posToMove = new Vector3(p_tile.transform.position.x, grid.startY + grid.height * actualTile.transform.localScale.y);
        if (!grid.isSwapping)
        {
            StartCoroutine(LerpMove());
        }
    }
    public bool GetIsMoving()
    {
        return _isMoving;
    }

    public void ReturnToBaseTile()
    {
        MoveToTile(actualTile);
    }

    //change sprite of the unit to the wall png
    public void spriteChange(Sprite img)
    {
        transform.GetComponent<SpriteRenderer>().sprite = img;
    }

    public void ResetBuffs()
    {
        attack = SO_Unit.attack;
        defense = SO_Unit.defense;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();

        if (unit.grid != grid)
        {
            if (!unit.mustAttack)
            {
                if (unit.actualFormation.Count <= 0)
                {
                    attack -= unit.defense;
                    unit.TakeDamage(attack + unit.defense);

                }
                else if (unit.turnCharge > 0)
                {
                    attack -= unit.attack;
                    unit.TakeDamage(attack + unit.attack);

                }
                if(attack <= 0)
                {
                    actualFormation[0].DestroyFormation();
                }
            }

        }
    }
    private void OnMouseOver()
    {
        if (S_GameManager.Instance.currentTurn != S_GameManager.TurnEmun.TransitionTurn)
        {
            highlight.SetActive(true);
            S_RemoveUnit.Instance.hoveringUnit = this;
        }
    }
    private void OnMouseExit()
    {
        highlight.SetActive(false);
        S_RemoveUnit.Instance.hoveringUnit = null;
    }
    private void OnMouseDown()
    {
        if (!grid.isSwapping)
        {
            if (grid.unitSelected == null && state == 0 && S_GameManager.Instance.currentTurn != S_GameManager.TurnEmun.TransitionTurn)
            {
                SelectUnit();
            }
        }
        else
        {
            if (state == 0 && S_GameManager.Instance.currentTurn != S_GameManager.TurnEmun.TransitionTurn)
            {
                SelectUnit();
            }
        }
    }
}