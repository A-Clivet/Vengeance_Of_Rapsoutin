using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static S_GameManager;
using static S_UnitSelectorMenu;
using Random = UnityEngine.Random;

public class S_UnitCall : MonoBehaviour
{
    /*ui variable*/
    private int callAmount; /* is increased when a unitComponent create a wall, attack or dies */
    public S_GridManager grid;
    public int unitCapacity;
    public bool firstUnitCalled = false;
    public List<List<S_Tile>> tile;
    public TextMeshProUGUI text;

    public List<PlayersSelectedUnit> units = new();

    List<GameObject> _unitsPrefabList = new();

    GameObject _unitsParentGameObject;

    private void Update()
    {
        TextUpdate();
    }

    private void Awake()
    {
        // Setting up local variables
        _unitsParentGameObject = S_UnitCallButtonHandler.Instance.unitsParentGameObject;

        foreach (PlayersSelectedUnit playersSelectedUnit in units)
        {
            _unitsPrefabList.Add(playersSelectedUnit.selectedUnit);
        }

        for (int i = 0; i < _unitsPrefabList.Count; i++)
        {
            _unitsPrefabList[i].GetComponent<Unit>().ResetBuffs();
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
            }
            else if (S_GameManager.Instance.currentTurn == TurnEmun.Player2Turn)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.MonsterWarHorn, this.transform.position);
            }

            for (int i = 0; i < callAmount; i++)
            {
                int X = ColumnSelector();

                while (tile[X][5].unit != null) // peut crash à casue du nombre d'unité sur le board non définie 
                {
                    X = ColumnSelector();
                }

                int unitIndex = TypeSelector();

                GameObject unitToSpawn = Instantiate(units[unitIndex].selectedUnit, _unitsParentGameObject.transform); /* unitComponent that will get its value changed */

                Unit unitComponent = unitToSpawn.GetComponent<Unit>();

                unitComponent.unitColor = units[unitIndex].unitColor;

                //unitToSpawn.GetComponent<Unit>().SO_Unit.unitColor = ColorSelector();
                unitComponent.tileX = X;

                //function to move the unitComponent on the _grid to the right spots
                unitComponent.OnSpawn(grid.gridList[X][Mathf.Abs(grid.height) - 1]);
                unitToSpawn.transform.position = new Vector3(unitComponent.actualTile.transform.position.x, unitComponent.grid.startY + unitComponent.grid.height+ unitComponent.actualTile.transform.position.y);
                unitComponent.MoveToTile(unitComponent.actualTile);

                grid.totalUnitAmount++;

                if (grid.isGridVisible)
                {
                    unitComponent.statsCanvas.SetActive(true);
                }
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
    { /* select which column this unitComponent will go to */
        return Random.Range(0, grid.width);
    }
    private int TypeSelector()
    { /* select which type is the unitComponent */
        return Random.Range(0, 3);
    }
    private int ColorSelector()
    { /* select which color is the unitComponent */
        return Random.Range(0, 3);
    }
    public List<GameObject> GetUnits()
    {
        return _unitsPrefabList;
    }
}