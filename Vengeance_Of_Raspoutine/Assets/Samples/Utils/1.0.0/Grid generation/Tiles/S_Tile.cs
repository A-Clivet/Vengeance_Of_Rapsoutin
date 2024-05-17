using UnityEngine;
using UnityEngine.SceneManagement;

public class S_Tile : MonoBehaviour
{
    public int m_TileX;
    public int m_TileY;
    public int m_Dist;
    public S_UnitAction unit;
    //public S_Tile m_PreviousTile;
    private S_GridManager grid;
    
    //Pathfinding variable
    //public int m_GCost;
    //public int m_HCost;
    //public int m_FCost;
    //public int m_MoveCost = 0;

    private void Awake()
    {
        grid = S_GridManager.Instance;
    }
}
