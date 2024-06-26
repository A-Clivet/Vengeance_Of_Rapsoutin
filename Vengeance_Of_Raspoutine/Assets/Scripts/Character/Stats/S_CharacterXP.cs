using UnityEngine;

public class S_CharacterXP : MonoBehaviour
{
    private int _ammount = 0;

    public int ammount
    {
        get { return _ammount; }
    }

    public void GainXP(int p_value)
    {
        _ammount += p_value;
    }

    public void LoseXP(int p_quantity)
    {
        _ammount -= p_quantity;
    }
}
