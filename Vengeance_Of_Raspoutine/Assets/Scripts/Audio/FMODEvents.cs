using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{

    [field: Header("Music")]
    [field: SerializeField] public EventReference Music { get; private set; }

    [field: Header("Global")]
    [field: SerializeField] public EventReference Claws { get; private set; }
    [field: SerializeField] public EventReference Impact { get; private set; }

    [field: Header("Environement")]
    [field: SerializeField] public EventReference Earthquake { get; private set; }
    [field: SerializeField] public EventReference Fogg { get; private set; }
    [field: SerializeField] public EventReference SnowStorm { get; private set; }

    [field: Header("Monsters")]
    [field: SerializeField] public EventReference DarkMagic { get; private set; }
    [field: SerializeField] public EventReference MonsterNoise { get; private set; }
    [field: SerializeField] public EventReference MonsterWarHorn { get; private set; }

    [field: Header("UI")]
    [field: SerializeField] public EventReference ClickUI { get; private set; }
    [field: SerializeField] public EventReference UnitDeployed { get; private set; }
    [field: SerializeField] public EventReference UnitSelected { get; private set; }

    [field: Header("Ussr")]
    [field: SerializeField] public EventReference Bear { get; private set; }
    [field: SerializeField] public EventReference CarRun { get; private set; }
    [field: SerializeField] public EventReference CarStart { get; private set; }
    [field: SerializeField] public EventReference RifleReload { get; private set; }
    [field: SerializeField] public EventReference RifleShoot { get; private set; }
    [field: SerializeField] public EventReference UssrWarHorn { get; private set; }
    [field: SerializeField] public EventReference VodkaSprinklers { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }
}
