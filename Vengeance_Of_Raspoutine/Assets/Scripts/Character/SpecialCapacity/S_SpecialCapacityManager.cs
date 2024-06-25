using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_SpecialCapacityManager : MonoBehaviour
{
    #region Variables
    public static S_SpecialCapacityManager Instance;

    [Header("Banner references :")]
    [SerializeField] Image _specialCapacityBannerSpecialCapacityImage;
    [SerializeField] TextMeshProUGUI _specialCapacityBannerSpecialCapacityName;

    [Header("Grid references :")]
    [SerializeField] S_GridManager _player1GridManager;
    [SerializeField] S_GridManager _player2GridManager;

    [Header("Banner stats :")]
    [SerializeField] int _specialCapacityBannerTimeShowedValue = 2;

    List<Unit> _allPlayer1Units { get { return _player1GridManager.unitList; } }
    List<Unit> _allPlayer2Units { get { return _player2GridManager.unitList; } }

    S_GameManager _gameManager;

    S_RemoveUnit _removeUnitClass;
    GameObject _unitsParentGameObject;
    S_GridManagersHandler _gridManagersHandler;
    #endregion

    #region Methods
    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
    }

    private void Start()
    {
        _gameManager = S_GameManager.Instance;
        _removeUnitClass = S_RemoveUnit.Instance;
        _unitsParentGameObject = S_UnitCallButtonHandler.Instance.unitsParentGameObject;
        _gridManagersHandler = S_GridManagersHandler.Instance;
    }

    /// <summary> Launches the special capacity based on the provided special capacity stats for the specified player. </summary>
    /// <param name = "p_specialCapacityStats"> The stats of the special capacity to be launched. </param>
    /// <param name = "p_isPlayer1"> Indicates whether the capacity is for player 1 (true) or player 2 (false). </param>
    public IEnumerator LaunchSpecialCapacity(S_SpecialCapacityStats p_specialCapacityStats, bool p_isPlayer1)
    {
        #region Invoking special capacity's banner

        // Change banner's image and text
        _specialCapacityBannerSpecialCapacityName.text = p_specialCapacityStats.capacityName;
        _specialCapacityBannerSpecialCapacityImage.sprite = p_specialCapacityStats.capacitySprite;

        // Show the banner
        GameObject specialCapacityBanner = _specialCapacityBannerSpecialCapacityName.transform.parent.parent.gameObject;

        specialCapacityBanner.SetActive(true);

        // Wait than the player read before hidding the banner
        yield return new WaitForSeconds(_specialCapacityBannerTimeShowedValue);

        specialCapacityBanner.SetActive(false);
        #endregion

        #region Launching special capacity

        // NOTE : We can't use a switch there because we have to look in multiple boolean variables, so we use a else if block;
        if (p_specialCapacityStats.isUnitsStatChangementSpecialCapacity)
        {
            ChangeAllFactionUnitStats(p_specialCapacityStats.isPlayer1UnitsChanged, p_specialCapacityStats.unitStatChanged, p_specialCapacityStats.unitStatsChangementValue);
        }
        else if (p_specialCapacityStats.isUnitsMetamorphosisSpecialCapacity)
        {
            UnitsMetamorphosis(p_isPlayer1, p_specialCapacityStats.initialUnitState, p_specialCapacityStats.newUnitState);
        }
        else if (p_specialCapacityStats.isUnitsDestroySpecialCapacity)
        {
            RandomUnitDestroyer(p_isPlayer1, p_specialCapacityStats.numberOfUnitsToDestroy, p_specialCapacityStats.unitDestructionAnimationsFeedback);
        }
        else if (p_specialCapacityStats.isUltimateSpecialCapacity)
        {
            UltimateCapacity(p_isPlayer1, p_specialCapacityStats.ultimateCapacityProjectilePrefab, p_specialCapacityStats.targetUnitState, p_specialCapacityStats.damageCap);
        }
        else
        {
            Debug.LogError("ERROR ! No capacity have been selected in the [" + p_specialCapacityStats + "] Scriptable Object.");
        }       
        #endregion
    }

    void ChangeAllFactionUnitStats(bool p_isPlayer1Units, S_SpecialCapacityStats.UnitStatsEnum p_unitStatToChange, int p_value)
    {
        // To avoid having to manage two Unit list variables
        // we create a local variable nammed "allPlayerUnits" it contain the Unit list we will use later,
        // this variable will change depending if the player given is the first or the second. 
        List<Unit> allPlayerUnits = _allPlayer1Units;

        if (!p_isPlayer1Units)
            allPlayerUnits = _allPlayer2Units;

        // If there is more than zero units in the list we iterate throught all units and change the given stat value by the given value.
        if (allPlayerUnits.Count > 0)
        {
            foreach (Unit unit in allPlayerUnits)
            {
                switch (p_unitStatToChange)
                {
                    case S_SpecialCapacityStats.UnitStatsEnum.HP:
                        unit.defense += p_value;

                        // Security
                        unit.defense = (int)HandleStatChangement(unit.defense, 0);
                        break;

                    case S_SpecialCapacityStats.UnitStatsEnum.Attack:
                        unit.attack += p_value;

                        // Security
                        unit.attack = (int)HandleStatChangement(unit.attack, 0);
                        break;

                    case S_SpecialCapacityStats.UnitStatsEnum.TurnCharge:
                        unit.turnCharge += p_value;

                        // Security
                        unit.turnCharge = (int)HandleStatChangement(unit.turnCharge, 1);
                        break;

                    default:
                        Debug.LogError("The unit stats to change type given [" + p_unitStatToChange.ToString() + "] is not planned in the switch.");
                        break;
                }
            }
        }
    }
    
    float HandleStatChangement(float p_statValue, float p_minimalValue)
    {
        if (p_statValue < p_minimalValue)
            p_statValue = p_minimalValue;

        return p_statValue;
    }

    /// <summary> Transform all allied unit in p_baseStats, in p_newState </summary>
    void UnitsMetamorphosis(bool p_isPlayer1Units, int p_baseState, int p_newState)
    {
        List<Unit> allPlayerUnits = _allPlayer1Units;

        if (!p_isPlayer1Units)
            allPlayerUnits = _allPlayer2Units;

        // If there is more than zero units in the list we iterate throught all units and change the given stat value by the given value.
        if (allPlayerUnits.Count > 0)
        {
            foreach (Unit unit in allPlayerUnits)
            {
                if (unit.state == p_baseState)
                {
                    unit.state = p_newState;
                    unit.attack = unit.defense;
                    unit.turnCharge = 1;
                    unit.actualFormation.Clear();
                    unit.actualFormation.Add(unit);
                    unit.grid.unitManager.UnitColumn.Add(unit.actualFormation);
                }
            }
        }
    }

    void RandomUnitDestroyer(bool p_isPlayer1Units, int p_numberOfUnitsToDestroy, S_UnitDestructionAnimationManager.UnitDestructionAnimationsEnum p_unitDestructionAnimation)
    {
        int unitsThatWillBeDestroyedNumber = 0;

        // We save all the units we will destroy (they will be destroyed when unitsThatWillBeDestroyedNumber = p_numberOfUnitsToDestroy)
        List<Unit> allPlayerUnitsThatWillBeDestroyed = new();

        List<Unit> allPlayerUnits = _allPlayer2Units;

        if (!p_isPlayer1Units)
            allPlayerUnits = _allPlayer1Units;

        if (allPlayerUnits.Count > p_numberOfUnitsToDestroy)
        {
            foreach (Unit unit in allPlayerUnits)
            {
                // Heads or tails to determine whether or not we destroy the unit.
                if (Random.Range(0, 2) == 1)
                {
                    allPlayerUnitsThatWillBeDestroyed.Add(unit);
                    
                    unitsThatWillBeDestroyedNumber++;
                }

                // If we've destroyed enough units we stop the function.
                if (unitsThatWillBeDestroyedNumber >= p_numberOfUnitsToDestroy)
                    break;
            }

            // If we haven't destroyed enough units and we've finished the ennemy grid, we repeat the process.
            if (unitsThatWillBeDestroyedNumber < p_numberOfUnitsToDestroy)
            {
                RandomUnitDestroyer(p_isPlayer1Units, p_numberOfUnitsToDestroy - unitsThatWillBeDestroyedNumber, p_unitDestructionAnimation);
            }

            // Destruction of all the save units in allPlayerUnitsThatWillBeDestroyed
            for (int i = 0; i < allPlayerUnitsThatWillBeDestroyed.Count; i++)
            {
                _removeUnitClass.RemoveUnitOnSpecificTile(allPlayerUnitsThatWillBeDestroyed[i].actualTile[0], p_unitDestructionAnimation);
            }
        }
    }

    void UltimateCapacity(bool p_isPlayer1Units, GameObject p_ultimateCapacityProjectilePrefab, int p_unitsToChange, int p_damageCap)
    {
        int damageIncrement = 0;

        // We save all the units we will destroy
        List<Unit> allPlayerUnitsThatWillBeDestroyed = new();

        List<Unit> allPlayerUnits = _allPlayer1Units;
        S_GridManager playerGridManager = _gridManagersHandler.player1GridManager;

        if (!p_isPlayer1Units)
        {
            allPlayerUnits = _allPlayer2Units;
            playerGridManager = _gridManagersHandler.player2GridManager;
        }

        if (allPlayerUnits.Count > 0)
        {
            foreach (Unit unit in allPlayerUnits)
            {
                // We combine all the idle units while capping the damage increment.
                if (unit.state == p_unitsToChange)
                {
                    damageIncrement += unit.attack;

                    if (damageIncrement > p_damageCap)
                        damageIncrement = p_damageCap;

                    allPlayerUnitsThatWillBeDestroyed.Add(unit);
                }
            }

            // Removing all saved units
            for (int i = 0; i < allPlayerUnitsThatWillBeDestroyed.Count; i++)
            {
                _removeUnitClass.RemoveUnitOnSpecificTile(allPlayerUnitsThatWillBeDestroyed[i].actualTile[0], S_UnitDestructionAnimationManager.UnitDestructionAnimationsEnum.Explosion);
            }

            // We spawn the new projectile.
            GameObject energyBall = Instantiate(p_ultimateCapacityProjectilePrefab, _unitsParentGameObject.transform);

            Unit energyBallUnitComponent = energyBall.GetComponent<Unit>();

            energyBallUnitComponent.OnSpawn(playerGridManager.gridList[0][Mathf.Abs(playerGridManager.height) - 1]);

            energyBall.transform.position = new Vector3(
                // X coordonate
                energyBallUnitComponent.actualTile[0].transform.position.x,

                // Y coordonate
                energyBallUnitComponent.grid.startY +
                energyBallUnitComponent.grid.height +
                energyBallUnitComponent.actualTile[0].transform.position.y
            );

            playerGridManager.totalUnitAmount++;

            if (playerGridManager.isGridVisible)
            {
                energyBallUnitComponent.statsCanvas.SetActive(true);
            }

            // Giving the energyBallGameObject his attack stat
            energyBallUnitComponent.attack = damageIncrement;

            // Passing the unit in attack state
            energyBallUnitComponent.state = 2;
            energyBallUnitComponent.actualFormation.Clear();
            energyBallUnitComponent.actualFormation.Add(energyBallUnitComponent);
            energyBallUnitComponent.grid.unitManager.UnitColumn.Add(energyBallUnitComponent.actualFormation);

            // Make the energyBallGameObject selected, and give 1 action point because placing a unit cost 1 action point
            _gameManager.IncreaseActionPointBy1();
            energyBallUnitComponent.SelectUnit();
        }
    }

    #endregion
}