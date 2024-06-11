using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_RemoveUnit : MonoBehaviour
{
    public static S_RemoveUnit Instance;
    public int NbCombo;
    public bool removing;

    [HideInInspector] public Unit hoveringUnit;

    S_GameManager _gameManager;
    S_GridManagersHandler _gridManagersHandler;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
    }

    private void Start()
    {
        _gameManager = S_GameManager.Instance;
        _gridManagersHandler = S_GridManagersHandler.Instance;
    }

    public void RemoveUnit(InputAction.CallbackContext p_context)
    {
        if (p_context.started)
        {
            // Security, if the player doesn't click on a unit we stop the function
            if (hoveringUnit == null)
                return;

            // If the removing of the unit succeded
            if (RemoveUnitOnSpecificTile(hoveringUnit.actualTile))
            {
                //Used for verifying if the action of removing the unit has created a combo
                NbCombo = hoveringUnit.grid.unitManager.UnitColumn.Count;
                removing = true;
                
                // Reduces the number of Action Points of the player playing by one
                _gameManager.ReduceActionPointBy1();
            }
        }
    }

    /// <summary> Function used to remove a unit without having to give a player's input </summary>
    /// <param name = "p_tile"> Tile where is the unit you want to destroy </param>
    /// <returns> Return if the function succeded </returns>
    public bool RemoveUnitOnSpecificTile(S_Tile p_tile)
    {
        // Security
        if (p_tile == null)
            return false;

        // Storing the unit inside p_tile into a variable
        Unit _unit = p_tile.unit;

        // Check if there is a unit under the mouse position, and if the unit is in state 0, or 1 (idle, or wall)
        if (_unit != null && (_unit.state == 0 || _unit.state == 1))
        {
            HandleUnitDestruction(_unit);

            return true;
        }

        return false;
    }

    public void HandleUnitDestruction(Unit p_unit)
    {
        #region Removing of the futur destroyed unit reference in the unitList variable, AllUnitPerColumn variable, and in his tile

        // We remove the unit from the unitList
        p_unit.grid.unitList.Remove(p_unit);

        // We remove the unit from the AllUnitPerColumn unit's column
        p_unit.grid.AllUnitPerColumn[p_unit.tileX].Remove(p_unit);

        // We set the unit's grid tile's local variable "unit" (store what unit is on the tile) to nothing
        p_unit.actualTile.unit = null;
        #endregion

        #region Updating of the other units position on the same column than the futur destroyed unit

        // Loop throw every tiles on the same column than unit's tile (from Y 0 to Y max)
        foreach (S_Tile tile in p_unit.grid.gridList[p_unit.tileX])
        {
            // If there is a unit on the tile, we handle other units positions to avoid gaps in the column
            if (tile.unit != null)
            {
                tile.unit.MoveToTile(p_unit.actualTile);
            }
        }
        #endregion
        
        #region Destruction of the unit

        // Now we finished using hoveringUnit variable we can destroy the unit
        Destroy(p_unit.gameObject);
        #endregion
    }

    /// <summary> Remove all units in the two player's grid | WARNING WORKS ONLY WITH NORMAL UNIT, NOT ELITE ONE </summary>
    public void RemoveAllUnits()
    {
        // We create a copy of the unitLists
        List<Unit> _player1UnitList = new(_gridManagersHandler.player1GridManager.unitList) ;
        List<Unit> _player2UnitList = new(_gridManagersHandler.player2GridManager.unitList);

        foreach (Unit _unit in _player1UnitList)
        {
            HandleUnitDestruction(_unit);
        }

        foreach (Unit _unit in _player2UnitList)
        {
            HandleUnitDestruction(_unit);
        }
    }
}