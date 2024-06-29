using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class S_CharacterManager : MonoBehaviour
{
    #region Variables
    public static S_CharacterManager Instance;

    // Created to let other scripts access player1's character and player2's character
    [HideInInspector] public GameObject player1CharacterGameObject { get; private set; }
    [HideInInspector] public GameObject player2CharacterGameObject { get; private set; }

    [Header("Score references :")]
    [SerializeField] Sprite _emptyScorePoint;
    [SerializeField] Sprite _scorePointFilled;

    [Header("References :")]
    [SerializeField] GameObject _character1Prefab;
    [SerializeField] GameObject _character2Prefab;
    [SerializeField] GameObject _allCharactersParent;

    S_GameManager _gameManager;
    #endregion

    #region Methods
    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
        _gameManager = S_GameManager.Instance;
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

        #region Settings up characters caracteristics
        // Setting characterName of the character to his correspondant values
        character.name = p_characterStats.characterName;

        // Assignation of a sprite to the character (WARNING : The object named "CharacterSprite" in the prefab Character1 and 2 must be the first child)
        character.transform.GetChild(0).GetComponent<Image>().sprite = p_characterStats.sprite;

        // Transfert health stats
        character.GetComponent<S_CharacterHealth>().RecieveCharacterHealthStats(
            p_characterStats.maxHP,
            p_isPlayer1Character,
            _emptyScorePoint,
            _scorePointFilled,
            // Cheat code's part
            _gameManager.areCheatCodesEnable,
            _gameManager.cheatCodes.healthStatsChangement
        );

        // Transfert adrenaline stats
        character.GetComponent<S_CharacterAdrenaline>().RecieveCharacterAdrenalineStats(
            p_characterStats.maxAdrenaline,
            p_characterStats.specialCapacity,
            p_isPlayer1Character,
            // Cheat code's part
            _gameManager.areCheatCodesEnable,
            _gameManager.cheatCodes.adrenalineStatsChangement
        );

        //Transfert money stats
        character.GetComponent<S_CharacterMoney>().RecieveCharacterMoneyStats(p_characterStats.money);

        // Transfert special capacity stats
        S_CharacterSpecialCapacity characterSpecialCapacity = character.GetComponent<S_CharacterSpecialCapacity>();

        characterSpecialCapacity.RecieveSpecialCapacityStats(
            p_characterStats.specialCapacity.capacityName,
            p_characterStats.specialCapacity.capacityDescription,
            p_characterStats.specialCapacity.capacityEffectDescription
        );

        // Store the GameObject reference of the created character in the corresponding variable
        // and linking the method "ShowSpecialCapacityDescriptionUIs" to the Show Skill Description action
        // (this event is performed when the player press Alt)
        if (p_isPlayer1Character)
        {
            player1CharacterGameObject = character;

            // COMMENTED BECAUSE THE SPECIAL CAPACITY INPUT WAS DELETED
            // _gameManager.player1InputsGameObject.GetComponent<PlayerInput>().actions["Show Special Capacity Description"].performed += characterSpecialCapacity.ShowSpecialCapacityDescriptionUIs;
        }
        else
        {
            player2CharacterGameObject = character;

            // COMMENTED BECAUSE THE SPECIAL CAPACITY INPUT WAS DELETED
            // _gameManager.player2InputsGameObject.GetComponent<PlayerInput>().actions["Show Special Capacity Description"].performed += characterSpecialCapacity.ShowSpecialCapacityDescriptionUIs;
        }

        #endregion
    }

    #endregion
}