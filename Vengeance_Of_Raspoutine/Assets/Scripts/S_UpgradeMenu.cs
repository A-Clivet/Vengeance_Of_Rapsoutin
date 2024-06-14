using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class S_UpgradeMenu : MonoBehaviour
{
    [SerializeField] private int _playerNumber;
    [SerializeField] private S_UnitManager _unitManager;
    [SerializeField] private TextMeshProUGUI _moneyDisplay;
    [SerializeField] private Button _attBuffButton;
    [SerializeField] private Button _defBuffButton;
    private List<GameObject> _units = new List<GameObject>();
    [SerializeField] private SpriteRenderer _UnitDisplay;
    private int _unitInd = 0;

    S_GameManager _gameManager;

    private void Start()
    {
        _gameManager = S_GameManager.Instance;

        UnitListInit();
        MoneyDisplayUpdate();
        UnitDisplayUpdate();
    }
    public void AttackBuffUpgrade()
    {
        if (_playerNumber == 1)
        {
            if (_gameManager.player1CharacterMoney.Buy(5))
            {
                _unitManager.AttackBuff(_units[_unitInd]);
                MoneyDisplayUpdate();
            }
        }
        else if (_playerNumber == 2)
        {
            if (_gameManager.player2CharacterMoney.Buy(5))
            {
                _unitManager.AttackBuff(_units[_unitInd]);
                MoneyDisplayUpdate();
            }
        }
    }

    public void DefenseBuffUpgrade()
    {
        if (_playerNumber == 1)
        {
            if (_gameManager.player1CharacterMoney.Buy(5))
            {
                _unitManager.DefenseBuff(_units[_unitInd]);
                MoneyDisplayUpdate();
            }
        }
        else if (_playerNumber == 2)
        {
            if (_gameManager.player2CharacterMoney.Buy(5))
            {
                _unitManager.DefenseBuff(_units[_unitInd]);
                MoneyDisplayUpdate();
            }
        }
    }

    public void UnitIndAdd()
    {
        if (_unitInd < _units.Count - 1)
        {
            _unitInd++;
            UnitDisplayUpdate();
        }
        else
        {
            _unitInd = 0;
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
        else
        {
            _unitInd = _units.Count - 1;
            UnitDisplayUpdate();
        }
    }

    private void MoneyDisplayUpdate()
    {
        if (_playerNumber == 1)
        {
            _moneyDisplay.text = "Money left : " + _gameManager.player1CharacterMoney.ammount + " g";
        }
        else if (_playerNumber == 2)
        {
            _moneyDisplay.text = "Money left : " + _gameManager.player2CharacterMoney.ammount + " g";
        }
    }

    private void UnitDisplayUpdate()
    {
        _UnitDisplay.sprite = _units[_unitInd].GetComponent<SpriteRenderer>().sprite;
    }

    private void UnitListInit()
    {
        switch (_playerNumber)
        {
            case 1:
                for (int i = 0; i < _gameManager.player1UnitCall.GetUnits().Count; i++)
                {
                    _units.Add(_gameManager.player1UnitCall.GetUnits()[i]);
                }
                break;
            case 2:
                for (int i = 0; i < _gameManager.player2UnitCall.GetUnits().Count; i++)
                {
                    _units.Add(_gameManager.player2UnitCall.GetUnits()[i]);
                }
                break;
            default:
                break;
        }
    }
}
