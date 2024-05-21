using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "ScriptableObject/CharacterStats")]
public class S_CharacterStats : ScriptableObject
{
    // -- Enums -- //

    public enum SpecialCapacities
    {
        VodkaSprinklers,
        DarkMagic
    }

    // -- Variables -- //

    [Header("Statistics :")]
    public string Name;
    public Sprite Sprite;
    public int MaxHP;
    public int MaxAdrenaline;
    public SpecialCapacities SpecialCapacity;
}