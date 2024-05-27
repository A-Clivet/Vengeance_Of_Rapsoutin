using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_UnitCall : MonoBehaviour
{
    public static S_UnitCall Instance;
    /*ui variable*/
    public int callAmount; /* is increased when a unit [[create a wall,]] attack or dies */
    public S_UnitManager unitManager;
    public S_GridManager gridP1;
    public S_GridManager gridP2;
    public List<List<S_Tile>> tile;
    [SerializeField] private List<GameObject> units = new List<GameObject>();


    private S_GridManager _grid;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void UnitCalling(){ /* function that will call other functions, will be referenced in the button UI OnClick */
        if (S_GameManager.Instance.isPlayer1Turn)
        {
            tile = gridP1.gridList;
            _grid = gridP1;
        }
        else
        {
            tile = gridP2.gridList;
            _grid = gridP2;
        }
        
        if (_grid.totalUnitAmount < 48)
        {
            for (int i = 0; i < callAmount; i++)
            {
                GameObject unitToSpawn = Instantiate(units[TypeSelector()]); /* unit that will get its value changed */
                unitToSpawn.GetComponent<Unit>().SO_Unit.unitColor = ColorSelector();
                int p_X = ColumnSelector();
                while (tile[p_X][5].unit != null)
                {
                    p_X = ColumnSelector();
                }

                unitToSpawn.GetComponent<Unit>().tileX = p_X;

                //function to move the unit on the _grid to the right spots
                unitToSpawn.GetComponent<Unit>().OnSpawn(_grid.gridList[p_X][Mathf.Abs(_grid.height) - 1]);
                unitToSpawn.transform.position = new Vector3(unitToSpawn.GetComponent<Unit>().actualTile.transform.position.x, unitToSpawn.GetComponent<Unit>()._grid.startY + unitToSpawn.GetComponent<Unit>()._grid.height+ unitToSpawn.GetComponent<Unit>().actualTile.transform.position.y);
                unitToSpawn.GetComponent<Unit>().MoveToTile(unitToSpawn.GetComponent<Unit>().actualTile);
                _grid.totalUnitAmount++;

                if(i == callAmount - 1) // ca sert a quoi ?? faut commenter clairement !!
                {
                    //unitManager.CheckUnitFormation(unitToSpawn.GetComponent<Unit>());
                }
            }
            S_GameManager.Instance.ReduceActionPointBy1();
        }

    }

    private int ColumnSelector()
    { /* select which column this unit will go to */
        return Random.Range(0, _grid.width);
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
