using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class S_UnitSelectorMenu : MonoBehaviour
{
    public static S_UnitSelectorMenu instance;
    [SerializeField] private List<GameObject> Units = new List<GameObject>();
    private List<GameObject> UssrSelectedUnits = new List<GameObject>();
    private List<GameObject> RasputinSelectedUnits = new List<GameObject>();
    private int IntSelectedUnit;

    private void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;
    }

    private void SelectedUnit()
    {
        if (IntSelectedUnit > 0 && IntSelectedUnit <= 3)
        {
            UssrSelectedUnits = 
            IntSelectedUnit = 0;
        }
        else if (IntSelectedUnit > 3 )
        {
            RasputinSelectedUnits = 
            IntSelectedUnit = 0;
        }
        else
        {
            return;
        }
    }
    private void DeselectUnit()
    {

    }
    public void ButtonSelector(int P_index)
    {
        IntSelectedUnit = P_index;
    }


}
