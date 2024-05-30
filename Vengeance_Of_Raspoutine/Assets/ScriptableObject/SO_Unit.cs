using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Unity_Data", menuName = "ScriptableObjects/UnityData", order = 1)]
public class SO_Unit : ScriptableObject
{
    public string UnitName;
    /* unit stats */
    public int attack;
    public int defense;

    /*grid variable*/
    public int sizeX;
    public int sizeY;

    /*unit display and match variable*/
    public Image unitSprite;
    public int unitType;
    public int unitColor;
    public int unitTurnCharge;
}