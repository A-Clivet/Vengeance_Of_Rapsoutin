using UnityEngine;

public class S_TESTING : MonoBehaviour
{
    // NOTE : Used to simulate the GameManager

    [SerializeField] S_CharacterStats _character1Stats;
    [SerializeField] S_CharacterStats _character2Stats;

    // Start is called before the first frame update
    void Start()
    {
        S_CharacterManager.Instance.SpawnCharacter(_character1Stats, true);
        S_CharacterManager.Instance.SpawnCharacter(_character2Stats, false);
    }
}