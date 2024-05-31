using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_UpgradeMenu : MonoBehaviour
{
    [SerializeField] private S_UnitManager _unitManager;
    [SerializeField] private TextMeshProUGUI _moneyDisplay;
    [SerializeField] private int _upgradeLimit;
    [SerializeField] private Button _attBuffButton;
    [SerializeField] private Button _defBuffButton;
    [SerializeField] private List<Unit> _units;
    [SerializeField] private SpriteRenderer _UnitDisplay;
    private int _unitInd = 0;
    private int _attBuffIncrement = 0;
    private int _defBuffIncrement = 0;

    private void Start()
    {
        MoneyDisplayUpdate();
        UnitDisplayUpdate();
    }
    public void AttackBuffUpgrade()
    {
        if (_attBuffIncrement < _upgradeLimit)
        {
            if (S_GameManager.Instance.player1CharacterMoney.Buy(5))
            {
                _unitManager.AttackBuff(_units[_unitInd]);
                MoneyDisplayUpdate();
                _attBuffIncrement++;
            }
        }
        else
        {
            _attBuffButton.enabled = false;
        }
    }

    public void DefenseBuffUpgrade()
    {
        if (_defBuffIncrement < _upgradeLimit)
        {
            if (S_GameManager.Instance.player1CharacterMoney.Buy(5))
            {
                _unitManager.DefenseBuff(_units[_unitInd]);
                MoneyDisplayUpdate();
                _defBuffIncrement++;
            }
        }
        else
        {
            _defBuffButton.enabled = false;
        }
    }

    public void UnitIndAdd()
    {
        if (_unitInd < _units.Count)
        {
            _unitInd++;
            UnitDisplayUpdate();
        }
    }

    public void UnitIndSub()
    {
        if (_unitInd > 0)
        {
            _unitInd--;
            UnitDisplayUpdate();
        }
    }

    private void MoneyDisplayUpdate()
    {
        _moneyDisplay.text = "Money left : " + S_GameManager.Instance.player1CharacterMoney.ammount + " g";
    }

    private void UnitDisplayUpdate()
    {
        _UnitDisplay.sprite = _units[_unitInd].unitSprite;
    }
}
