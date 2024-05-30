using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_GameManager : MonoBehaviour
{
    #region Variables
    public static S_GameManager Instance;


    #region Getter / Setter
    public bool isPlayer1Turn { get; private set; } = true;

    public int player1ScorePoint { get; private set; }
    public int player2ScorePoint { get; private set; }

    // Character's health script references
    public S_CharacterHealth player1CharacterHealth { get; private set; }
    public S_CharacterHealth player2CharacterHealth { get; private set; }

    // Character's adrenaline script references
    public S_CharacterAdrenaline player1CharacterAdrenaline { get; private set; }
    public S_CharacterAdrenaline player2CharacterAdrenaline { get; private set; }

    public S_CharacterMoney player1CharacterMoney { get; private set; }
    public S_CharacterMoney player2CharacterMoney { get; private set; }

    // Local variable that store the _mapIndex variable's value (this variable is needed for the _mapIndex getter setter to exist)
    int __mapIndex = 2;

    /// <summary> Private int variable that when change will actualise the map visuals </summary>
    private int _mapIndex
    {
        get { return __mapIndex; }
        set
        {
            __mapIndex = value;

            if (__mapIndex < 0)
            {
                __mapIndex = 0;
            }
            else if (__mapIndex > mapSelection.Count - 1)
            {
                __mapIndex = mapSelection.Count - 1;
            }

            // Update the map visual
            _gameBackgroundSpriteRenderer.sprite = mapSelection[_mapIndex];
        }
    }

    #endregion

    [Header("Player's inputs GameObject references :")]
    public GameObject player1Inputs;
    public GameObject player2Inputs;

    [Header("Background references :")]
    [SerializeField] private SpriteRenderer _gameBackgroundSpriteRenderer;
    public List<Sprite> mapSelection = new(new Sprite[5]);

    [Header("Panel references :")]
    [SerializeField] private GameObject _panelPlayer1;
    [SerializeField] private GameObject _panelPlayer2;

    [Header("UnitCall buttons :")]
    [SerializeField] private Button _player1UnitCallButton;
    [SerializeField] private Button _player2UnitCallButton;

    [Header("Turn references :")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _playerTurnText;
    [SerializeField] private TextMeshProUGUI _turnsText;

    [Header("Characters stats's references :")]
    [SerializeField] S_CharacterStats _character1Stats;
    [SerializeField] S_CharacterStats _character2Stats;

    [Header("Unit manager's references")]
    [SerializeField] public S_UnitManager unitManagerP1;
    [SerializeField] public S_UnitManager unitManagerP2;

    [Header("Cooldown between actions :")]
    [SerializeField] private float _cooldown;

    [Header("Player's grid :")]
    [SerializeField] private S_GridManager _player1GridManager;
    [SerializeField] private S_GridManager _player2GridManager;

    // Character manager's reference
    S_CharacterManager _characterManager;

    //End Menu ref
    [Header("End menu reference :")]
    [SerializeField] private S_EndMenu _endMenu;

    private float _targetTime;
    private int _currentRoundNumber;
    private int _playerActionNumber;

    int _loseCoefficient = 1;

    bool _isInMainMenuScene = true;
    #endregion

    #region Methods

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.DestructionOfTheSecondOne);
        DontDestroyOnLoad(transform.parent);
    }

    private void Start()
    {
        //if (_isInMainMenuScene)
        //{
        //
        //}
        //// If we are in the MainGame Scene
        //else
        //{
            _targetTime = 60f;
            _currentRoundNumber = 1;
            _playerActionNumber = 3;
            _timerText.text = _targetTime.ToString();

            _gameBackgroundSpriteRenderer.sprite = mapSelection[_mapIndex]; // set the current sprite on start

            RandomStartTurn();

            #region Characters management

            // Setting up character manager reference
            _characterManager = S_CharacterManager.Instance;

            // Creating the player's character
            _characterManager.SpawnCharacter(_character1Stats, true);
            _characterManager.SpawnCharacter(_character2Stats, false);

            // Setting up character's adrenaline and health and money script references
            player1CharacterAdrenaline = _characterManager.player1CharacterGameObject.GetComponent<S_CharacterAdrenaline>();
            player2CharacterAdrenaline = _characterManager.player2CharacterGameObject.GetComponent<S_CharacterAdrenaline>();

            player1CharacterHealth = _characterManager.player1CharacterGameObject.GetComponent<S_CharacterHealth>();
            player2CharacterHealth = _characterManager.player2CharacterGameObject.GetComponent<S_CharacterHealth>();

            player1CharacterMoney = _characterManager.player1CharacterGameObject.GetComponent<S_CharacterMoney>();
            player2CharacterMoney = _characterManager.player2CharacterGameObject.GetComponent<S_CharacterMoney>();


            // Enable / disable special capacity button's interaction
            player1CharacterAdrenaline.RecieveNewTurnInfo(isPlayer1Turn);
            player2CharacterAdrenaline.RecieveNewTurnInfo(isPlayer1Turn);

            // Updates the character's score visuals
            player1CharacterHealth.RecieveScoreInfo(player1ScorePoint, true);
            player2CharacterHealth.RecieveScoreInfo(player2ScorePoint, false);

            #endregion

            if (isPlayer1Turn)
            {
                _playerTurnText.text = "Player 1 turn";

                _panelPlayer1.SetActive(false);
                _panelPlayer2.SetActive(true);
            }
            else if (!isPlayer1Turn)
            {
                _playerTurnText.text = "Player 2 turn";

                _panelPlayer1.SetActive(true);
                _panelPlayer2.SetActive(false);
            }
        //}
    }

    private void Update()
    {
        // Decrease of the turn timer
        _targetTime -= Time.deltaTime;

        // Display the rounded timer in seconds in a text
        _timerText.text = "Remaining Time : " + ((int)_targetTime).ToString();

        // Display the current round number
        _turnsText.text = "Turns : " + _currentRoundNumber.ToString();

        // Check if the timer is equal or less to 0
        if (_targetTime <= 0.0f)
        {
            EndTurn();
        }
    }

    public void UnitCallOnOff(int p_playerNumber, bool p_isActive)
    {
        switch (p_playerNumber)
        {
            case 1:
                _player1UnitCallButton.interactable = p_isActive;
                break;
            case 2:
                _player2UnitCallButton.interactable = p_isActive;
                break;
            default:
                Debug.LogError("Player number incorrect");
                break;
        }
    }

    /// <summary> Allows to randomise wich player starts at the start of the game </summary>
    private void RandomStartTurn()
    {
        // Take a random number beetween 0 and 2 (2 excluded)
        int randomNumber = Random.Range(0, 2);

        switch (randomNumber)
        {
            case 0:
                isPlayer1Turn = false;
                _player1UnitCallButton.interactable = false;
                break;

            case 1:
                isPlayer1Turn = true;
                _player2UnitCallButton.interactable = false;
                break;
        }
    }

    /// <summary> Change the map when the player lose a point or win a point and add a point to the player 1 or 2
    /// and checks if the player 1 or 2 wins </summary>
    /// <param name="p_isPlayer1Dead"></param>
    public void HandlePlayerLose(bool p_isPlayer1Dead)
    {
        // Add a score point to the player who won, let the player who lost play the first in the new round,
        // change the map progression according to the entire game 
        if (p_isPlayer1Dead)
        {
            player2ScorePoint++;

            isPlayer1Turn = true;

            _mapIndex += 1 * _loseCoefficient;
        }
        else
        {
            player1ScorePoint++;

            isPlayer1Turn = false;

            _mapIndex -= 1 * _loseCoefficient;
        }

        _loseCoefficient++;

        if (player1ScorePoint >= 1) // mettre a 3 pour les builds suivantes
        {
            _endMenu.WhoWin(true);
        }

        if (player2ScorePoint >= 1) // mettre a 3 pour les builds suivantes
        {
            _endMenu.WhoWin(false);
        }

        #region Characters management
        // Updates the character's score visuals
        player1CharacterHealth.RecieveScoreInfo(player1ScorePoint, true);
        player2CharacterHealth.RecieveScoreInfo(player2ScorePoint, false);

        // Reset players character's stats
        player1CharacterHealth.ResetHealthStats();
        player2CharacterHealth.ResetHealthStats();

        player1CharacterAdrenaline.ResetAdrenalineStats();
        player2CharacterAdrenaline.ResetAdrenalineStats();
        #endregion

        _gameBackgroundSpriteRenderer.sprite = mapSelection[_mapIndex];
    }

    /// <summary> End the turn of the player who played and let the other player play, reset the timer to 60s and adds 1 to the current round number </summary>
    public void EndTurn()
    {
        if (isPlayer1Turn)
        {
            isPlayer1Turn = false;

            _currentRoundNumber += 1;

            _playerTurnText.text = "Player 2 Turn";

            _panelPlayer1.SetActive(true);

            _player1UnitCallButton.interactable = false;
            _player2UnitCallButton.interactable = true;

            StartTurnCheckUnit();

            DeactivateGrid();

            _panelPlayer2.SetActive(false);
        }

        else if (!isPlayer1Turn)
        {
            isPlayer1Turn = true;

            _currentRoundNumber += 1;

            _playerTurnText.text = "Player 1 Turn";

            _panelPlayer1.SetActive(false);

            _player1UnitCallButton.interactable = true;
            _player2UnitCallButton.interactable = false;

            StartTurnCheckUnit();

            DeactivateGrid();

            _panelPlayer2.SetActive(true);
        }

        _targetTime = 60.0f;
        _playerActionNumber = 3;

        // Enable / disable special capacity button's interaction
        player1CharacterAdrenaline.RecieveNewTurnInfo(isPlayer1Turn);
        player2CharacterAdrenaline.RecieveNewTurnInfo(isPlayer1Turn);
    }

    public void StartTurnCheckUnit()
    {
        // To avoid having to manage twog grid manager variables
        // we create a local variable nammed "gridManager" it contain the S_GridManager we will use later,
        // this variable will change depending if it's player1 turn and vice versa. 
        S_GridManager gridManager = _player1GridManager;

        if (!isPlayer1Turn)
        {
            gridManager = _player1GridManager;
        }
        else
        {
            gridManager = _player2GridManager;
        }

        for (int i = 0; i < gridManager.width; i++)
        {
            for (int j = 0; j < Mathf.Abs(gridManager.height); j++)
            {
                Unit unit = gridManager.gridList[i][j].unit;

                if (unit != null)
                {
                    unit.ReturnToBaseTile();
                }
            }
        }

        if (!isPlayer1Turn)
        {
            gridManager = _player2GridManager;
        }
        else
        {
            gridManager = _player1GridManager;
        }

        // We loop throught all grid's tiles, looking for a unit
        for (int i = 0; i < gridManager.width; i++)
        {
            for (int j = 0; j < Mathf.Abs(gridManager.height); j++)
            {
                Unit unit = gridManager.gridList[i][j].unit;

                if (unit != null)
                {
                    unit.AttackCharge();
                }
            }
        }
    }

    public void ReduceActionPointBy1()
    {
        _playerActionNumber -= 1;

        if (isPlayer1Turn)
        {
            Debug.Log("combo is called P1");
            unitManagerP1.UnitCombo(3);
        }
        else
        {
            Debug.Log("combo is called P2");
            unitManagerP2.UnitCombo(3);
        }

        // Action time cooldown
        StartCoroutine(LaunchActionCooldown());

        // Ends the player who played's turn if he doeasn't have any action points left
        if (_playerActionNumber <= 0)
        {
            EndTurn();
        }
    }

    public void IncreaseActionPointBy1()
    {
        _playerActionNumber += 1;
    }

    private IEnumerator LaunchActionCooldown()
    {
        player1Inputs.SetActive(false);
        player2Inputs.SetActive(false);

        yield return new WaitForSecondsRealtime(_cooldown);

        player1Inputs.SetActive(true);
        player2Inputs.SetActive(true);
    }

    public void DeactivateGrid()
    {
        for (int i = 0; i < _player1GridManager.width; i++)
        {
            for (int j = 0; j < Mathf.Abs(_player1GridManager.height); j++)
            {
                if (isPlayer1Turn)
                {
                    _player2GridManager.gridList[i][j].GetComponent<BoxCollider2D>().enabled = false;
                    _player1GridManager.gridList[i][j].GetComponent<BoxCollider2D>().enabled = true;

                }
                else
                {
                    _player1GridManager.gridList[i][j].GetComponent<BoxCollider2D>().enabled = false;
                    _player2GridManager.gridList[i][j].GetComponent<BoxCollider2D>().enabled = true;
                }
            }
        }
        if (isPlayer1Turn)
        {
            foreach (Unit unit in _player2GridManager.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = false;
            }
            foreach (Unit unit in _player1GridManager.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        else
        {
            foreach (Unit unit in _player1GridManager.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = false;
            }
            foreach (Unit unit in _player2GridManager.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
    #endregion
}