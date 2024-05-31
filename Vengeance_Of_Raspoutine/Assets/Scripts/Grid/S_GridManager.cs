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
    [SerializeField] private S_Tile _tile;

    [Header("Differents tile's types :")]
    public Sprite tileSprite;

    private Color _spriteColor = new Color(0,0,0,1);
    private Color _transparentColor = new Color(0,0,0,-1);
    private bool _isGridVisible = false;

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
                    var spawnedTile = Instantiate(_tile, new Vector3((x +p_x) * _gridScale.x, (y +p_y) * _gridScale.y, 0), Quaternion.identity, transform);
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
                    var spawnedTile = Instantiate(_tile, new Vector3((x + p_x) * _gridScale.x, (y + p_y) * _gridScale.y, 0), Quaternion.identity, transform);
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
                if (gridList[x][y].unit.state == 0) StateIdleUnit.Add(gridList[x][y].unit);
                if (gridList[x][y].unit.state == 1) StateDefendUnit.Add(gridList[x][y].unit);
                if (gridList[x][y].unit.state == 2) StateAttackUnit.Add(gridList[x][y].unit);
                gridList[x][y].unit.actualTile = null;
                gridList[x][y].unit = null;
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

            GridUnit.Add(OrganizedColumn);

            for(int y = 0; y < OrganizedColumn.Count; y++)
            {
                OrganizedColumn[y].SwitchUnit(gridList[x][y]);
            }
        }

        return GridUnit; 
    }
}