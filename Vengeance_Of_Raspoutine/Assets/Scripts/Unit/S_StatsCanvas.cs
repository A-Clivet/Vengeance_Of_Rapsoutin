using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_StatsCanvas : MonoBehaviour
{
    Unit _unit;

    GameObject _attackSpriteBackground;
    GameObject _defenseSpriteBackground;
    GameObject _turnChargeSpriteBackground;

    private void Awake()
    {
        _unit = gameObject.transform.parent.GetComponent<Unit>();

        _attackSpriteBackground = gameObject.transform.GetChild(0).gameObject;
        _defenseSpriteBackground = gameObject.transform.GetChild(1).gameObject;
        _turnChargeSpriteBackground = gameObject.transform.GetChild(2).gameObject;
    }

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
        _attackSpriteBackground.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _unit.attack.ToString();
        _defenseSpriteBackground.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _unit.defense.ToString();
        _turnChargeSpriteBackground.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _unit.turnCharge.ToString();

        if (_unit.state == 2)
        {
            _attackSpriteBackground.SetActive(true);
            _turnChargeSpriteBackground.SetActive(true);
            _defenseSpriteBackground.SetActive(false);
        }
        else
        {
            _attackSpriteBackground.SetActive(false);
            _turnChargeSpriteBackground.SetActive(false);
            _defenseSpriteBackground.SetActive(true);
        }
    }
}
