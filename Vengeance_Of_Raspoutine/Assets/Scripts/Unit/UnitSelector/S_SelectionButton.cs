using UnityEngine;

public class S_SelectionButton : MonoBehaviour
{
    [SerializeField] int _value;

    public void ButtonPressed()
    {
        S_UnitSelectorMenu.Instance.SelectedUnit(_value);
    }
}