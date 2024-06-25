using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_RemoveUnit : MonoBehaviour
{
    public static S_RemoveUnit Instance;
    public int NbCombo;
    public bool removing;

    [HideInInspector] public Unit hoveringUnit;

    [SerializeField] AnimatorController _explosionAnimatorController;
    [SerializeField] GameObject _unitAnimationDestructionPrefab;

    S_GameManager _gameManager;
    S_UnitDestructionAnimationManager _unitDestructionAnimationManager;
    S_GridManagersHandler _gridManagersHandler;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
    }

    private void Start()
    {
        _unitDestructionAnimationManager = S_UnitDestructionAnimationManager.Instance;
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

            if (DoesPlayerHasActionPoint())
            {
                // If the removing of the unit succeded
                if (RemoveUnitOnSpecificTile(hoveringUnit.actualTile, S_UnitDestructionAnimationManager.UnitDestructionAnimationsEnum.Pouf))
                {
                    //Used for verifying if the action of removing the unit has created a combo
                    NbCombo = hoveringUnit.grid.unitManager.UnitColumn.Count;
                    removing = true;

                    // Reduces the number of Action Points of the player playing by one
                    _gameManager.ReduceActionPointBy1();
                }
            }
        }
    }

    bool DoesPlayerHasActionPoint()
    {
        if (_gameManager.playerActionNumber > 0)
            return true;

        return false;
    }

    /// <summary> Function used to remove a unit without having to give a player's input </summary>
    /// <param name = "p_tile"> Tile where is the unit you want to destroy </param>
    /// <returns> Return if the function succeded </returns>
    public bool RemoveUnitOnSpecificTile(
        S_Tile p_tile,
        S_UnitDestructionAnimationManager.UnitDestructionAnimationsEnum p_unitDestructionAnimationsEnum = S_UnitDestructionAnimationManager.UnitDestructionAnimationsEnum.Pouf)
    {
        // Security
        if (p_tile == null)
            return false;

        // Storing the unit inside p_tile into a variable
        Unit _unit = p_tile.unit;

        // Security : Return false if there is not a unit under the mouse position, and if the unit is not in state 0, or 1 (idle, or wall)
        if (!(_unit != null && (_unit.state == 0 || _unit.state == 1)))
            return false;

        // Hidding unit (from the one that attacks)
        _unit.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        // Create and handle unit animation destruction
        StartCoroutine(_unitDestructionAnimationManager.HandleUnitAnimationDestruction(
            p_unitDestructionAnimationsEnum,
            _unit
        ));

        HandleUnitDestruction(_unit);

        _unit.actualTile.grid.unitManager.UnitCombo(3);

        return true;
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
        // We clear all the existing attack formation
        _gridManagersHandler.player1GridManager.unitManager.UnitColumn.Clear();
        _gridManagersHandler.player2GridManager.unitManager.UnitColumn.Clear();

        // We create a copy of the unitLists
        //List<Unit> _player1UnitList = new(_gridManagersHandler.player1GridManager.unitList);
        //List<Unit> _player2UnitList = new(_gridManagersHandler.player2GridManager.unitList);

        for(int i = _gridManagersHandler.player1GridManager.unitList.Count - 1; i >= 0; i--)
        {
            HandleUnitDestruction(_gridManagersHandler.player1GridManager.unitList[i]);
        }
        for (int i = _gridManagersHandler.player2GridManager.unitList.Count - 1; i >= 0; i--)
        {
            HandleUnitDestruction(_gridManagersHandler.player2GridManager.unitList[i]);
        }

        //foreach (Unit _unit in _player1UnitList)
        //{
        //    Debug.Log("will be destroyed");
        //    HandleUnitDestruction(_unit);
        //}

        //foreach (Unit _unit in _player2UnitList)
        //{
        //    Debug.Log("will be destroyed");
        //    HandleUnitDestruction(_unit);
        //}

    }
}