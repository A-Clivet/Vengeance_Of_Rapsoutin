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
    public string characterName;
    public Sprite sprite;
    public int maxHP;
    public int maxAdrenaline;
    public S_SpecialCapacityStats specialCapacity;
}