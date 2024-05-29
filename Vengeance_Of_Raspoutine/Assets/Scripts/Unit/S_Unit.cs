using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    [Header("Movements :")]
    public int speed = 10;
    private bool _isMoving = false;
    private Vector3 _posToMove;

    [Header("References :")]
    public SO_Unit SO_Unit; //Unit.SO_Unit.
    public int attack;
    public int defense;
    public int state = 0;
    public int turnCharge;
    public Sprite unitImg;
    public S_Tile actualTile;
    public GameObject highlight;
    public S_GridManager grid;
    private S_GridManager enemyGrid;
    private S_UnitManager unitManager;


    public int tileX;
    public int tileY;
    // Start is called before the first frame update

    private void Start()
    {
        attack = SO_Unit.attack;
        defense = SO_Unit.defense;
        turnCharge = SO_Unit.unitTurnCharge;
        speed = 10;

        
    }
    private IEnumerator LerpMove()
    {
        while(Vector3.Distance(transform.position, new Vector3(_posToMove.x, _posToMove.y, -1)) >= 0.5)
        
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(_posToMove.x, _posToMove.y, -1),Time.deltaTime*5);
            yield return new WaitForEndOfFrame();
        }
        _isMoving = false;
        transform.position = new Vector3(_posToMove.x, _posToMove.y, -1);
        yield return null;       
    }
    private IEnumerator DestroyUnit()
    {
        actualTile.unit = null;
        grid.unitList.Remove(this);
        grid.totalUnitAmount -= 1;
        for (int i = 0; i < grid.gridList[tileX].Count; i++)
        {
            if (grid.gridList[tileX][i].unit!=null)
            {
                grid.gridList[tileX][i].unit.MoveToTile(grid.gridList[tileX][0]);
            }
        }
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
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
        enemyGrid = p_tile.grid.enemyGrid;
        unitManager = p_tile.grid.unitManager;
    }

    public void spriteChange(Sprite img)
    {
        transform.GetComponent<SpriteRenderer>().sprite = img;
    }

    

    public void AttackCharge()
    {
        if(state == 2) turnCharge--;

        if( turnCharge == 0)
        {
            for (int j = 0; j < unitManager.UnitColumn.Count; j++)
            {
                if (unitManager.UnitColumn[j].Contains(this))
                {

                    StartCoroutine(AttackAnotherUnit(unitManager.UnitColumn[j]));
                    break;
                }   
            }
            _posToMove=new Vector3(transform.position.x, -((grid.startY + grid.height * actualTile.transform.localScale.y)+transform.position.y),-1);
            if (!_isMoving)
            {
                _isMoving = true;
                StartCoroutine(LerpMove());
            }
            ReducePlayerHp();
            StartCoroutine(DestroyUnit());
        }
    }

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

    public void TakeDamage(int p_damage) 
    {
        for (int i = 0; i < unitManager.UnitColumn.Count; i++)
        {
            
            if (unitManager.UnitColumn[i].Contains(this))
            {
                attack-= p_damage;
                if (attack <= 0)
                {
                    for(int j = 0; j < unitManager.UnitColumn[i].Count; j++)
                    {
                        DestroyUnit();
                    }
                }
                break;
            }
            else
            {
                defense -= p_damage;
                if (attack <= 0)
                {
                    DestroyUnit();
                }
            }
        }
    }
    public IEnumerator AttackAnotherUnit(List<Unit> p_formation) 
    {
        if (!_isMoving)
        {
            _isMoving = true;
            StartCoroutine(LerpMove());
        }
        if (p_formation[0]==this)
        {
            for (int i = 0; i < enemyGrid.gridList[tileX].Count; i++)
            {
                if (enemyGrid.gridList[tileX][i].unit)
                {

                    attack -= enemyGrid.gridList[tileX][i].unit.defense;
                    enemyGrid.gridList[tileX][i].unit.TakeDamage(attack + enemyGrid.gridList[tileX][i].unit.defense);
                    if (attack <= 0)
                    {
                        foreach (Unit u in p_formation)
                        {
                            u.actualTile.unit = null;
                            u.grid.unitList.Remove(u);
                            u.grid.totalUnitAmount -= 1;
                            Destroy(u);
                        }
                    }
                    _posToMove = new Vector3(enemyGrid.gridList[tileX][i].unit.transform.position.x, enemyGrid.gridList[tileX][i].unit.transform.position.y, -1);
                    for (int j = 1; j < p_formation.Count; j++)
                    {
                        p_formation[j].AttackAnotherUnit(p_formation);
                    }
                }
                yield return new WaitForSeconds(1);
            }
        }
        else
        {

            _posToMove = 
                new Vector3(
                p_formation[p_formation.FindIndex(a => a == this) - 1].transform.position.x - transform.localScale.x, 
                p_formation[p_formation.FindIndex(a => a == this) - 1].transform.position.y - transform.localScale.y,
                -1);
        }
        
        yield break;
        
    }

    /*Move the unit to the top of the row of unit corresponding at the tile clicked if possible
    then deselect the unit*/
    public void ActionMoveToTile(S_Tile p_tile)
    {
        foreach (S_Tile tile in p_tile.grid.gridList[p_tile.tileX])
        {
            if (tile.unit == null || tile.unit==this)
            {
                if (tileX != tile.tileX)
                {
                    S_GameManager.Instance.ReduceActionPointBy1();
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
                break;
            }
        }
        
    }

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

    //get the last unit of the row corresponding to the tile clicked
    public void SelectUnit()
    {
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
        _posToMove = new Vector3(p_tile.transform.position.x, grid.startY + grid.height*actualTile.transform.localScale.y);
        if (!_isMoving)
        {
            _isMoving = true;
            StartCoroutine(LerpMove());
        }
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
        if(grid.unitSelected==null)
        SelectUnit();
    }
}
