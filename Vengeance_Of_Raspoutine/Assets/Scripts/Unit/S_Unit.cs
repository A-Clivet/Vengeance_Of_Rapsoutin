using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Movements :")]
    public S_Tile m_ActualTile;
    public int speed = 10;
    private Vector3 m_TilePos;

    [Header("Ant statistics :")]


    [Header("References :")]
    [SerializeField] private GameObject m_highlight;


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
        Debug.Log(SO_Unit.UnitName);
        Debug.Log(SO_Unit.attack);
        Debug.Log(SO_Unit.defense);
        Debug.Log(SO_Unit.sizeX);
        Debug.Log(SO_Unit.sizeY);
        Debug.Log(SO_Unit.unitType);
        
    }

    //IS ABSOLUTELY NEEDED TO BE CALLED WHEN A UNIT IS INSTANTIATED
    //AND THE REFERENCE TO HIS OWN TILE IS KNOWN
    public void OnSpawn(S_Tile tile)
    {
        _grid = tile.grid;
        actualTile = tile;
        tile.unit = this;
    }

    private void Update()
    {
    }

    //this function execute itself when a ant is spawned
    public void Spawn(S_Tile tile)
    {
        m_ActualTile = tile;
        tile.unit = this;
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
                tile.unit = this;
                actualTile.unit = null;
                actualTile = p_tile;
                transform.position = tile.transform.position;
                highlight.SetActive(false);
                _grid.unitSelected = null;
                break;
            }
        }
    }

    //get the last unit of the row corresponding to the tile clicked
    public void SelectUnit()
    {
        if (actualTile.tileY + 1 > _grid.gridList[actualTile.tileX].Count-1)
        {
            return;
        }
        if (_grid.gridList[actualTile.tileX][actualTile.tileY + 1].unit != null)
        {
            _grid.gridList[actualTile.tileX][actualTile.tileY + 1].unit.SelectUnit();
        }
        else
        {
            highlight.SetActive(true);
            _grid.unitSelected = this;
        }
        return;
    }

    public void VisualizePosition(S_Tile p_tile)
    {
        transform.position = new Vector3(p_tile.transform.position.x, _grid.startY+ _grid.height);
    }

    private void OnMouseDown()
    {
        if(_grid.unitSelected==null)
        SelectUnit();
    }
}
