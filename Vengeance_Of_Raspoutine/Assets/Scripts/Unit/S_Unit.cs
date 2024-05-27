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
    public int state;
    public int turnCharge;
    public Sprite unitImg;
    public S_Tile actualTile;
    public GameObject highlight;
    [SerializeField]
    public S_GridManager _grid;

    public int tileX;
    public int tileY;
    // Start is called before the first frame update

    private void Start()
    {
        attack = SO_Unit.attack;
        defense = SO_Unit.defense;
        state = SO_Unit.unitState;
        turnCharge = SO_Unit.unitTurnCharge;
        speed = 10;
    }

    private void Update()
    {
        if (_isMoving)
        {
            transform.position =Vector3.Lerp(transform.position, new Vector3(_posToMove.x, _posToMove.y, -1),Time.deltaTime*5);
            if (Vector3.Distance(transform.position, _posToMove) < 0.5)
            {
                transform.position=_posToMove;
                _isMoving = false;
            }
        }
    }

    public void spriteChange(Sprite img)
    {
        transform.GetComponent<SpriteRenderer>().sprite = img;
    }

    //IS ABSOLUTELY NEEDED TO BE CALLED WHEN A UNIT IS INSTANTIATED
    //AND THE REFERENCE TO HIS OWN TILE IS KNOWN
    public void OnSpawn(S_Tile p_tile)
    {
        _grid = p_tile.grid;
        _grid.unitList.Add(this);
        actualTile = p_tile;
        p_tile.unit = this;
        tileX= p_tile.tileX;
        tileY= p_tile.tileY;
    }

    public void AttackCharge()
    {
        if(state == 2) turnCharge--;

        if( turnCharge == 0)
        {
            OnAttack();
        }
    }

    /* is called by the UnitManager, can be used to define what happens for a unit if units are kill by the enemy attack*/
    public void OnAttack(){
        if (S_GameManager.Instance.isPlayer1Turn)
        {
            S_GameManager.Instance._player2CharacterHealth._currentHP -=  attack;
        }
        else
        {
            S_GameManager.Instance._player1CharacterHealth._currentHP -= attack;
        }
    }

    /* is called by the unit that killed it, can be used to check if units are kill by the enemy attack*/
    public void OnHit() {
        
    }

    /*Move the unit to the top of the row of unit corresponding at the tile clicked if possible
    then deselect the unit*/
    public void MoveToTileSwitch(S_Tile p_tile)
    {
        foreach (S_Tile tile in p_tile.grid.gridList[p_tile.tileX])
        {
            if (tile.unit == null || tile.unit==this)
            {
                actualTile.unit = null;
                actualTile = tile;
                actualTile.unit = this;
                _grid.unitSelected = null;
                tileX = tile.tileX;
                tileY = tile.tileY;
                _isMoving = true;
                _posToMove = tile.transform.position;
                foreach (Unit unit in _grid.unitList)
                {
                    unit.GetComponent<BoxCollider2D>().enabled = true;
                }
                break;
            }
        }
        S_GameManager.Instance.ReduceActionPointBy1();
    }

    public void MoveToTileAction(S_Tile p_tile)
    {
        foreach (S_Tile tile in p_tile.grid.gridList[p_tile.tileX])
        {
            if (tile.unit == null || tile.unit == this)
            {
                actualTile.unit = null;
                actualTile = tile;
                actualTile.unit = this;
                _grid.unitSelected = null;
                tileX = tile.tileX;
                tileY = tile.tileY;
                _isMoving = true;
                _posToMove = tile.transform.position;
                foreach (Unit unit in _grid.unitList)
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
        if (actualTile.tileY + 1 > _grid.gridList[actualTile.tileX].Count-1)
        {
            if(actualTile.tileY== _grid.gridList[actualTile.tileX].Count - 1)
            {
                _grid.unitSelected = this;
                foreach (Unit unit in _grid.unitList)
                {
                    unit.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
            return;
        }
        if (_grid.gridList[actualTile.tileX][actualTile.tileY + 1].unit != null)
        {
            _grid.gridList[actualTile.tileX][actualTile.tileY + 1].unit.SelectUnit();
        }
        else
        {
            _grid.unitSelected = this;
            foreach (Unit unit in _grid.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        return;
    }

    //Align the Unit with the collumn overed by the mouse to previsualize where you're aiming
    public void VisualizePosition(S_Tile p_tile)
    {
        _isMoving = true;
        _posToMove = new Vector3(p_tile.transform.position.x, _grid.startY + _grid.height*actualTile.transform.localScale.y);
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
        if(_grid.unitSelected==null)
        SelectUnit();
    }
    

}
