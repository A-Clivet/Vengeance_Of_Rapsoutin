using UnityEngine;

[CreateAssetMenu(fileName = "CapacityStats", menuName = "ScriptableObject/CapacityStats")]
public class S_SpecialCapacityStats : ScriptableObject
{
    public enum UnitStatsEnum
    {
        HP,
        Attack,
        TurnCharge
    }

    [Header("Special capacities :")]
    public bool isUnitsStatChangementSpecialCapacity;
    public bool isUnitsMetamorphosisSpecialCapacity;
    public bool isUnitsDestroySpecialCapacity;
    public bool isUltimateSpecialCapacity;

    [Header("Basic statistics :")]
    public string capacityName;
    public Sprite capacitySprite;
    public string capacityDescription;
    public string capacityEffectDescription;
    public bool isCapacityLocked = true;

    [Header("Unit stats changement capacity's statistics :")]
    [ShowCondition("isUnitsStatChangementSpecialCapacity")]
        public bool isPlayer1UnitsChanged;
    [ShowCondition("isUnitsStatChangementSpecialCapacity")]
        public UnitStatsEnum unitStatChanged;
    [ShowCondition("isUnitsStatChangementSpecialCapacity")]
        public int unitStatsChangementValue;

    [Header("Unit types changements variables :")]
    [ShowCondition("isUnitsMetamorphosisSpecialCapacity")]
    public int baseState;
    [ShowCondition("isUnitsMetamorphosisSpecialCapacity")]
    public int newState;

    [Header("How many units to destroy :")]
    [ShowCondition("isUnitsDestroySpecialCapacity")]
    public int unitsToDestroy;

    [Header("Which unit type to turn into a Ball Of Mega Death")]
    [ShowCondition("isUltimateSpecialCapacity")]
    public int affectedState;
    [ShowCondition("isUltimateSpecialCapacity")]
    public int dmgLimit;
    [ShowCondition("isUltimateSpecialCapacity")]
    public GameObject EnergyBall;

/*#if UNITY_EDITOR

    private void OnValidate()
    {
        // Bad code - TODO : Change it
        if (isUnitsStatChangementSpecialCapacity == true)
        {
            isUnitsMetamorphosisSpecialCapacity = false;
            isUnitsDestroySpecialCapacity = false;
        }
        else if (isUnitsMetamorphosisSpecialCapacity == true)
        {
            isUnitsStatChangementSpecialCapacity = false;
            isUnitsDestroySpecialCapacity = false;
        }
        else if (isUnitsDestroySpecialCapacity == true)
        {
            isUnitsMetamorphosisSpecialCapacity = false;
            isUnitsStatChangementSpecialCapacity = false;
        }
    }

#endif*/
}