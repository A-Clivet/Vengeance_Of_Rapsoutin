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
        Debug.Log("UnitPriority entered");
        List<List<Unit>> GridUnit = new();
        List<Unit> BigUnits = new();


        for (int x = 0; x < width; x++)
        {
            List<Unit> StateIdleUnit = new();
            List<Unit> StateDefendUnit = new();
            List<Unit> StateAttackUnit = new();
            List<Unit> OrganizedColumn = new();
            int BigUnitY = 123456789;


            for (int y = 0; y < Mathf.Abs(height); y++)
            {
                
                if(gridList[x][y].unit == null ) continue;
                if (gridList[x][y].unit.sizeY == 2 && !BigUnits.Contains(gridList[x][y].unit))
                {
                    BigUnits.Add(gridList[x][y].unit);

                    if(gridList[x][y].unit.sizeX == 2) BigUnitY = gridList[x][y].tileY;

                    if (BigUnits[0].state == 0 || BigUnits[0].state == 3)
                    {
                        StateIdleUnit.Add(BigUnits[BigUnits.Count - 1]);
                    }
                    else if (BigUnits[0].state == 1)
                    {
                        StateDefendUnit.Add(BigUnits[BigUnits.Count - 1]);
                    }
                    else if (BigUnits[0].state == 2)
                    {
                        StateAttackUnit.Add(BigUnits[BigUnits.Count - 1]);
                    }
                    y++;
                }
                else if (gridList[x][y].unit.state == 0 || gridList[x][y].unit.state == 3) StateIdleUnit.Add(gridList[x][y].unit); //will be placed last in the column in order
                else if (gridList[x][y].unit.state == 1) StateDefendUnit.Add(gridList[x][y].unit); //will be placed first in the column in order
                else if (gridList[x][y].unit.state == 2) StateAttackUnit.Add(gridList[x][y].unit); //will be placed middle in the column in order
                //gridList[x][y].unit.actualTile = null;
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


            for (int y = 0; y < OrganizedColumn.Count; y++)
            {
                Debug.Log("BigUnit Y value : " + BigUnitY);
                Debug.Log("OrganizedColumn Y value : " + OrganizedColumn[y].tileY);

                if (BigUnitY == OrganizedColumn[y].tileY || BigUnitY + 1 == OrganizedColumn[y].tileY)
                {
                    continue;
                }
                else
                {
                    switch (OrganizedColumn[y].sizeX, OrganizedColumn[y].sizeX)
                    {
                        case (1, 1):

                            OrganizedColumn[y].actualTile.unit = null;

                            break;


                        case (1, 2):

                            gridList[OrganizedColumn[y].tileX][OrganizedColumn[y].tileY + 1].unit = null;
                            OrganizedColumn[y].actualTile.unit = null;
                            break;


                        case (2, 2):

                            gridList[OrganizedColumn[y].tileX + 1][OrganizedColumn[y].tileY + 1].unit = null;
                            gridList[OrganizedColumn[y].tileX][OrganizedColumn[y].tileY + 1].unit = null;
                            gridList[OrganizedColumn[y].tileX + 1][OrganizedColumn[y].tileY].unit = null;
                            OrganizedColumn[y].actualTile.unit = null;
                            break;

                        default:

                            break;
                    }
                }
            }

            GridUnit.Add(OrganizedColumn);


            Debug.Log("Current Column value : " + x);

            for(int y = 0; y < OrganizedColumn.Count; y++)
            {
                if (gridList[x][y].tileY != BigUnitY || gridList[x][y].tileY != BigUnitY +1)
                {
                    if (OrganizedColumn[y].sizeX == 2)
                    {

                        BigUnitY = OrganizedColumn[y].tileY;
                        Debug.Log("UnitPrio MoveToTile sizeX 2");
                        OrganizedColumn[y].MoveToTile(gridList[x][gridList[x].Count - OrganizedColumn[y].sizeY]);
                        continue;
                    }
                    else if(OrganizedColumn[y].sizeY == 2)
                    {
                        if (gridList[x][y].tileY != BigUnitY - 1)
                        {
                            Debug.Log("UnitPrio MoveToTile sizeY 2");
                            OrganizedColumn[y].MoveToTile(gridList[x][gridList[x].Count - OrganizedColumn[y].sizeY]);
                        }
                        else
                        {
                            Debug.Log("UnitPrio MoveToTile sizeY 2 forced");
                            OrganizedColumn[y].ForceOnTile(gridList[x][BigUnitY + 1]);
                        }
                    }
                    else
                    {
                        Debug.Log("UnitPrio MoveToTile size 1");
                        OrganizedColumn[y].MoveToTile(gridList[x][gridList[x].Count - OrganizedColumn[y].sizeY]);
                    }
                }
                else
                {
                    Debug.Log("UnitPrio MoveToTile size 1 forced");
                    OrganizedColumn[y].ForceOnTile(gridList[x][BigUnitY + 1]);
                }
                Debug.Log("#################################");
            }

            GridUnit[GridUnit.Count - 1] = OrganizedColumn;
        }

        return GridUnit; 
    }
}