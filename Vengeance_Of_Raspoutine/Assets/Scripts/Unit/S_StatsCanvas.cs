using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_StatsCanvas : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private GameObject _atkSpriteBackground;
    [SerializeField] private GameObject _defSpriteBackground;
    [SerializeField] private GameObject _turnChargeSpriteBackground;

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
        _atkSpriteBackground.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _unit.attack.ToString();
        _defSpriteBackground.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _unit.defense.ToString();
        _turnChargeSpriteBackground.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _unit.turnCharge.ToString();

        if (_unit.state == 2)
        {
            _atkSpriteBackground.SetActive(true);
            _turnChargeSpriteBackground.SetActive(true);
            _defSpriteBackground.SetActive(false);
        }
        else
        {
            _atkSpriteBackground.SetActive(false);
            _turnChargeSpriteBackground.SetActive(false);
            _defSpriteBackground.SetActive(true);
        }
    }
}
