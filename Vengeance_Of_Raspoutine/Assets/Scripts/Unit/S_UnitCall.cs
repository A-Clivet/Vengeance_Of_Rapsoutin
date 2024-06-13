using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static S_GameManager;
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

    GameObject _unitsParentGameObject;

    private void Update()
    {   
        TextUpdate();
    }

        private void Awake()
    {
        // Setting up local variables
        _unitsParentGameObject = S_UnitCallButtonHandler.Instance.unitsParentGameObject;

        for (int i = 0; i < units.Count; i++)
        {
            units[i].GetComponent<Unit>().ResetBuffs();
        }
    }

    public void Start()
    {
        UnitCalling();
        grid.AllUnitPerColumn = grid.UnitPriorityCheck();
        CallAmountUpdate();
    }

    public int CallAmountUpdate()
    {
        if (S_GameManager.Instance.currentTurn == TurnEmun.Player1Turn)
        {
            tile = grid.gridList;
        }

        if (S_GameManager.Instance.currentTurn == TurnEmun.Player2Turn)
        {
            tile = grid.gridList;
        }

        return callAmount = unitCapacity - grid.totalUnitAmount;
    }

    public void UnitCalling()
    { /* function that will call other functions, will be referenced in the button UI OnClick */

        if (!firstUnitCalled)
        {
            CallAmountUpdate();
            callAmount /= 2;
        }

        if (grid.totalUnitAmount < unitCapacity)
        {
            if (S_GameManager.Instance.currentTurn == TurnEmun.Player1Turn)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.UssrWarHorn, this.transform.position);
                Debug.Log("1");
            }
            else if (S_GameManager.Instance.currentTurn == TurnEmun.Player2Turn)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.MonsterWarHorn, this.transform.position);
                Debug.Log("2");
            }
            for (int i = 0; i < callAmount; i++)
            {
                int unitType = TypeSelector();
                if (eliteAmount >= 2)
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
                        while (X == 7 || grid.gridList[X][4].unit != null || grid.gridList[X + 1][4].unit != null)
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
                    while (grid.gridList[X][5].unit != null) 
                    {
                        X = ColumnSelector();
                    }
                }

                GameObject unitToSpawn = Instantiate(units[TypeSelector()], _unitsParentGameObject.transform); /* unit that will get its value changed */
                //unitToSpawn.GetComponent<Unit>().SO_Unit.unitColor = ColorSelector();
                unitSpawned.tileX = X;

                //function to move the unit on the _grid to the right spots
                unitSpawned.OnSpawn(grid.gridList[X][Mathf.Abs(grid.height) - unitSpawned.sizeY]);

                unitToSpawn.transform.position = new Vector3(unitSpawned.actualTile.transform.position.x, unitSpawned.grid.startY + unitSpawned.grid.height + unitSpawned.actualTile.transform.position.y);

                unitSpawned.MoveToTile(grid.gridList[unitSpawned.tileX][Mathf.Abs(grid.height) - unitSpawned.sizeY]);

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