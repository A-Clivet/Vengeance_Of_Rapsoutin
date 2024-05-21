
//using UnityEngine;

//public class S_UnitAction : MonoBehaviour
//{
//    [Header("Movements :")]
//    public S_Tile m_ActualTile;
//    public int speed = 10;
//    private Vector3 m_TilePos;

//    [Header("Ant statistics :")]


//    [Header("References :")]
//    [SerializeField] private GameObject m_highlight;
    

    
//    private void Start()
//    {
//        speed = 10;

//    }

//    private void Update()
//    {
//    }

//    //this function execute itself when a ant is spawned
//    public void Spawn(S_Tile tile)
//    {
//        m_ActualTile = tile;
//        tile.unit = this;
//    }

//    //move the ant and update the number of movements left. If none, deselect the ant.
//    public bool MoveToTile(S_Tile tile)
//    {

//            m_TilePos=tile.transform.position;
//          //  transform.position = tile.transform.position;
//            m_ActualTile.unit = null;
//            tile.GetComponent<S_Tile>().unit = this;
//            m_ActualTile = tile.GetComponent<S_Tile>();
//            return true;
//    }

//    //select the Unit
//    private void OnMouseDown()
//    {
//        m_highlight.SetActive(true);
//    }
//}