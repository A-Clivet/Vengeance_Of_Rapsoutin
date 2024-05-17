using System.Collections.Generic;
using UnityEngine;

public class S_GridManager : MonoBehaviour
{
    public static S_GridManager Instance;

    public List<List<S_Tile>> m_GridList = new();
    public List<Vector2> m_posToFill = new();

    public int m_Width, m_Height;
    [SerializeField] private S_Tile m_tile;

    [Header("Differents tile's types :")]
    public Sprite m_TileSprite;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GenerateGrid();
    }

    // Generate the grid
    private void GenerateGrid()
    {     
        for (int x = 0; x < m_Width; x++)
        {
            m_GridList.Add(new List<S_Tile>());
            for (int y = 0; y < m_Height; y++)
            {
                var spawnedTile = Instantiate(m_tile, new Vector3(x, y, 0), Quaternion.identity, transform);
                m_GridList[x].Add(spawnedTile);
                spawnedTile.GetComponent<SpriteRenderer>().sprite=m_TileSprite;
                spawnedTile.m_TileX = x;
                spawnedTile.m_TileY = y;
            }
        }
        //FillAtPosition();
        //Camera.main.orthographicSize = 7;
    }
    private void FillAtPosition()
    {
        foreach (Vector2 pos in m_posToFill)
        {
            for (int x = 0; x < m_Width; x++)
            {
                for (int y = 0; y < m_Height; y++)
                {
                    if (pos.x == m_GridList[x][y].m_TileX && pos.y == m_GridList[x][y].m_TileY)
                    {
                        //Do Stuff
                    }
                }
            }
        }
    }
}
