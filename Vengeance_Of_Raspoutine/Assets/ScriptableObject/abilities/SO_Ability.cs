using UnityEngine;

public class SO_Ability : ScriptableObject
{
    public enum AbilityType
    {
        Passive,
        Consumable
    }

    [Header("Statistics :")]
    public AbilityType type;
    public int price;

    //public void AttackBuffRelativeToTurnNumber()
    //{
    //    if (S_GameManager.Instance.isPlayer1Turn)
    //    {
    //        if (S_GameManager.Instance.player1CharacterMoney.ammount >= price)
    //        {
    //            for (int i=0; i < S_GameManager.Instance.player1GridManager.unitList.Count; i++)
    //            {
    //                S_GameManager.Instance.player1GridManager.unitList[i].attack += 1;
    //            }
    //        }
    //    }
    //}
}
