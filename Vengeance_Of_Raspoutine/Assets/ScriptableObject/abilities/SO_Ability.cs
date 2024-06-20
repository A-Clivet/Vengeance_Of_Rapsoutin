using System.Collections.Generic;
using UnityEngine;

public class SO_Ability : ScriptableObject
{
    public enum AbilityType
    {
        Passive,
        Consumable
    }

    private int _unitBaseAttack = 0;
    [Header("Statistics :")]
    public AbilityType type;
    public int price;

    public void AttackBuffRelativeToTurnNumber()
    {
        int dmgBonus = Mathf.RoundToInt(S_GameManager.Instance.currentRoundNumber * 0.2f);
        if (S_GameManager.Instance.isPlayer1Turn)
        {
            if (S_GameManager.Instance.player1CharacterMoney.ammount >= price)
            {
                for (int i = 0; i < S_UnitSelectorMenu.Instance.player1SelectedUnits.Count; i++)
                {
                    S_UnitSelectorMenu.Instance.player1SelectedUnits[i].selectedUnit.GetComponent<Unit>().attack += dmgBonus;
                }
            }
        }
        else
        {
            if (S_GameManager.Instance.player2CharacterMoney.ammount >= price)
            {
                for (int i = 0; i < S_UnitSelectorMenu.Instance.player2SelectedUnits.Count; i++)
                {
                    S_UnitSelectorMenu.Instance.player2SelectedUnits[i].selectedUnit.GetComponent<Unit>().attack += dmgBonus;
                }
            }
        }
    }

    public void AttackBuffWithTurnCharge()
    {
        if (S_GameManager.Instance.isPlayer1Turn)
        {
            if (S_GameManager.Instance.player1CharacterMoney.ammount >= price)
            {
                for (int i = 0; i < S_UnitSelectorMenu.Instance.player1SelectedUnits.Count; i++)
                {
                    S_UnitSelectorMenu.Instance.player1SelectedUnits[i].selectedUnit.GetComponent<Unit>().attack += 20;
                    S_UnitSelectorMenu.Instance.player1SelectedUnits[i].selectedUnit.GetComponent<Unit>().turnCharge += 1;
                }
            }
        }
        else
        {
            if (S_GameManager.Instance.player2CharacterMoney.ammount >= price)
            {
                for (int i = 0; i < S_UnitSelectorMenu.Instance.player2SelectedUnits.Count; i++)
                {
                    S_UnitSelectorMenu.Instance.player2SelectedUnits[i].selectedUnit.GetComponent<Unit>().attack += 20;
                    S_UnitSelectorMenu.Instance.player2SelectedUnits[i].selectedUnit.GetComponent<Unit>().turnCharge += 1;
                }
            }
        }
    }

    public void TurnChargeBuffWithAttack()
    {
        if (S_GameManager.Instance.isPlayer1Turn)
        {
            if (S_GameManager.Instance.player1CharacterMoney.ammount >= price)
            {
                for (int i = 0; i < S_UnitSelectorMenu.Instance.player1SelectedUnits.Count; i++)
                {
                    S_UnitSelectorMenu.Instance.player1SelectedUnits[i].selectedUnit.GetComponent<Unit>().attack -= 20;
                    S_UnitSelectorMenu.Instance.player1SelectedUnits[i].selectedUnit.GetComponent<Unit>().turnCharge -= 1;
                }
            }
        }
        else
        {
            if (S_GameManager.Instance.player2CharacterMoney.ammount >= price)
            {
                for (int i = 0; i < S_UnitSelectorMenu.Instance.player2SelectedUnits.Count; i++)
                {
                    S_UnitSelectorMenu.Instance.player2SelectedUnits[i].selectedUnit.GetComponent<Unit>().attack -= 20;
                    S_UnitSelectorMenu.Instance.player2SelectedUnits[i].selectedUnit.GetComponent<Unit>().turnCharge -= 1;
                }
            }
        }
    }

    public void UnitStatsSwitch()
    {
        if (S_GameManager.Instance.isPlayer1Turn)
        {
            if (S_GameManager.Instance.player1CharacterMoney.ammount >= price)
            {
                for (int i = 0; i < S_UnitSelectorMenu.Instance.player1SelectedUnits.Count; i++)
                {
                    int baseattack = S_UnitSelectorMenu.Instance.player1SelectedUnits[i].selectedUnit.GetComponent<Unit>().attack;
                    S_UnitSelectorMenu.Instance.player1SelectedUnits[i].selectedUnit.GetComponent<Unit>().attack = S_UnitSelectorMenu.Instance.player1SelectedUnits[i].selectedUnit.GetComponent<Unit>().defense;
                    S_UnitSelectorMenu.Instance.player1SelectedUnits[i].selectedUnit.GetComponent<Unit>().defense = baseattack;
                }
            }
        }
        else
        {
            if (S_GameManager.Instance.player2CharacterMoney.ammount >= price)
            {
                for (int i = 0; i < S_UnitSelectorMenu.Instance.player2SelectedUnits.Count; i++)
                {
                    int baseattack = S_UnitSelectorMenu.Instance.player2SelectedUnits[i].selectedUnit.GetComponent<Unit>().attack;
                    S_UnitSelectorMenu.Instance.player2SelectedUnits[i].selectedUnit.GetComponent<Unit>().attack = S_UnitSelectorMenu.Instance.player2SelectedUnits[i].selectedUnit.GetComponent<Unit>().defense;
                    S_UnitSelectorMenu.Instance.player2SelectedUnits[i].selectedUnit.GetComponent<Unit>().defense = baseattack;
                }
            }
        }
    }
}
