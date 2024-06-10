using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_SkillTree : MonoBehaviour
{
    [Header("Stats :")]
    [SerializeField] bool _isPlayer1SkillTree;

    [Header("References :")]
    [SerializeField] private S_CharacterStats _characterStats;
    [SerializeField] private List<S_SpecialCapacityStats> _specialCapacities;
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private int _threshold;
    [SerializeField] private TextMeshProUGUI _xpDisplay;
    [SerializeField] private TextMeshProUGUI _thresholdDisplay;

    private S_GameManager _gameManager;
    private S_CharacterAdrenaline _player1CharacterAdrenaline;
    private S_CharacterAdrenaline _player2CharacterAdrenaline;

    private int _baseThreshold;

    private void Start()
    {
        _gameManager = S_GameManager.Instance;

        _player1CharacterAdrenaline = _gameManager.player1CharacterAdrenaline;
        _player2CharacterAdrenaline = _gameManager.player2CharacterAdrenaline;

        _baseThreshold = _threshold;
        ResetSkillTree();
        UpdateXpDisplay();
        UpdateThresholdDisplay(false);
    }

    void ChangeAbility(int p__specialCapacitiesIndex)
    {
        if (_isPlayer1SkillTree)
        {
            _player1CharacterAdrenaline.specialCapacity = _specialCapacities[p__specialCapacitiesIndex];

            S_CharacterManager.Instance.player1CharacterGameObject.GetComponent<S_CharacterSpecialCapacity>().RecieveSpecialCapacityStats
                (_specialCapacities[p__specialCapacitiesIndex].capacityName, 
                _specialCapacities[p__specialCapacitiesIndex].capacityDescription, 
                _specialCapacities[p__specialCapacitiesIndex].capacityEffectDescription
            );
        }
            

        else
        {
            _player2CharacterAdrenaline.specialCapacity = _specialCapacities[p__specialCapacitiesIndex];

            S_CharacterManager.Instance.player2CharacterGameObject.GetComponent<S_CharacterSpecialCapacity>().RecieveSpecialCapacityStats
                (_specialCapacities[p__specialCapacitiesIndex].capacityName, 
                _specialCapacities[p__specialCapacitiesIndex].capacityDescription, 
                _specialCapacities[p__specialCapacitiesIndex].capacityEffectDescription
            );
        }
            
    }

    //Default Ability.
    public void BaseAbility()
    {
        ChangeAbility(0);
    }

    //Ability to reduce the turn charges of your troupes.
    public void AbilityTurnChargeMinus()
    {
        if (_specialCapacities[1].isCapacityLocked)
        {
            if (S_GameManager.Instance.player1CharacterXP.ammount >= _threshold)
            {
                _specialCapacities[1].isCapacityLocked = false;

                ChangeAbility(1);

                _buttons[2].interactable = false;
                _buttons[3].interactable = false;
                _threshold *= 2;
                _buttons[4].interactable = true;

                UpdateXpDisplay();
                UpdateThresholdDisplay(false);
            }
        }
        else
        {
            ChangeAbility(1);
        }
    }

    //Ability to increase the turn charges of your opponent.
    public void AbilityTurnChargePlus()
    {
        if (_specialCapacities[2].isCapacityLocked)
        {
            if (S_GameManager.Instance.player1CharacterXP.ammount >= _threshold)
            {
                _specialCapacities[2].isCapacityLocked = false;

                ChangeAbility(2);

                _buttons[1].interactable = false;
                _buttons[3].interactable = false;
                _threshold *= 2;
                _buttons[5].interactable = true;

                UpdateXpDisplay();
                UpdateThresholdDisplay(false);
            }
        }
        else
        {
            ChangeAbility(2);
        }
    }

    //Ability to randomly destroy ennemy troupes.
    public void AbilityRandomDestroyer()
    {
        if (_specialCapacities[3].isCapacityLocked)
        {
            if (S_GameManager.Instance.player1CharacterXP.ammount >= _threshold)
            {
                _specialCapacities[3].isCapacityLocked = false;

                ChangeAbility(3);

                _buttons[1].interactable = false;
                _buttons[2].interactable = false;
                _threshold *= 2;
                _buttons[5].interactable = true;

                UpdateXpDisplay();
                UpdateThresholdDisplay(false);
            }
        }
        else
        {
            ChangeAbility(3);
        }
    }

    //Ability to turn your walls into projectiles.
    public void AbilityWallProjectiles()
    {
        if (_specialCapacities[4].isCapacityLocked)
        {
            if (S_GameManager.Instance.player1CharacterXP.ammount >= _threshold)
            {
                _specialCapacities[4].isCapacityLocked = false;

                ChangeAbility(4);

                _threshold *= 2;
                _buttons[6].interactable = true;

                UpdateXpDisplay();
                UpdateThresholdDisplay(false);
            }
        }
        else
        {
            ChangeAbility(4);
        }
    }

    //Ability to debuff your opponent's troupes.
    public void OppsDebuff()
    {
        if (_specialCapacities[5].isCapacityLocked)
        {
            if (S_GameManager.Instance.player1CharacterXP.ammount >= _threshold)
            {
                _specialCapacities[5].isCapacityLocked = false;

                ChangeAbility(5);

                _threshold *= 2;
                _buttons[6].interactable = true;

                UpdateXpDisplay();
                UpdateThresholdDisplay(false);
            }
        }
        else
        {
            ChangeAbility(5);
        }
    }

    //Ability to tranform all your non-formation troupes into a big projectile.
    public void UltimateSkill()
    {
        if (_specialCapacities[6].isCapacityLocked)
        {
            if (S_GameManager.Instance.player1CharacterXP.ammount >= _threshold)
            {
                _specialCapacities[6].isCapacityLocked = false;
                ChangeAbility(6);

                UpdateXpDisplay();
                UpdateThresholdDisplay(true);
            }
        }
        else
        {
            ChangeAbility(6);
        }
    }

    //To reset the tree.
    public void ResetSkillTree()
    {
        S_GameManager.Instance.player1CharacterXP.ResetAmmount();
        _threshold = _baseThreshold;

        ChangeAbility(0);

        for (int i = 1; i < _specialCapacities.Count; i++)
        {
            _specialCapacities[i].isCapacityLocked = true;
        }
        for (int i = 4; i < _buttons.Count; i++)
        {
            _buttons[i].interactable = false;
        }
        for (int i = 1; i <= 3; i++)
        {
            _buttons[i].interactable = true;
        }

        UpdateXpDisplay();
        UpdateThresholdDisplay(false);
    }

    private void UpdateXpDisplay()
    {
        if (_isPlayer1SkillTree)
        {
            _xpDisplay.text = "xp = " + S_GameManager.Instance.player1CharacterXP.ammount.ToString();
        }
        else
        {
            _xpDisplay.text = "xp = " + S_GameManager.Instance.player2CharacterXP.ammount.ToString();
        }
    }

    private void UpdateThresholdDisplay(bool p_allSkillsUnlocked)
    {
        if (p_allSkillsUnlocked)
        {
            _thresholdDisplay.text = "Congratulations you unlocked everything !";
        }
        else
        {
            _thresholdDisplay.text = "Price = " + _threshold.ToString();
        }
    }
}
