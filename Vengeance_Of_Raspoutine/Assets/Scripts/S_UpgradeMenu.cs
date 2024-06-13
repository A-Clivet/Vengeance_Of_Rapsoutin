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
    [SerializeField] private int _upgradeLimit;
    [SerializeField] private Button _attBuffButton;
    [SerializeField] private Button _defBuffButton;
    private List<GameObject> _units = new List<GameObject>();
    [SerializeField] private SpriteRenderer _UnitDisplay;
    private int _unitInd = 0;
    private int _attBuffIncrement = 0;
    private int _defBuffIncrement = 0;

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
        if (_attBuffIncrement < _upgradeLimit)
        {
            if (_playerNumber == 1)
            {
                if (_gameManager.player1CharacterMoney.Buy(5))
                {
                    _unitManager.AttackBuff(_units[_unitInd]);
                    MoneyDisplayUpdate();
                    _attBuffIncrement++;
                }
            }
            else if (_playerNumber == 2)
            {
                if (_gameManager.player2CharacterMoney.Buy(5))
                {
                    _unitManager.AttackBuff(_units[_unitInd]);
                    MoneyDisplayUpdate();
                    _attBuffIncrement++;
                }
            }
        }
        else
        {
            _attBuffButton.interactable = false;
        }
    }

    public void DefenseBuffUpgrade()
    {
        if (_defBuffIncrement < _upgradeLimit)
        {
            if (_playerNumber == 1)
            {
                if (_gameManager.player1CharacterMoney.Buy(5))
                {
                    _unitManager.DefenseBuff(_units[_unitInd]);
                    MoneyDisplayUpdate();
                    _defBuffIncrement++;
                }
            }
            else if (_playerNumber == 2)
            {
                if (_gameManager.player2CharacterMoney.Buy(5))
                {
                    _unitManager.DefenseBuff(_units[_unitInd]);
                    MoneyDisplayUpdate();
                    _defBuffIncrement++;
                }
            }
        }
        else
        {
            _defBuffButton.interactable = false;
        }
    }

    public void UnitIndAdd()
    {
        if (_unitInd < _units.Count - 1)
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
        switch(_playerNumber)
        {
            case 1:
                for (int i=0; i < _gameManager.player1UnitCall.GetUnits().Count; i++)
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
