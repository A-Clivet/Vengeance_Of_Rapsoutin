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

        callAmount = unitCapacity - grid.totalUnitAmount;

        TextUpdate(callAmount);

        return callAmount;
    }

    public void UnitCalling(){ /* function that will call other functions, will be referenced in the button UI OnClick */

        CallAmountUpdate();


        if (!firstUnitCalled)
        {
            callAmount /= 2;

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

                while (grid.TryFindUnitOntile(tile[X][5], out var unit)) // peut crash à casue du nombre d'unité sur le board non définie 
                {
                    X = ColumnSelector();
                }

                int unitIndex = TypeSelector();

                GameObject unitToSpawn = Instantiate(units[unitIndex].selectedUnit, _unitsParentGameObject.transform); /* unitComponent that will get its value changed */

                Unit unitComponent = unitToSpawn.GetComponent<Unit>();

                unitComponent.unitColor = units[unitIndex].unitColor;

                //unitToSpawn.GetComponent<Unit>().SO_Unit.unitColor = ColorSelector();

                //function to move the unitComponent on the _grid to the right spots
                unitComponent.OnSpawn(grid.gridList[X][Mathf.Abs(grid.height) - 1]);
                unitToSpawn.transform.position = new Vector3(unitComponent.actualTile[0].transform.position.x, unitComponent.grid.startY + unitComponent.grid.height+ unitComponent.actualTile[0].transform.position.y);
                unitComponent.MoveToTile(unitComponent.actualTile[0]);

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
        grid.UnitPriorityCheck();
        grid.unitManager.UnitCombo(3);
        CallAmountUpdate();

    }

    public void TextUpdate(int p_amount)
    {
        text.SetText(p_amount.ToString());
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