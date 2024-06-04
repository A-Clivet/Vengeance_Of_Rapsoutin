using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static S_Instantiator;

public class S_RemoveUnit : MonoBehaviour
{
    public static S_RemoveUnit Instance;

    public Unit hoveringUnit;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void RemoveUnit(InputAction.CallbackContext p_context)
    {
        if (p_context.started)
        {
            if (hoveringUnit != null && (hoveringUnit.state == 0 || hoveringUnit.state == 1)) // remove les unit.state = 1 
            {
                hoveringUnit.grid.unitList.Remove(hoveringUnit);
                hoveringUnit.grid.AllUnitPerColumn[hoveringUnit.tileX].Remove(hoveringUnit);
                hoveringUnit.actualTile.unit = null;
                foreach (S_Tile tile in hoveringUnit.grid.gridList[hoveringUnit.tileX])
                {
                    if (tile.unit != null)
                    {
                        tile.unit.MoveToTile(hoveringUnit.actualTile);
                    }
                }
                hoveringUnit.grid.totalUnitAmount -= 1;
                Destroy(hoveringUnit.gameObject);
                hoveringUnit = null;
                S_GameManager.Instance.ReduceActionPointBy1();
            }   
        }
    }

    public void RemoveUnitAI(S_Tile p_tile)
    {
        if (hoveringUnit != null && (hoveringUnit.state == 0 || hoveringUnit.state == 1)) // remove les unit.state = 1 
        {
            hoveringUnit.grid.unitList.Remove(hoveringUnit);
            hoveringUnit.actualTile.unit = null;
            foreach (S_Tile tile in hoveringUnit.grid.gridList[hoveringUnit.tileX])
            {
                if (tile.unit != null)
                {
                    tile.unit.MoveToTile(hoveringUnit.actualTile);
                }
            }
            hoveringUnit.grid.totalUnitAmount -= 1;
            Destroy(hoveringUnit.gameObject);
            hoveringUnit = null;
            S_GameManager.Instance.ReduceActionPointBy1();
        }
    }
}
