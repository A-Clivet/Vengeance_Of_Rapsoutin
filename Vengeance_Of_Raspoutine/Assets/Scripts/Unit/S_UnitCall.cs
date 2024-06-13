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

    public void UnitCalling(){ /* function that will call other functions, will be referenced in the button UI OnClick */

        if (!firstUnitCalled)
        {
            CallAmountUpdate();
            callAmount /= 2;
            grid.AllUnitPerColumn = grid.UnitPriorityCheck();
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
                int X = ColumnSelector();
                while (tile[X][5].unit != null) // peut crash à casue du nombre d'unité sur le board non définie 
                {
                    X = ColumnSelector();
                }

                GameObject unitToSpawn = Instantiate(units[TypeSelector()], _unitsParentGameObject.transform); /* unit that will get its value changed */
                //unitToSpawn.GetComponent<Unit>().SO_Unit.unitColor = ColorSelector();
                unitToSpawn.GetComponent<Unit>().tileX = X;

                //function to move the unit on the _grid to the right spots
                unitToSpawn.GetComponent<Unit>().OnSpawn(grid.gridList[X][Mathf.Abs(grid.height) - 1]);
                unitToSpawn.transform.position = new Vector3(unitToSpawn.GetComponent<Unit>().actualTile.transform.position.x, unitToSpawn.GetComponent<Unit>().grid.startY + unitToSpawn.GetComponent<Unit>().grid.height+ unitToSpawn.GetComponent<Unit>().actualTile.transform.position.y);
                unitToSpawn.GetComponent<Unit>().MoveToTile(unitToSpawn.GetComponent<Unit>().actualTile);
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
        return Random.Range(0, 3);
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