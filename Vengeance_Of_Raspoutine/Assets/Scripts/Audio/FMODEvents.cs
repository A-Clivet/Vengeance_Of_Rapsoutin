using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{

    [field: Header("Music")]
    [field: SerializeField] public EventReference MainMenuMusic { get; private set; }
    [field: SerializeField] public EventReference MainGameMusic { get; private set; }

    [field: Header("Global")]
    [field: SerializeField] public EventReference Claws { get; private set; } //
    [field: SerializeField] public EventReference Impact { get; private set; } //

    [field: Header("Environement")]
    [field: SerializeField] public EventReference Earthquake { get; private set; } //
    [field: SerializeField] public EventReference Fogg { get; private set; } //
    [field: SerializeField] public EventReference SnowStorm { get; private set; } //

    [field: Header("Monsters")]
    [field: SerializeField] public EventReference MonsterNoise { get; private set; } //
    [field: SerializeField] public EventReference MonsterWarHorn { get; private set; } //

    [field: Header("UI")]
    [field: SerializeField] public EventReference ClickUI { get; private set; } 
    [field: SerializeField] public EventReference UnitDeployed { get; private set; } 
    [field: SerializeField] public EventReference UnitSelected { get; private set; } 

    [field: Header("Ussr")]
    [field: SerializeField] public EventReference Bear { get; private set; } //m
    [field: SerializeField] public EventReference CarRun { get; private set; }//
    [field: SerializeField] public EventReference CarStart { get; private set; } //m
    [field: SerializeField] public EventReference RifleReload { get; private set; } //
    [field: SerializeField] public EventReference RifleShoot { get; private set; }//m
    [field: SerializeField] public EventReference UssrWarHorn { get; private set; } //

    [field: Header("skils")]
    [field: SerializeField] public EventReference VodkaSprinklers { get; private set; }
    [field: SerializeField] public EventReference DarkMagic { get; private set; }
    [field: SerializeField] public EventReference Anti_Material { get; private set; }
    [field: SerializeField] public EventReference BearTrap { get; private set; }
    [field: SerializeField] public EventReference Curse { get; private set; }
    [field: SerializeField] public EventReference Doping { get; private set; }
    [field: SerializeField] public EventReference Gas { get; private set; }
    [field: SerializeField] public EventReference Missile { get; private set; }
    [field: SerializeField] public EventReference Mortar { get; private set; }
    [field: SerializeField] public EventReference SlowDown { get; private set; }
    [field: SerializeField] public EventReference SpeedUp { get; private set; }



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
