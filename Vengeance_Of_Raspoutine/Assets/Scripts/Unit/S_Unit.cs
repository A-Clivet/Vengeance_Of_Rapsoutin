using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Movements :")]
    public int speed = 10;

    [Header("References :")]
    public SO_Unit SO_Unit;
    public S_Tile actualTile;
    public GameObject highlight;
    [SerializeField]
    private S_GridManager _grid;

    public int tileX;
    public int tileY;
    // Start is called before the first frame update

    private void Start()
    {
        speed = 10;
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

    /* is called by the UnitManager, can be used to define what happens for a unit if units are kill by the enemy attack*/
    public void OnAttack(){
        switch (SO_Unit.unitType)
        {
            case 0:
                break;

            case 1:
                break;

            case 2:
                break;
        }
    }

    /* is called by the unit that killed it, can be used to check if units are kill by the enemy attack*/
    public void OnHit() {
        switch (SO_Unit.unitType)
        {
            case 0:
                break;

            case 1:
                break;

            case 2:
                break;
        }
    }

    /*Move the unit to the top of the row of unit corresponding at the tile clicked if possible
    then deselect the unit*/
    public void MoveToTile(S_Tile p_tile)
    {
        foreach (S_Tile tile in p_tile.grid.gridList[p_tile.tileX])
        {
            if (tile.unit == null || tile.unit==this)
            {
                actualTile.unit = null;
                actualTile = tile;
                actualTile.unit = this;
                transform.position = tile.transform.position;
                _grid.unitSelected = null;
                tileX = tile.tileX;
                tileY = tile.tileY;
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
        }
        return;
    }

    //Align the Unit with the collumn overed by the mouse to previsualize where you're aiming
    public void VisualizePosition(S_Tile p_tile)
    {
        transform.position = new Vector3(p_tile.transform.position.x, _grid.startY+ _grid.height);
    }

    private void OnMouseDown()
    {
        if(_grid.unitSelected==null)
        SelectUnit();
    }
    private void OnMouseOver()
    {
        highlight.SetActive(true);
    }
    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }
}
