using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{

    [field: Header("Music")]
    [field: SerializeField] public EventReference MainMenuMusic { get; private set; }
    [field: SerializeField] public EventReference BattleMusic { get; private set; }
    [field: SerializeField] public EventReference BattleMusicAdap { get; private set; }

    [field: Header("SFX")]
    [field: SerializeField] public EventReference X_SFX { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}
