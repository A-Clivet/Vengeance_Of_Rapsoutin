using TMPro;
using UnityEngine;

public class S_UpgradeMenu : MonoBehaviour
{
    [SerializeField] private S_UnitManager _unitManager;
    [SerializeField] private TextMeshProUGUI _moneyDisplay;
    [SerializeField] private int _upgradeLimit;
    private int _attBuffIncrement = 0;
    private int _defBuffIncrement = 0;

    private void Start()
    {
        DisplayUpdate();
    }
    public void AttackBuffUpgrade()
    {
        if (_attBuffIncrement <= _upgradeLimit)
        {
            if (S_GameManager.Instance.player1CharacterMoney.Buy(5))
            {
                _unitManager.AttackBuff();
                DisplayUpdate();
                _attBuffIncrement++;
            }
        }
    }

    public void DefenseBuffUpgrade()
    {
        if (_defBuffIncrement <= _upgradeLimit)
        {
            if (S_GameManager.Instance.player1CharacterMoney.Buy(5))
            {
                _unitManager.DefenseBuff();
                DisplayUpdate();
                _defBuffIncrement++;
            }
        }
    }

    private void DisplayUpdate()
    {
        _moneyDisplay.text = "Money left : " + S_GameManager.Instance.player1CharacterMoney.ammount + " g";
    }
}
