using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class S_GridManager : MonoBehaviour
{

    public List<List<S_Tile>> gridList = new();
    public List<Unit> unitList = new();
    public Unit unitSelected;
    public int totalUnitAmount = 0;
    public S_GridManager enemyGrid;
    public S_UnitManager unitManager;
    //public List<Vector2> m_posToFill = new();
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
    //private void Start()
    //{
    //    GameObject test = Instantiate(pfTest, gridList[0][0].transform.position, Quaternion.identity);
    //    test.GetComponent<Unit>().OnSpawn(gridList[0][0]);
    //    GameObject test2 = Instantiate(pfTest, gridList[1][0].transform.position, Quaternion.identity);
    //    test2.GetComponent<Unit>().OnSpawn(gridList[1][0]);
    //}

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
        //Place the camera at the center of the map
        //Camera.main.transform.position = new Vector3((m_Width * m_gridScale.x) / 2, (m_Height * m_gridScale.y) / 2, -10);
        //FillAtPosition();
        //Camera.main.orthographicSize = 7;
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

    //private void FillAtPosition()
    //{
    //    foreach (Vector2 pos in m_posToFill)
    //    {
    //        for (int x = 0; x < m_Width; x++)
    //        {
    //            for (int y = 0; y < m_Height; y++)
    //            {
    //                if (pos.x == m_GridList[x][y].m_TileX && pos.y == m_GridList[x][y].m_TileY)
    //                {
    //                    //Do Stuff
    //                }
    //            }
    //        }
    //    }
    //}
}
