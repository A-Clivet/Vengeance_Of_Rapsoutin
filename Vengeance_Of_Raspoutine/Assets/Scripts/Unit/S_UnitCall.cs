using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_UnitCall : MonoBehaviour
{
    /*ui variable*/
    private int callAmount; /* is increased when a unit create a wall, attack or dies */
    public S_GridManager grid;
    public int unitCapacity;
    public bool firstUnitCalled = false;
    public List<List<S_Tile>> tile;
    public TextMeshProUGUI text;
    [SerializeField] private List<GameObject> units = new List<GameObject>();
    public int eliteAmount = 0;

    private void Update()
    {   
        TextUpdate();
    }

    public void Start()
    {
        UnitCalling();
        CallAmountUpdate();
    }

    public int CallAmountUpdate()
    {
        return callAmount = unitCapacity - grid.totalUnitAmount;
    }

    public void UnitCalling()
    { /* function that will call other functions, will be referenced in the button UI OnClick */

        if (!firstUnitCalled)
        {
            CallAmountUpdate();
            callAmount /= 2;
            grid.AllUnitPerColumn = grid.UnitPriorityCheck();
        }

        if (grid.totalUnitAmount < unitCapacity)
        {
            for (int i = 0; i < callAmount; i++)
            {
                int unitType = TypeSelector();
                if (eliteAmount == 3)
                {   
                    unitType = Random.Range(0,3);
                }
                GameObject unitToSpawn = Instantiate(units[unitType]); /* unit that will be spawned onto the grid */
                Unit unitSpawned = unitToSpawn.GetComponent<Unit>();
                //unitToSpawn.GetComponent<Unit>().SO_Unit.unitColor = ColorSelector();
                int X = ColumnSelector();

                if (unitSpawned.sizeY == 2)
                {
                    eliteAmount++;
                    if (unitSpawned.sizeX == 2)
                    {
                        while (X >= 7 || grid.gridList[X][4].unit != null || grid.gridList[X][5].unit != null)
                        {
                            X = ColumnSelector();        
                        }
                    }
                    else 
                    {
                        while (grid.gridList[X][4].unit != null)
                        {
                            X = ColumnSelector();
                        }
                    }
                }
                else
                {
                    while (grid.gridList[X][5].unit != null) // peut crash à casue du nombre d'unité sur le board non définie 
                    {
                        X = ColumnSelector();
                    }
                }

                unitSpawned.tileX = X;
                //function to move the unit on the _grid to the right spots
                unitSpawned.OnSpawn(grid.gridList[X][Mathf.Abs(grid.height) - 1]);

                unitToSpawn.transform.position = new Vector3(unitSpawned.actualTile.transform.position.x, unitSpawned.grid.startY + unitSpawned.grid.height + unitSpawned.actualTile.transform.position.y);
                
                if(unitSpawned.sizeX == 1)
                {
                    unitSpawned.MoveToTile(unitSpawned.actualTile);
                }
                else if(unitSpawned.sizeX == 2)
                {
                    unitSpawned.EliteMoveToTile(unitSpawned.actualTile);
                }
                grid.totalUnitAmount++;
            }

            if (firstUnitCalled)
            {
                S_GameManager.Instance.ReduceActionPointBy1();
            }
            else
            {
                firstUnitCalled = true;
            }
        }
        grid.unitManager.UnitCombo(3);
        TextUpdate();
    }

    public void TextUpdate()
    {
        string buttonText = CallAmountUpdate().ToString();
        text.SetText(buttonText);
    }

    private int ColumnSelector()
    { /* select which column this unit will go to */
        return Random.Range(0, grid.width);
    }
    private int TypeSelector()
    { /* select which type is the unit */
        return Random.Range(0, units.Count);
    }
    private int ColorSelector()
    { /* select which color is the unit */
        return Random.Range(0, 3);
    }
    public List<GameObject> GetUnits()
    {
        return units;
    }
}