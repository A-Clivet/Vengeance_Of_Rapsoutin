using UnityEngine;
using UnityEngine.UI;

public class S_CharacterManager : MonoBehaviour
{
    #region Variables
    public static S_CharacterManager Instance;

    [Header("References :")]
    [SerializeField] GameObject _character1Prefab;
    [SerializeField] GameObject _character2Prefab;
    [SerializeField] GameObject _allCharactersParent;
    [SerializeField] S_SkillPanel _skillPanelP1;
    [SerializeField] S_SkillPanel _skillPanelP2;

    // Created to let other scripts access player1's character and player2's character
    [HideInInspector] public GameObject player1CharacterGameObject { get; private set; }
    [HideInInspector] public GameObject player2CharacterGameObject { get; private set; }
    #endregion

    #region Methods
    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
    }

    /// <summary> Create a new character based on the CharacterStats given </summary>
    /// <param characterName = "p_characterStats"> It contains all character's data that will be used in the game </param>
    /// <param characterName = "p_isPlayer1Character"> To know if the chracter will be for the player1 or 2 </param>
    public void SpawnCharacter(S_CharacterStats p_characterStats, bool p_isPlayer1Character)
    {
        #region Securities
        if (_allCharactersParent.transform.childCount > 2)
        {
            Debug.LogWarning("WARNING ! Trying to spawn a new character when there are already two in the game. THE CHARACTER HAS NOT BEEN CREATED");
            return;
        }

        if (player1CharacterGameObject != null && p_isPlayer1Character || player2CharacterGameObject != null && !p_isPlayer1Character) 
        {
            Debug.LogWarning("WARNING ! Trying to spawn a new character on top of an another character. THE CHARACTER HAS NOT BEEN CREATED");
            return;
        }
        #endregion

        #region Instantiation of the character's GameObject
        // To avoid having to manage two character's GameObject variables
        // we create a local variable nammed "characterPrefab" it contain the prefab we will use later,
        // this variable will change depending if we create the player2's character. 
        GameObject characterPrefab = _character1Prefab;

        if (!p_isPlayer1Character)
        {
            characterPrefab = _character2Prefab;
        }

        // Creation of the character
        GameObject character = Instantiate(characterPrefab, _allCharactersParent.transform);

        RectTransform prefabRectTransform = characterPrefab.GetComponent<RectTransform>();

        // Set the character's GameObject position (we can't do that with the Instantiate fonction because Unity change the position we give)
        character.GetComponent<RectTransform>().anchoredPosition = new Vector3(prefabRectTransform.anchoredPosition.x, prefabRectTransform.anchoredPosition.y, 0);
        #endregion

        #region Settings up character�s caracteristics
        // Setting characterName of the character to his correspondant values
        character.name = p_characterStats.characterName;

        // Assignation of a sprite to the character (WARNING : The object named "CharacterSprite" in the prefab Character1 and 2 must be the first child)
        character.transform.GetChild(0).GetComponent<Image>().sprite = p_characterStats.sprite;

        // Transfert health stats
        character.GetComponent<S_CharacterHealth>().RecieveCharacterHealthStats(p_characterStats.maxHP, p_isPlayer1Character);

        // Transfert adrenaline stats
        character.GetComponent<S_CharacterAdrenaline>().RecieveCharacterAdrenalineStats(p_characterStats.maxAdrenaline, p_characterStats.specialCapacity, p_isPlayer1Character);

        // Store the GameObject reference of the created character in the corresponding variable
        if (p_isPlayer1Character)
        {
            player1CharacterGameObject = character;
            //_skillPanelP1.SetDesc(p_characterStats.specialCapacity.capacityDesc);
        }
        else
        {
            player2CharacterGameObject = character;
            //_skillPanelP2.SetDesc(p_characterStats.specialCapacity.capacityDesc);
        }

            #endregion
        }

    #endregion
}