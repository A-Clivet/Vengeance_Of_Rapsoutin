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
    #endregion

    #region Methods
    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
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
            ChangeAllFactionUnitStats(p_isPlayer1, p_specialCapacityStats.unitStatChanged, p_specialCapacityStats.unitStatsChangementValue);
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
        {
            allPlayerUnits = _allPlayer2Units;
        }

        // If there is more than zero units in the list we iterate throught all units and change the given stat value by the given value.
        if (allPlayerUnits.Count > 0)
        {
            foreach (Unit unit in allPlayerUnits)
            {
                switch (p_unitStatToChange)
                {
                    case S_SpecialCapacityStats.UnitStatsEnum.HP:
                        unit.defense += p_value;
                        break;

                    case S_SpecialCapacityStats.UnitStatsEnum.Attack:
                        unit.attack += p_value;
                        break;

                    default:
                        Debug.LogError("The unit stats to change type given [" + p_unitStatToChange.ToString() + "] is not planned in the switch.");
                        break;
                }
            }
        }
    }

    #endregion
}