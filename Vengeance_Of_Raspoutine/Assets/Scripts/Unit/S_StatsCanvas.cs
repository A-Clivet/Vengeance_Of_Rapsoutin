using TMPro;
using UnityEngine;

public class S_StatsCanvas : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private GameObject _atkDisplay;
    [SerializeField] private GameObject _defDisplay;
    [SerializeField] private GameObject _turnChargeDisplay;

    private void OnEnable()
    {
        UpdateStatsDisplay();
    }

    private void Update()
    {
        UpdateStatsDisplay();
    }

    public void UpdateStatsDisplay()
    {
        _atkDisplay.GetComponent<TextMeshProUGUI>().text = _unit.attack.ToString();
        _defDisplay.GetComponent<TextMeshProUGUI>().text = _unit.defense.ToString();
        _turnChargeDisplay.GetComponent<TextMeshProUGUI>().text = _unit.turnCharge.ToString();
        if (_unit.state == 2)
        {
            _atkDisplay.SetActive(true);
            _turnChargeDisplay.SetActive(true);
            _defDisplay.SetActive(false);
        }
        else
        {
            _atkDisplay.SetActive(false);
            _turnChargeDisplay.SetActive(false);
            _defDisplay.SetActive(true);
        }
    }
}
