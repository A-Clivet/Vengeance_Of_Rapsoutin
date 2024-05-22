using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class S_UnitCall : MonoBehaviour
{
    /*ui variable*/
    public int callAmount; /* is increased when a unit [[create a wall,]] attack or dies */
 // put this varaible in GridManager
    public S_GridManager grid;
    public List<List<S_Tile>> tile;
    [SerializeField] private List<GameObject> units = new List<GameObject>();

    public void Start()
    {
        tile = grid.gridList;
    }

    public void UnitCalling(){ /* function that will call other functions, will be referenced in the button UI OnClick */
        if (grid.totalUnitAmount < 48)
        {
            for (int i = 0; i < callAmount; i++)
            {
                GameObject unitToSpawn = Instantiate(units[TypeSelector()]); /* unit that will get its value changed */
                unitToSpawn.GetComponent<Unit>().SO_Unit.unitColor = ColorSelector();
                int p_X = ColumnSelector();
                Debug.Log(p_X);
                Debug.Log(tile[p_X][5].unit);
                while (tile[p_X][5].unit != null)
                {
                    Debug.Log(tile[p_X][5].unit);
                    Debug.Log(p_X);
                    p_X = ColumnSelector();
                }

                unitToSpawn.GetComponent<Unit>().tileX = p_X;

                //function to move the unit on the grid to the right spots
                unitToSpawn.GetComponent<Unit>().OnSpawn(grid.gridList[p_X][Mathf.Abs(grid.height) - 1]);
                unitToSpawn.GetComponent<Unit>().MoveToTile(unitToSpawn.GetComponent<Unit>().actualTile);
                grid.totalUnitAmount++;
                Debug.Log("TileX : " + unitToSpawn.GetComponent<Unit>().tileX + "TileY :" + unitToSpawn.GetComponent<Unit>().tileY);
            }
        }
    }

    private int ColumnSelector()
    { /* select which column this unit will go to */
        return Random.Range(0, grid.width);
    }
    private int TypeSelector()
    { /* select which type is the unit */
        return Random.Range(0, 3);
    }
    private int ColorSelector()
    { /* select which color is the unit */
        return Random.Range(0, 3);
    }
}
