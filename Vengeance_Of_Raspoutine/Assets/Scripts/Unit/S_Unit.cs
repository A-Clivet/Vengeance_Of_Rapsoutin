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

    private void Update()
    {
    }

    //this function execute itself when a ant is spawned
    public void Spawn(S_Tile tile)
    {
        m_ActualTile = tile;
        tile.unit = this;
    }

    //move the ant and update the number of movements left. If none, deselect the ant.
    public bool MoveToTile(S_Tile tile)
    {

        m_TilePos = tile.transform.position;
        //  transform.position = tile.transform.position;
        m_ActualTile.unit = null;
        tile.GetComponent<S_Tile>().unit = this;
        m_ActualTile = tile.GetComponent<S_Tile>();
        return true;
    }

    //select the Unit
    private void OnMouseDown()
    {
        m_highlight.SetActive(true);
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
}
