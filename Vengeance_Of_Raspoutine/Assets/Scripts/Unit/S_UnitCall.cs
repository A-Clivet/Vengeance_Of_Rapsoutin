using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_UnitCall : MonoBehaviour
{
    /*ui variable*/
    private int callAmount; /* is increased when a unit [[create a wall,]] attack or dies */
    public S_GridManager grid;
    public List<List<S_Tile>> tile;
    [SerializeField] private List<GameObject> units = new List<GameObject>();

    public void UnitCalling(){ /* function that will call other functions, will be referenced in the button UI OnClick */
        for (int i = 0; i < callAmount; i++)
        {
            GameObject unitToSpawn = Instantiate(units[TypeSelector()]); /* unit that will get its value changed */
            unitToSpawn.GetComponent<Unit>().SO_Unit.unitColor = ColorSelector();
            unitToSpawn.GetComponent<Unit>().tileY = ColumnSelector();

            //function to move the unit on the grid to the right spots
            unitToSpawn.GetComponent<Unit>().OnSpawn(unitToSpawn.GetComponent<Unit>().actualTile);
            unitToSpawn.GetComponent<Unit>().MoveToTile(tile[ColumnSelector()][grid.height]);
        }
    }

    private int ColumnSelector()
    { /* select which column this unit will go to */
        return Random.Range(0, 9);
    }
    private int TypeSelector()
    { /* select which type is the unit */
        return Random.Range(0, 4);
    }
    private int ColorSelector()
    { /* select which color is the unit */
        return Random.Range(0, 4);
    }
}
