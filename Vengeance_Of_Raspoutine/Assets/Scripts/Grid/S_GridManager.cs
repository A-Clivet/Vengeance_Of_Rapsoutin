using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class S_GridManager : MonoBehaviour
{

    public List<List<S_Tile>> gridList = new();
    public List<Unit> unitList = new();
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
    public bool isGridVisible = false;

    [Header("UI :")] 
    public GameObject turnCounter;
    
    private void Awake()
    {
        _gridScale = _tile.transform.localScale;

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
            isGridVisible = !isGridVisible;
            if (isGridVisible)
            {
                turnCounter.SetActive(true);
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
                turnCounter.SetActive(false);
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
    public bool IsOutOfIndex(int p_x, int p_y)
    {
        if(p_x<0 || p_x >=width || p_y<0 || p_y >= height)
        {
            return true;
        }
        return false;
    }
    public bool TryFindUnitOntile(S_Tile p_tile, out Unit p_unit)
    {
        p_unit = null;
        foreach (Unit u in unitList)
        {
            if (u.actualTile.Contains(p_tile))
            {
                p_unit = u;
                return true;
            }
        }
        return false;
    }


    public List<List<Unit>> UnitPriorityCheck() // check the units priority, order is : wall (1), charging(2), idle(0)
    {
        List<List<Unit>> GridUnit = new();
        List<Unit> StateIdleUnit = new();
        List<Unit> StateDefendUnit = new();
        List<Unit> StateAttackUnit = new();


        for (int x = 0; x < width; x++)
        {
            StateIdleUnit.Clear();
            StateDefendUnit.Clear();
            StateAttackUnit.Clear();
            List<Unit> OrganizedColumn = new();


            for (int y = 0; y < Mathf.Abs(height); y++)
            {
                if (TryFindUnitOntile(gridList[x][y], out var unit))
                {
                    if (unit.state == 0 || unit.state == 3) StateIdleUnit.Add(unit);
                    if (unit.state == 1) StateDefendUnit.Add(unit);
                    if (unit.state == 2) StateAttackUnit.Add(unit);
                    unit.actualTile = new List<S_Tile>();
                }
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

            for(int y = 0; y < OrganizedColumn.Count; y++)
            {
                OrganizedColumn[y].SwitchUnit(gridList[x][0]);
            }
            GridUnit.Add(OrganizedColumn);
        }
        return GridUnit; 
    }

    public void SwapUnits(S_Tile p_tile1, S_Tile p_tile2, Unit p_unit1, Unit p_unit2)
    {
        p_unit1.actualTile.Clear();
        p_unit2.actualTile.Clear();
        p_unit1.actualTile.Add(p_tile2);
        p_unit2.actualTile.Add(p_tile1);
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
                    S_SwapButtonsHandler.Instance.HandleSwapUnitButtonEffects(true, isSwapping);
                    S_UnitCallButtonHandler.Instance.HandleUnitCallButtonInteraction(true, !isSwapping);
                }
            }
            else
            {
                if (S_GameManager.Instance.swapCounterP2 > 0)
                {
                    isSwapping = !isSwapping;
                    S_SwapButtonsHandler.Instance.HandleSwapUnitButtonEffects(false, isSwapping);
                    S_UnitCallButtonHandler.Instance.HandleUnitCallButtonInteraction(false, !isSwapping);
                }
            }
        }
        else
        {
            unitSelected.ReturnToBaseTile();
            unitSelected.highlight.SetActive(false);
            unitSelected.shadow.SetActive(false);
            unitSelected = null;
            if (S_GameManager.Instance.isPlayer1Turn)
            {
                if (S_GameManager.Instance.swapCounterP1 > 0)
                {
                    isSwapping = !isSwapping;
                    S_UnitCallButtonHandler.Instance.HandleUnitCallButtonInteraction(true, true);
                    S_SwapButtonsHandler.Instance.HandleSwapUnitButtonEffects(true, isSwapping);
                }
            }
            else
            {
                if (S_GameManager.Instance.swapCounterP2 > 0)
                {
                    isSwapping = !isSwapping;
                    S_UnitCallButtonHandler.Instance.HandleUnitCallButtonInteraction(false, true);
                    S_SwapButtonsHandler.Instance.HandleSwapUnitButtonEffects(false, isSwapping);
                }
            }
            S_GameManager.Instance.DeactivateGrid();
        }
    }
}