using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class S_GridManager : MonoBehaviour
{

    public List<List<S_Tile>> gridList = new();
    public List<Unit> unitList = new();
    public List<List<Unit>> AllUnitPerColumn = new();
    public Unit unitSelected;
    public int totalUnitAmount = 0;
    public S_GridManager enemyGrid;
    public S_UnitManager unitManager;
    private Vector3 _gridScale;

    public int width, height;
    public float startX, startY;
    public bool isSwapping = false;
    [SerializeField] private S_Tile _tile;

    [Header("Differents tile's types :")]
    public Sprite tileSprite;

    private Color _spriteColor = new Color(0,0,0,1);
    private Color _transparentColor = new Color(0,0,0,-1);
    private bool _isGridVisible = false;

    private void Awake()
    {
        _gridScale = _tile.transform.localScale;
        for(int i = 0; i < width; i++)
        {
            AllUnitPerColumn.Add(new());
        }
        GenerateGrid(startX,startY);
    }

    // Generate the grid
    private void GenerateGrid(float p_x, float p_y)
    {
        if (height >= 0)
        {
            for (int x = 0; x < width; x++)
            {
                gridList.Add(new List<S_Tile>());
                for (int y = 0; y < height; y++)
                {
                    var spawnedTile = Instantiate(_tile, new Vector3((x +p_x) * (_gridScale.x + 0.1f), (y +p_y) * _gridScale.y, 0), Quaternion.identity, transform);
                    gridList[x].Add(spawnedTile);
                    spawnedTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
                    spawnedTile.GetComponent<SpriteRenderer>().color += _transparentColor;
                    spawnedTile.tileX = x;
                    spawnedTile.tileY = y;
                }
            }
        }
        else
        {
            for (int x = 0; x < width; x++)
            {
                gridList.Add(new List<S_Tile>());
                for (int y = 0;y>height;y--)
                {
                    var spawnedTile = Instantiate(_tile, new Vector3((x + p_x) * (_gridScale.x + 0.1f), (y + p_y) * _gridScale.y, 0), Quaternion.identity, transform);
                    gridList[x].Add(spawnedTile);
                    spawnedTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
                    spawnedTile.GetComponent<SpriteRenderer>().color += _transparentColor;
                    spawnedTile.tileX = x;
                    spawnedTile.tileY = -y;
                }
            }
        }
    }

    //Change the state of the S_Tile script of the grid, enabling or desabling it alternatively.
    public void ActualizeGrid()
    {
        for (int x = 0; x < gridList.Count; x++)
        {
            for (int y = 0; y < gridList[x].Count; y++)
            {
                gridList[x][y].GetComponent<S_Tile>().enabled = !gridList[x][y].GetComponent<S_Tile>().enabled;
            }
        }
    }

    public void HighLightGrid(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _isGridVisible = !_isGridVisible;
            if (_isGridVisible)
            {
                for (int i = 0; i < gridList.Count; i++)
                {
                    for (int j = 0; j < gridList[i].Count; j++)
                    {
                        gridList[i][j].GetComponent<SpriteRenderer>().color += _spriteColor;
                    }
                }
                for (int i = 0; i < unitList.Count; i++)
                {
                    unitList[i].statsCanvas.SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < gridList.Count; i++)
                {
                    for (int j = 0; j < gridList[i].Count; j++)
                    {
                        gridList[i][j].GetComponent<SpriteRenderer>().color += _transparentColor;
                    }
                }
                for (int i = 0; i < unitList.Count; i++)
                {
                    unitList[i].statsCanvas.SetActive(false);
                }
            }
        }
    }
    
    public List<List<Unit>> UnitPriorityCheck() // check the units priority, order is : wall (1), charging(2), idle(0)
    {
        Debug.Log("UnitPriority entered / Grid changed");
        List<List<Unit>> GridUnit = new();


        for (int x = 0; x < width; x++)
        {
            List<Unit> StateIdleUnit = new();
            List<Unit> StateDefendUnit = new();
            List<Unit> StateAttackUnit = new();
            List<Unit> OrganizedColumn = new();


            for (int y = 0; y < Mathf.Abs(height); y++)
            {
                
                if(gridList[x][y].unit == null ) continue;
                if(gridList[x][y].unit.sizeY == 2)
                {
                    if (gridList[x][y].unit.state == 0 || gridList[x][y].unit.state == 3)
                    {
                        StateIdleUnit.Add(gridList[x][y].unit);
                    }
                    else if (gridList[x][y].unit.state == 1)
                    {
                        StateDefendUnit.Add(gridList[x][y].unit);
                    }
                    else if (gridList[x][y].unit.state == 2)
                    {
                        StateAttackUnit.Add(gridList[x][y].unit);
                    }
                    continue;
                }
                else if (gridList[x][y].unit.state == 0 || gridList[x][y].unit.state == 3) StateIdleUnit.Add(gridList[x][y].unit); //will be placed last in the column in order
                else if (gridList[x][y].unit.state == 1) StateDefendUnit.Add(gridList[x][y].unit); //will be placed first in the column in order
                else if (gridList[x][y].unit.state == 2) StateAttackUnit.Add(gridList[x][y].unit); //will be placed middle in the column in order
                //gridList[x][y].unit.actualTile = null;
            }

            foreach (Unit u in StateDefendUnit)
            {
                OrganizedColumn.Add(u);
            }
            foreach (Unit u in StateAttackUnit)
            {
                OrganizedColumn.Add(u);
            }
            foreach (Unit u in StateIdleUnit)
            {
                OrganizedColumn.Add(u);
            }

            foreach (S_Tile tile in gridList[x])
            {
                tile.unit = null;
            }

            GridUnit.Add(OrganizedColumn);


            Debug.Log("Current Column value : " + x);

            for(int y = 0; y < OrganizedColumn.Count; y++)
            {
                if(OrganizedColumn[y].sizeY == 2)
                {
                    Debug.Log("UnitPrio MoveToTile sizeY 2");
                    Debug.Log("Unit MoveToTile to move to (" + x + "," + y + ")");
                    OrganizedColumn[y].MoveToTile(gridList[x][(Mathf.Abs(height) - OrganizedColumn[y].sizeY)]);
                }
                else
                {
                    Debug.Log("UnitPrio MoveToTile size 1");
                    Debug.Log("Unit MoveToTile to move to (" + x + "," + y + ")");
                    OrganizedColumn[y].MoveToTile(gridList[x][(Mathf.Abs(height) - OrganizedColumn[y].sizeY)]);
                }
                Debug.Log("#################################");
            }
            GridUnit[GridUnit.Count - 1] = OrganizedColumn;
        }
        return GridUnit; 
    }

    public void SwapUnits(S_Tile p_tile1, S_Tile p_tile2, Unit p_unit1, Unit p_unit2)
    {
        p_tile1.unit = p_unit2;
        p_unit2.actualTile = p_tile1;
        p_unit2.tileX = p_tile1.tileX;
        p_unit2.tileY = p_tile1.tileY;
        p_tile2.unit = p_unit1;
        p_unit1.actualTile = p_tile2;
        p_unit1.tileX = p_tile2.tileX;
        p_unit1.tileY = p_tile2.tileY;
        isSwapping = false;
    }

    public void SwapBtn()
    {
        if (unitSelected == null)
        {
            if (S_GameManager.Instance.isPlayer1Turn)
            {
                if (S_GameManager.Instance.swapCounterP1 > 0)
                {
                    isSwapping = !isSwapping;
                }
            }
            else
            {
                if (S_GameManager.Instance.swapCounterP2 > 0)
                {
                    isSwapping = !isSwapping;
                }
            }
        }
        else
        {
            unitSelected.ReturnToBaseTile();
            unitSelected = null;
            if (S_GameManager.Instance.isPlayer1Turn)
            {
                if (S_GameManager.Instance.swapCounterP1 > 0)
                {
                    isSwapping = !isSwapping;
                    S_UnitCallButtonHandler.Instance.HandleUnitCallButtonInteraction(true, true);
                }
            }
            else
            {
                if (S_GameManager.Instance.swapCounterP2 > 0)
                {
                    isSwapping = !isSwapping;
                    S_UnitCallButtonHandler.Instance.HandleUnitCallButtonInteraction(false, true);
                }
            }
        }
    }
}