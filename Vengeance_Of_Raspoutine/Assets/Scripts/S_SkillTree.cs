using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_SkillTree : MonoBehaviour
{
    [SerializeField] private S_CharacterStats _characterStats;
    [SerializeField] private List<S_SpecialCapacityStats> _specialCapacities;
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private int _threshold;
    private int _baseThreshold;

    private void Start()
    {
        _baseThreshold = _threshold;
        ResetSkillTree();
    }

    //Default Ability.
    public void BaseAbility()
    {
        _characterStats.specialCapacity = _specialCapacities[0];
    }

    //Ability to reduce the turn charges of your troupes.
    public void AbilityTurnChargeMinus()
    {
        if (_specialCapacities[1].isCapacityLocked)
        {
            if (S_GameManager.Instance.player1CharacterXP.ammount >= _threshold)
            {
                _specialCapacities[1].isCapacityLocked = false;
                _characterStats.specialCapacity = _specialCapacities[1];
                _buttons[2].interactable = false;
                _buttons[3].interactable = false;
                _threshold *= 2;
                _buttons[4].interactable = true;
            }
        }
        else
        {
            _characterStats.specialCapacity = _specialCapacities[1];
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
                _characterStats.specialCapacity = _specialCapacities[2];
                _buttons[1].interactable = false;
                _buttons[3].interactable = false;
                _threshold *= 2;
                _buttons[5].interactable = true;
            }
        }
        else
        {
            _characterStats.specialCapacity = _specialCapacities[2];
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
                _characterStats.specialCapacity = _specialCapacities[3];
                _buttons[1].interactable = false;
                _buttons[2].interactable = false;
                _threshold *= 2;
                _buttons[5].interactable = true;
            }
        }
        else
        {
            _characterStats.specialCapacity = _specialCapacities[3];
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
                _characterStats.specialCapacity = _specialCapacities[4];
                _threshold *= 2;
                _buttons[6].interactable = true;
            }
        }
        else
        {
            _characterStats.specialCapacity = _specialCapacities[4];
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
                _characterStats.specialCapacity = _specialCapacities[5];
                _threshold *= 2;
                _buttons[6].interactable = true;
            }
        }
        else
        {
            _characterStats.specialCapacity = _specialCapacities[5];
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
                _characterStats.specialCapacity = _specialCapacities[6];
            }
        }
        else
        {
            _characterStats.specialCapacity = _specialCapacities[6];
        }
    }

    //To reset the tree.
    public void ResetSkillTree()
    {
        S_GameManager.Instance.player1CharacterXP.ResetAmmount();
        _threshold = _baseThreshold;
        _characterStats.specialCapacity = _specialCapacities[0];
        for (int i = 1; i < _specialCapacities.Count; i++)
        {
            _specialCapacities[i].isCapacityLocked = true;
        }
        for (int i = 4; i < _buttons.Count; i++)
        {
            _buttons[i].interactable = false;
        }
        for (int i = 1; i < 3; i++)
        {
            _buttons[i].interactable = true;
        }
    }
}
