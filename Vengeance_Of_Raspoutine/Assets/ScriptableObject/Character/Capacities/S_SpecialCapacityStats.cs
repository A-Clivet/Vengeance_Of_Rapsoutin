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

    [HideInInspector] public bool isUnitsStatChangementSpecialCapacityHidden;
    [HideInInspector] public bool isUnitsMetamorphosisSpecialCapacityHidden;
    [HideInInspector] public bool isUnitsDestroySpecialCapacityHidden;
    [HideInInspector] public bool isUltimateSpecialCapacityHidden;

    [Header("Special capacities :")]
    [HideCondition("isUnitsStatChangementSpecialCapacityHidden")]
        public bool isUnitsStatChangementSpecialCapacity;
    [HideCondition("isUnitsMetamorphosisSpecialCapacityHidden")]
        public bool isUnitsMetamorphosisSpecialCapacity;
    [HideCondition("isUnitsDestroySpecialCapacityHidden")]
        public bool isUnitsDestroySpecialCapacity;
    [HideCondition("isUltimateSpecialCapacityHidden")]
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

    [Header("Unit types changements capacity's statistics :")]
    [ShowCondition("isUnitsMetamorphosisSpecialCapacity")]
        public int initialUnitState;
    [ShowCondition("isUnitsMetamorphosisSpecialCapacity")]
        public int newUnitState;

    [Header("How many units to destroy :")]
    [ShowCondition("isUnitsDestroySpecialCapacity")]
        public int numberOfUnitsToDestroy;

    [Header("Which unit type to turn into a Ball of Mega Death")]
    [Tooltip("Which unit state will be turn into the ultimate projectile")]
    [ShowCondition("isUltimateSpecialCapacity")]
        public int targetUnitState;
    [ShowCondition("isUltimateSpecialCapacity")]
        public int damageCap;
    [ShowCondition("isUltimateSpecialCapacity")]
        public GameObject energyBallGameObject;

#region Show and hide variables

#if UNITY_EDITOR

    private void OnValidate()
    {
        // NOTE : This code don't works and the code after the OnValidate, it was created to avoid the following code 

        /*// We create a dictionary that store for each special capacity type the function SetSpecialCapacityState() with specific argument in it
        Dictionary<bool, System.Action> specialCapacityTypes = new()
        {
            { isUnitsStatChangementSpecialCapacity, () => SetSpecialCapacityState(ref isUnitsStatChangementSpecialCapacityHidden, true, false, false, false) },
            { isUnitsMetamorphosisSpecialCapacity, () => SetSpecialCapacityState(ref isUnitsMetamorphosisSpecialCapacityHidden, false, true, false, false) },
            { isUnitsDestroySpecialCapacity, () => SetSpecialCapacityState(ref isUnitsDestroySpecialCapacityHidden, false, false, true, false) },
            { isUltimateSpecialCapacity, () => SetSpecialCapacityState(ref isUltimateSpecialCapacityHidden, false, false, false, true) }
        };

        // We loop through special capacity types dictionary's keys until we find one there is one key to bool
        foreach (KeyValuePair<bool, System.Action> specialCapacityType in specialCapacityTypes)
        {
            if (specialCapacityType.Key)
            {
                specialCapacityType.Value();
                break;
            }
        }*/

        // Due to the lack of time (priorization) this code will surely remain bad (RIP)
        if (isUnitsStatChangementSpecialCapacity)
        {
            isUnitsStatChangementSpecialCapacityHidden = false;
            isUnitsMetamorphosisSpecialCapacityHidden = true;
            isUnitsDestroySpecialCapacityHidden = true;
            isUltimateSpecialCapacityHidden = true;

            isUnitsMetamorphosisSpecialCapacity = false;
            isUnitsDestroySpecialCapacity = false;
            isUltimateSpecialCapacity = false;
        }
        else if (isUnitsMetamorphosisSpecialCapacity)
        {
            isUnitsStatChangementSpecialCapacityHidden = true;
            isUnitsMetamorphosisSpecialCapacityHidden = false;
            isUnitsDestroySpecialCapacityHidden = true;
            isUltimateSpecialCapacityHidden = true;

            isUnitsStatChangementSpecialCapacity = false;
            isUnitsDestroySpecialCapacity = false;
            isUltimateSpecialCapacity = false;
        }
        else if (isUnitsDestroySpecialCapacity)
        {
            isUnitsStatChangementSpecialCapacityHidden = true;
            isUnitsMetamorphosisSpecialCapacityHidden = true;
            isUnitsDestroySpecialCapacityHidden = false;
            isUltimateSpecialCapacityHidden = true;

            isUnitsStatChangementSpecialCapacity = false;
            isUnitsMetamorphosisSpecialCapacity = false;
            isUltimateSpecialCapacity = false;
        }
        else if (isUltimateSpecialCapacity)
        {
            isUnitsStatChangementSpecialCapacityHidden = true;
            isUnitsMetamorphosisSpecialCapacityHidden = true;
            isUnitsDestroySpecialCapacityHidden = true;
            isUltimateSpecialCapacityHidden = false;

            isUnitsStatChangementSpecialCapacity = false;
            isUnitsMetamorphosisSpecialCapacity = false;
            isUnitsDestroySpecialCapacity = false;
        }
        else
        {
            isUnitsStatChangementSpecialCapacityHidden = false;
            isUnitsMetamorphosisSpecialCapacityHidden = false;
            isUnitsDestroySpecialCapacityHidden = false;
            isUltimateSpecialCapacityHidden = false;
        }
    }

    /*    private void SetSpecialCapacityState(ref bool targetHiddenState, bool statChangement, bool metamorphosis, bool destroy, bool ultimate)
        {
            isUnitsStatChangementSpecialCapacityHidden = statChangement;
            isUnitsMetamorphosisSpecialCapacityHidden = metamorphosis;
            isUnitsDestroySpecialCapacityHidden = destroy;
            isUltimateSpecialCapacityHidden = ultimate;

            isUnitsStatChangementSpecialCapacity = statChangement;
            isUnitsMetamorphosisSpecialCapacity = metamorphosis;
            isUnitsDestroySpecialCapacity = destroy;
            isUltimateSpecialCapacity = ultimate;

            targetHiddenState = false;
        }*/
#endif
}

#endregion