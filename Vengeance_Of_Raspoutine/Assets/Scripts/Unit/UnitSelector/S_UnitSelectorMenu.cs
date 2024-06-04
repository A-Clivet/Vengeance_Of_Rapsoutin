using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class S_UnitSelectorMenu : MonoBehaviour
{
    public static S_UnitSelectorMenu instance;

    [SerializeField] private GameObject UssrHighlight;
    [SerializeField] private GameObject RaspHighlight;
    [SerializeField] private List<GameObject> UssrDisplay = new List<GameObject>();
    [SerializeField] private List<GameObject> RaspDisplay = new List<GameObject>();

    [SerializeField] private List<Image> UssrImageList = new List<Image>() { null, null, null };
    [SerializeField] private List<Sprite> UssrSprites = new List<Sprite>() { null, null, null };
    
    [SerializeField] private List<Image> RaspImageList = new List<Image>() { null, null, null };
    [SerializeField] private List<Sprite> RaspSprites = new List<Sprite>() { null, null, null };

    [SerializeField] private List<GameObject> Units = new List<GameObject>();
    public List<GameObject> UssrSelectedUnits = new List<GameObject>() { null, null, null };
    public List<GameObject> RasputinSelectedUnits = new List<GameObject>() { null, null, null };



    private int UssrPosInList;
    private int RasputinPosInList;

    private int IntSelectedUnit;
    private int UnitListIndex = 0;


    private void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;
        UnitListIndex = Units.Count;
        
    }

    public void SelectedUnit(int P_index)
    {
        IntSelectedUnit = P_index;

        if (IntSelectedUnit > -1 && IntSelectedUnit <= UnitListIndex / 2 - 1)
        {
            //between 0 and 2 are Ussr values
            UssrSelectedUnits[UssrPosInList] = Units[IntSelectedUnit];
            UssrImageList[UssrPosInList].sprite = UssrSprites[IntSelectedUnit];
            UssrHighlight.transform.position = UssrDisplay[UssrPosInList].transform.position;

            UssrPosInList++;
            if (UssrPosInList >= 3)
            {
                UssrPosInList = 0;
            }
        }
        else if (IntSelectedUnit > UnitListIndex / 2 - 1)
        {
            //between 3 and 5 are Rasputin values
            RasputinSelectedUnits[RasputinPosInList] = Units[IntSelectedUnit];
            RaspImageList[RasputinPosInList].sprite = RaspSprites[IntSelectedUnit - 3];
            RaspHighlight.transform.position = RaspDisplay[RasputinPosInList].transform.position;

            RasputinPosInList++;
            if (RasputinPosInList >= 3)
            {
                RasputinPosInList = 0;
            }
        }
        else
        {
            return;
        }
    }
}
