using System.Collections;
using System.Collections.Generic;
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
    public int formationIndex=0;
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
        unitColor = SO_Unit.unitColor;
        //gameObject.GetComponent<SpriteRenderer>().sprite = SO_Unit.unitSprite[randomNumber];
        //sizeX = SO_Unit.sizeX;
        //sizeY = SO_Unit.sizeY;
        //attack = SO_Unit.attack;
        //defense = SO_Unit.defense;
        //turnCharge = SO_Unit.unitTurnCharge;
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
        tileX= p_tile.tileX;
        tileY= p_tile.tileY;
        unitManager = p_tile.grid.unitManager;
        enemyGrid = grid.enemyGrid;
        grid.AllUnitPerColumn[tileX].Add(this);
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

    public void AttackPlayer()
    {
        ReducePlayerHp();
        actualFormation[0].DestroyFormation();
    }
    // Used to check where the attacking formation needs to go if an adversary unit is found and deals damage to them.
    //public void AttackAnotherUnit()
    //{
    //    //the lead of the formation is the one who checks where to go next and kills the formation if his own life fall to/below 0.
    //    if (actualFormation[0] == this)
    //    {
    //        for (int i = 0; i < enemyGrid.gridList[tileX].Count; i++)
    //        {
    //            if (enemyGrid.gridList[tileX][i].unit)
    //            {
    //                if (enemyGrid.gridList[tileX][i].unit.actualFormation == null)
    //                {
    //                    attack -= enemyGrid.gridList[tileX][i].unit.defense;
    //                    enemyGrid.gridList[tileX][i].unit.TakeDamage(attack + enemyGrid.gridList[tileX][i].unit.defense);

    //                }
    //                else
    //                {
    //                    attack -= enemyGrid.gridList[tileX][i].unit.attack;
    //                    enemyGrid.gridList[tileX][i].unit.TakeDamage(attack + enemyGrid.gridList[tileX][i].unit.attack);
    //                }
    //                if (attack <= 0)
    //                {
    //                    unitManager.UnitColumn.Remove(actualFormation);
    //                    StartCoroutine(DestroyFormation());
    //                    grid.AllUnitPerColumn = grid.UnitPriorityCheck();
    //                    return;
    //                }
    //                for (int j = i; j < enemyGrid.gridList[tileX].Count; j++)
    //                {
    //                    if (enemyGrid.gridList[tileX][j].unit)
    //                    {
    //                        _posToMove = enemyGrid.gridList[tileX][j].transform.position;
    //                        StartCoroutine(LerpMove());
    //                        break;
    //                    }
    //                }
    //                return;
    //            }
    //        }
    //        //at this point, there are no units left on the column of the attacker so it attacks the player instead
    //        _posToMove = new Vector3(transform.position.x, -((grid.startY + grid.height * transform.localScale.y) + transform.position.y), -1);
    //        grid.AllUnitPerColumn = grid.UnitPriorityCheck();
    //        grid.enemyGrid.AllUnitPerColumn = grid.UnitPriorityCheck();
    //        ReducePlayerHp();
    //        StartCoroutine(DestroyFormation());
    //        return;
    //    }

    //    //followers of the lead just moves.
    //    else
    //    {
    //        _posToMove = actualFormation[0]._posToMove - new Vector3(0, actualFormation[0].transform.position.y - transform.position.y, -1);
    //        StartCoroutine(LerpMove());
    //    }
    //    return;
    //}

    /* is called by the UnitManager, can be used to define what happens for a unit if units are kill by the enemy attack*/
    public void ReducePlayerHp(){ // needs rework

        AudioManager.instance.PlayOneShot(FMODEvents.instance.Impact, this.transform.position);

        if (S_GameManager.Instance.isPlayer1Turn)
        {
            S_GameManager.Instance.player2CharacterHealth.currentHP -=  attack;
        }
        else
        {
            S_GameManager.Instance.player1CharacterHealth.currentHP -= attack;
        }
    }

    /* is called by the unit that killed it, can be used to check if units are kill by the enemy attack*/
    public void TakeDamage(int p_damage)
    {
        if (actualFormation != null)
        {
            attack -= p_damage;
            if (attack <= 0)
            {
                for (int j = 0; j < actualFormation.Count; j++)
                {
                    if (actualFormation[j] != this)
                    {
                        actualFormation[j].actualTile.unit = null;
                        grid.unitList.Remove(actualFormation[j]);
                        grid.AllUnitPerColumn[actualFormation[j].tileX].Remove(actualFormation[j]);
                        Destroy(actualFormation[j].gameObject);
                    }
                }
                actualTile.unit = null;
                grid.unitList.Remove(this);
                grid.AllUnitPerColumn[tileX].Remove(this);
                unitManager.UnitColumn.Remove(actualFormation);
                Destroy(gameObject);
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
        foreach (S_Tile tile in p_tile.grid.gridList[p_tile.tileX])
        {
            if (tile.unit == null || tile.unit==this)
            {
                if (S_GameManager.Instance.isPlayer1Turn)
                {
                    S_GameManager.Instance.UnitCallOnOff(1, true);
                }
                else
                {
                    S_GameManager.Instance.UnitCallOnOff(2, true);
                }
                if (tileX != tile.tileX)
                {
                    _willLoseActionPoints = true;
                }
                actualTile.unit = null;
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
                if (_willLoseActionPoints)
                {
                    S_GameManager.Instance.ReduceActionPointBy1();
                    _willLoseActionPoints = false;
                }
                break;
            }
        }
        unitManager.UnitCombo(3);
    }
    /*Move the unit to the top of the row of unit corresponding at the tile clicked if possible
  then deselect the unit*/
    public void MoveToTile(S_Tile p_tile)
    {
        // We iterate throw the column of the given tile
        foreach (S_Tile tile in p_tile.grid.gridList[p_tile.tileX])
        {
            if (tile.unit == null || tile.unit == this)
            {
                actualTile.unit = null;
                actualTile = tile;
                actualTile.unit = this;
                grid.unitSelected = null;
                tileX = tile.tileX;
                tileY = tile.tileY;
                _posToMove = tile.transform.position;

                StartCoroutine(LerpMove());
                foreach (Unit unit in grid.unitList)
                {
                    unit.GetComponent<BoxCollider2D>().enabled = true;
                }
                break;
            }
        }
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

    //get the last unit of the row corresponding to the tile clicked
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
        if (actualTile.tileY + 1 > grid.gridList[actualTile.tileX].Count-1)
        {
            if(actualTile.tileY== grid.gridList[actualTile.tileX].Count - 1)
            {
                grid.unitSelected = this;
                foreach (Unit unit in grid.unitList)
                {
                    unit.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
            return;
        }
        if (grid.gridList[actualTile.tileX][actualTile.tileY + 1].unit != null)
        {
            grid.gridList[actualTile.tileX][actualTile.tileY + 1].unit.SelectUnit();
        }
        else
        {
            grid.unitSelected = this;
            foreach (Unit unit in grid.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    //Align the Unit with the collumn overed by the mouse to previsualize where you're aiming
    public void VisualizePosition(S_Tile p_tile)
    {
        _posToMove = new Vector3(p_tile.transform.position.x, grid.startY + grid.height * actualTile.transform.localScale.y);
        if (!_isMoving)
        {
            _isMoving = true;
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
                if (unit.actualFormation == null)
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
        if(grid.unitSelected==null && state==0 && S_GameManager.Instance.currentTurn != S_GameManager.TurnEmun.TransitionTurn)
        {
            SelectUnit();
        }
    }
}
