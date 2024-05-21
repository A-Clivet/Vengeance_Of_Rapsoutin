using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public SO_Unit SO_Unit;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(SO_Unit.UnitName);
        Debug.Log(SO_Unit.attack);
        Debug.Log(SO_Unit.defense);
        Debug.Log(SO_Unit.sizeX);
        Debug.Log(SO_Unit.sizeY);
        Debug.Log(SO_Unit.unitType);
    }

    /* is called by the UnitManager, can be used to define what happens for a unit if units are kill by the enemy attack*/
    public void OnAttack(){
        switch (SO_Unit.unitType)
        {
            case 0:
                break;

            case 1:
                break;

            case 2:
                break;
        }
    }

    /* is called by the unit that killed it, can be used to check if units are kill by the enemy attack*/
    public void OnHit() {
        switch (SO_Unit.unitType)
        {
            case 0:
                break;

            case 1:
                break;

            case 2:
                break;
        }
    }
}
