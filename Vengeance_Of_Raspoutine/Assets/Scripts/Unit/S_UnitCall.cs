using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (S_GameManager.Instance.isPlayer1Turn)
        {
            tile = grid.gridList;
        }

        if (!S_GameManager.Instance.isPlayer1Turn)
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
        }
        
        if (S_GameManager.Instance.isPlayer1Turn)
        {
            tile = grid.gridList;
        }

        if (!S_GameManager.Instance.isPlayer1Turn)
        {
            tile = grid.gridList;
        }

        if (grid.totalUnitAmount < unitCapacity)
        {
            for (int i = 0; i < callAmount; i++)
            {
                int X = ColumnSelector();
                while (tile[X][5].unit != null) // peut crash à casue du nombre d'unité sur le board non définie 
                {
                    X = ColumnSelector();
                }

                GameObject unitToSpawn = Instantiate(units[TypeSelector()]); /* unit that will get its value changed */
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