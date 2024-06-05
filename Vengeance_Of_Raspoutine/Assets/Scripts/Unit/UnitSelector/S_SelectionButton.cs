using UnityEngine;

public class S_SelectionButton : MonoBehaviour
{
    [SerializeField] private int Value;
    public void ButtonPressed()
    {
        S_UnitSelectorMenu.instance.SelectedUnit(Value);
    }
}
