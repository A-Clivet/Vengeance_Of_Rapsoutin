using UnityEngine;

public class S_CharacterMoney : MonoBehaviour
{
    [SerializeField] private int _ammount;

    public int ammount
    {
        get { return _ammount; }
    }

    public bool Buy(int value)
    {
        if (_ammount - value >= 0)
        {
            _ammount -= value;
            return true;
        }
        return false;
    }

    public void AddMoney(int amount)
    {
        _ammount += amount;
    }

    public void RecieveCharacterMoneyStats(int p_moneyAmmount)
    {
        _ammount = p_moneyAmmount;
    }
}
