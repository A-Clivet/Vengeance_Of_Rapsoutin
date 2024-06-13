using UnityEngine;

public class S_CharacterXP : MonoBehaviour
{
    private int _ammount;

    public int ammount
    {
        get { return _ammount; }
    }

    public void GainXP(int p_value)
    {
        _ammount += p_value;
    }

    public void ResetAmmount()
    {
        _ammount = 0;
    }
}
