using System.Collections.Generic;
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
    public bool isGridVisible = false;

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

        List<List<Unit>> GridUnit = new();

        for (int x = 0; x < width; x++)
        {
            List<Unit> StateIdleUnit = new();
            List<Unit> StateDefendUnit = new();
            List<Unit> StateAttackUnit = new();
            List<Unit> OrganizedColumn = new();


            for (int y = 0; y < Mathf.Abs(height); y++)
            {
                if (gridList[x][y].unit == null) continue;
                switch (gridList[x][y].unit.sizeY, gridList[x][y].unit.state)
                {
                    case (1, -1):
                        StateIdleUnit.Add(gridList[x][y].unit);
                        break;

                    case (1, 0):
                        StateIdleUnit.Add(gridList[x][y].unit);
                        break;
                    case (2, 0):
                        StateIdleUnit.Add(gridList[x][y].unit);
                        break;
                    case (1, 3):
                        StateIdleUnit.Add(gridList[x][y].unit);
                        break;
                    case (2, 3):
                        StateIdleUnit.Add(gridList[x][y].unit);
                        break;

                    case (1, 1):
                        StateDefendUnit.Add(gridList[x][y].unit);
                        break;
                    case (2, 1):
                        StateDefendUnit.Add(gridList[x][y].unit);
                        break;

                    case (1, 2):
                        StateAttackUnit.Add(gridList[x][y].unit);
                        break;
                    case (2, 2):
                        StateAttackUnit.Add(gridList[x][y].unit);
                        break;
                }
            }

            foreach (Unit u in StateDefendUnit) //will be placed first in the column in order
            {
                OrganizedColumn.Add(u);
            }
            foreach (Unit u in StateAttackUnit) //will be placed middle in the column in order
            {
                OrganizedColumn.Add(u);
            }
            foreach (Unit u in StateIdleUnit) //will be placed last in the column in order
            {
                OrganizedColumn.Add(u);
            }

            GridUnit.Add(OrganizedColumn);

            for(int y = 0; y < OrganizedColumn.Count; y++)
            {
                OrganizedColumn[y].MoveToTile(gridList[x][y]);
            }
        }
        unitManager.UnitCombo(3);
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
                    S_SwapButtonsHandler.Instance.HandleSwapUnitButtonEffects(true, isSwapping);
                    S_UnitCallButtonHandler.Instance.HandleUnitCallButtonInteraction(true, false);
                }
            }
            else
            {
                if (S_GameManager.Instance.swapCounterP2 > 0)
                {
                    isSwapping = !isSwapping;
                    S_SwapButtonsHandler.Instance.HandleSwapUnitButtonEffects(false, isSwapping);
                    S_UnitCallButtonHandler.Instance.HandleUnitCallButtonInteraction(false, false);
                }
            }
        }
        else
        {
            unitSelected.ReturnToBaseTile();
            unitSelected.highlight.SetActive(false);
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
        }
    }
}