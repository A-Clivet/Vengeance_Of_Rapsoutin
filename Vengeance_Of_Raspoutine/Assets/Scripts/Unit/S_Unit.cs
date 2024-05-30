using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    [Header("Movements :")]
    public int speed;
    public bool _isFollowing = false;
    private bool _isMoving = false;
    private Vector3 _posToMove;

    [Header("References :")]
    public SO_Unit SO_Unit; //Unit.SO_Unit.
    public int attack;
    public int defense;
    public int state = 0;
    public int turnCharge;
    //UnitDisplay
    public int unitColor;
    public Sprite unitSprite;
    public S_Tile actualTile;
    public GameObject highlight;
    public S_GridManager grid;
    private S_GridManager enemyGrid;
    private S_UnitManager unitManager;

    public int tileX;
    public int tileY;
    public int sizeX;
    public int sizeY;
    public bool isChecked;
    private bool _willLoseActionPoints = false;
    // Start is called before the first frame update

    private void Start()
    {
        int randomNumber = Random.Range(0, 3);
        unitColor = randomNumber;
        unitSprite = SO_Unit.unitSprite[randomNumber];
        sizeX = SO_Unit.sizeX;
        sizeY = SO_Unit.sizeY;
        attack = SO_Unit.attack;
        defense = SO_Unit.defense;
        turnCharge = SO_Unit.unitTurnCharge;
        speed = 10;
    } //Remove the Unit and replace the other units in the column
    private IEnumerator DestroyUnit()
    {
        for (int i = 0; i < grid.gridList[tileX].Count; i++)
        {
            if (grid.gridList[tileX][i].unit != null)
            {
                grid.gridList[tileX][i].unit.MoveToTile(grid.gridList[tileX][0]);
            }
        }
        yield return new WaitForSeconds(2);
        _isFollowing = false;
        Destroy(gameObject);
    }

    //used to get the location of the upper member of the formation to follow it correctly
    private IEnumerator followFormation(List<Unit> p_formation)
    {
        _isFollowing = true;
        while (true)
        {
            _posToMove =
                new Vector3(
                p_formation[p_formation.FindIndex(a => a == this) - 1].transform.position.x,
                p_formation[p_formation.FindIndex(a => a == this) - 1].transform.position.y + transform.localScale.y,
                -1);
            if (!_isMoving)
            {
                _isMoving = true;
                StartCoroutine(LerpMove());
            }
            yield return new WaitForEndOfFrame();
        }

    }
    //This function is the lerp from one position to another
    private IEnumerator LerpMove()
    {
        float t = 0;
        while (Vector3.Distance(transform.position, new Vector3(_posToMove.x, _posToMove.y, -1)) >= 0.1)

        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(_posToMove.x, _posToMove.y, -1), t);
            yield return new WaitForEndOfFrame();
            t = t + Time.deltaTime / 5;
        }
        _isMoving = false;
        transform.position = new Vector3(_posToMove.x, _posToMove.y, -1);
        yield return null;
    }

    public IEnumerator AttackAnotherUnit(List<Unit> p_formation)
    {

        if (p_formation[0] == this)
        {
            for (int i = 0; i < enemyGrid.gridList[tileX].Count; i++)
            {
                if (enemyGrid.gridList[tileX][i].unit)
                {
                    _posToMove = new Vector3(enemyGrid.gridList[tileX][i].unit.transform.position.x, enemyGrid.gridList[tileX][i].unit.transform.position.y, -1);
                    if (!_isMoving)
                    {
                        _isMoving = true;
                        StartCoroutine(LerpMove());
                    }
                    yield return new WaitForSeconds(0.5f);
                    attack -= enemyGrid.gridList[tileX][i].unit.defense;
                    enemyGrid.gridList[tileX][i].unit.TakeDamage(attack + enemyGrid.gridList[tileX][i].unit.defense);
                    if (attack <= 0)
                    {
                        foreach (Unit u in p_formation)
                        {
                            Destroy(u.gameObject);
                            u._isFollowing = false;
                            for (int k = 0; k < enemyGrid.gridList[tileX].Count; k++)
                            {
                                if (enemyGrid.gridList[tileX][k].unit != null)
                                {
                                    enemyGrid.gridList[tileX][k].unit.MoveToTile(enemyGrid.gridList[tileX][0]);
                                }
                            }
                        }
                        yield break;
                    }

                }
                yield return new WaitForSeconds(1);
            }
            _posToMove = new Vector3(transform.position.x, -((grid.startY + grid.height * transform.localScale.y) + transform.position.y), -1);
            for (int i = 0; i < grid.gridList[tileX].Count; i++)
            {
                if (grid.gridList[tileX][i].unit != null)
                {
                    grid.gridList[tileX][i].unit.MoveToTile(grid.gridList[tileX][0]);
                }
            }
            for (int k = 0; k < enemyGrid.gridList[tileX].Count; k++)
            {
                if (enemyGrid.gridList[tileX][k].unit != null)
                {
                    enemyGrid.gridList[tileX][k].unit.MoveToTile(enemyGrid.gridList[tileX][0]);
                }
            }
            ReducePlayerHp();
            for (int j = 0; j < p_formation.Count; j++)
            {
                p_formation[j].StartCoroutine(DestroyUnit());
            }

        }
        else
        {
            if (!_isFollowing)
            {
                StartCoroutine(followFormation(p_formation));
            }

        }
        yield break;

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
        enemyGrid = p_tile.grid.enemyGrid;
        unitManager = p_tile.grid.unitManager;
    }

    public void AttackCharge()
    {
        if (state == 2) turnCharge--;

        if (turnCharge == 0)
        {
            for (int j = 0; j < unitManager.UnitColumn.Count; j++)
            {

                if (unitManager.UnitColumn[j].Contains(this))
                {
                    for (int i = 0; i < unitManager.UnitColumn[j].Count; i++)
                    {
                        unitManager.UnitColumn[j][i].turnCharge = 0;
                        unitManager.UnitColumn[j][i].actualTile.unit = null;
                        grid.unitList.Remove(unitManager.UnitColumn[j][i]);
                        grid.totalUnitAmount -= 1;


                    }
                    for (int k = 0; k < grid.gridList[tileX].Count; k++)
                    {
                        if (grid.gridList[tileX][k].unit != null)
                        {
                            grid.gridList[tileX][k].unit.MoveToTile(grid.gridList[tileX][0]);
                        }
                    }
                    for (int i = 0; i < unitManager.UnitColumn[j].Count; i++)
                    {
                        StartCoroutine(unitManager.UnitColumn[j][i].AttackAnotherUnit(unitManager.UnitColumn[j]));
                    }
                    if (unitManager.UnitColumn[j][0] == this)
                    {
                        unitManager.UnitColumn.Remove(unitManager.UnitColumn[j]);
                    }
                    break;
                }
            }
        }
    }

    /* is called by the UnitManager, can be used to define what happens for a unit if units are kill by the enemy attack*/
    public void ReducePlayerHp(){
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
        for (int i = 0; i < unitManager.UnitColumn.Count; i++)
        {

            if (unitManager.UnitColumn[i].Contains(this))
            {
                attack -= p_damage;
                if (attack <= 0)
                {
                    for (int j = 0; j < unitManager.UnitColumn[i].Count; j++)
                    {
                        if (unitManager.UnitColumn[i][j] != this)
                        {
                            unitManager.UnitColumn[i][j].actualTile.unit = null;
                            grid.unitList.Remove(unitManager.UnitColumn[i][j]);
                            grid.totalUnitAmount -= 1;
                            Destroy(unitManager.UnitColumn[i][j].gameObject);
                        }
                }
                    actualTile.unit = null;
                    grid.unitList.Remove(this);
                    grid.totalUnitAmount -= 1;

                    Destroy(gameObject);

                }
                return;
            }
        }
        defense -= p_damage;
        if (defense <= 0)
        {
            actualTile.unit = null;
            grid.unitList.Remove(this);
            grid.totalUnitAmount -= 1;
            Destroy(gameObject);
        }

        return;
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
    private void OnMouseOver()
    {
        highlight.SetActive(true);
        S_RemoveUnit.Instance.hoveringUnit = this;
    }
    private void OnMouseExit()
    {
        highlight.SetActive(false);
        S_RemoveUnit.Instance.hoveringUnit = null;
    }
    private void OnMouseDown()
    {
        if(grid.unitSelected==null && state != 1 && state != 2)
        SelectUnit();
    }
}
