using System.Collections;
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
    [SerializeField]
    public S_GridManager _grid;

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
        _grid.unitList.Remove(this);
        _grid.totalUnitAmount -= 1;
        for (int i = 0; i < _grid.gridList[tileX].Count; i++)
        {
            if (_grid.gridList[tileX][i].unit!=null)
            {
                _grid.gridList[tileX][i].unit.MoveToTile(_grid.gridList[tileX][0]);
            }
        }
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
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
           
            _posToMove=new Vector3(transform.position.x, -((_grid.startY + _grid.height * actualTile.transform.localScale.y)+transform.position.y),-1);
            if (!_isMoving)
            {
                _isMoving = true;
                StartCoroutine(LerpMove());
            }
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

    public void TakeDamage() 
    {
        
    }
    public void AttackAnotherUnit() 
    { 

    }

    /*Move the unit to the top of the row of unit corresponding at the tile clicked if possible
    then deselect the unit*/
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
                    S_GameManager.Instance.ReduceActionPointBy1();
                }
                actualTile.unit = null;
                actualTile = tile;
                actualTile.unit = this;
                _grid.unitSelected = null;
                tileX = tile.tileX;
                tileY = tile.tileY;
                _posToMove = tile.transform.position;
                if (!_isMoving)
                {
                    _isMoving = true;
                    StartCoroutine(LerpMove());
                }
                foreach (Unit unit in _grid.unitList)
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
                _grid.unitSelected = null;
                tileX = tile.tileX;
                tileY = tile.tileY;
                _posToMove = tile.transform.position;
                if (!_isMoving)
                {
                    _isMoving = true;
                    StartCoroutine(LerpMove());
                }
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
        if (S_GameManager.Instance.isPlayer1Turn)
        {
            S_GameManager.Instance.UnitCallOnOff(1, false);
        }
        else
        {
            S_GameManager.Instance.UnitCallOnOff(2, false);
        }
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
    }

    //Align the Unit with the collumn overed by the mouse to previsualize where you're aiming
    public void VisualizePosition(S_Tile p_tile)
    {
        _posToMove = new Vector3(p_tile.transform.position.x, _grid.startY + _grid.height*actualTile.transform.localScale.y);
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
        if(_grid.unitSelected==null)
        SelectUnit();
    }

    public bool GetIsMoving()
    {
        return _isMoving;
    }

    public void ReturnToBaseTile()
    {
        MoveToTile(actualTile);
    }
}
