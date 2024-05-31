using UnityEngine;

[CreateAssetMenu(fileName = "CapacityStats", menuName = "ScriptableObject/CapacityStats")]
public class S_SpecialCapacityStats : ScriptableObject
{
    public enum UnitStatsEnum
    {
        HP,
        Attack
    }

    [Header("Special capacities :")]
    public bool isUnitsStatChangementSpecialCapacity;

    [Header("Basic statistics :")]
    public string capacityName;
    public Sprite capacitySprite;
    public string capacityDescription;
    public string capacityEffectDescription;

    [Header("Unit stats changement capacity's statistics :")]
    [ShowCondition("isUnitsStatChangementSpecialCapacity")]
        public UnitStatsEnum unitStatChanged;
    [ShowCondition("isUnitsStatChangementSpecialCapacity")]
        public int unitStatsChangementValue;
}