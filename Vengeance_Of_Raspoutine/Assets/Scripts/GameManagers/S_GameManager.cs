using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_GameManager : MonoBehaviour
{
    #region Enum
    public enum TurnEmun
    {
        Player1Turn,
        Player2Turn,

        // Used to let the visuels, animations finished
        TransitionTurn,
    }
    #endregion

    #region Variables

    public static S_GameManager Instance;

    #region Getter / Setter

    // -- Turn manager variables -- //
    TurnEmun _currentTurn;

    /// <summary> Manages the transition between player turns when a new value is set. 
    /// Adjusts the interactability of player unit call buttons, updates the player action preventer panels, 
    /// and changes the displayed player turn text. </summary>
    public TurnEmun currentTurn 
    { 
        get { return _currentTurn; }
        private set 
        {
            _currentTurn = value;

            if (_currentTurn == TurnEmun.Player1Turn)
            {
                // Set the boolean variable "isPlayer1Turn" to the corresponding value so that other functions dependent on this value function correctly
                isPlayer1Turn = true;

                // Add one to the total number of turn passed in the round
                _currentRoundNumber++;

                // Change the interactability of the unit call's buttons to the corresponding value
                player1UnitCallButton.interactable = true;
                player2UnitCallButton.interactable = false;

                // Enable / disable the two player's action preventer panel
                _player1ActionPreventerVisualGameObject.SetActive(false);
                _player2ActionPreventerVisualGameObject.SetActive(true);

                // Change the player turn text to the corresponding value
                _playerTurnTextUI.text = "Player 1 turn";

                // Reset the number of actions
                _playerActionNumber = 3;
            }
            else if (_currentTurn == TurnEmun.Player2Turn)
            {
                // Same as for the player1 but for the player2

                isPlayer1Turn = false;

                _currentRoundNumber++;

                player1UnitCallButton.interactable = false;
                player2UnitCallButton.interactable = true;

                _player1ActionPreventerVisualGameObject.SetActive(true);
                _player2ActionPreventerVisualGameObject.SetActive(false);

                _playerTurnTextUI.text = "Player 2 turn";

                _playerActionNumber = 3;
            }
            else if (_currentTurn == TurnEmun.TransitionTurn)
            {
                // In case if this turn is too long we disable all possible interactions for all players

                player1UnitCallButton.interactable = false;
                player2UnitCallButton.interactable = false;

                // NOTE : You can uncomment the code below if you see that the player can, for exemple, grap a unit after he finish his turn
                // (WARNING THO : That make the screen flash thats why it's currently commented)

                //_player1ActionPreventerVisualGameObject.SetActive(true);
                //_player2ActionPreventerVisualGameObject.SetActive(true);
            }
            else
            {
                Debug.LogError(
                    "ERROR ! You tried to change the variable '" + currentTurn.ToString() + "' to '" + value.ToString() + 
                    "' but it's not planned into the variable's code. UNITY IS PAUSED !"
                );
                Debug.Break();
                return;
            }

            DeactivateGrid();

            StartTurnCheckUnit();

            _turnTimerTime = 60.0f;
        }
    }

    public bool isPlayer1Turn { get; private set; } = true;

    // -- Player's score variables -- //
    public int player1ScorePoint { get; private set; }
    public int player2ScorePoint { get; private set; }

    // -- Unit managers's references -- //
    public S_UnitManager player1unitManager { get; private set; }
    public S_UnitManager player2unitManager { get; private set; }

    // -- Unit call button's references -- //
    public Button player1UnitCallButton { get; private set; }
    public Button player2UnitCallButton { get; private set; }

    // -- Players's inputs game object's references -- //
    public GameObject player1InputsGameObject { get; private set; }
    public GameObject player2InputsGameObject { get; private set; }

    // -- Players's grid managers's references -- //
    public S_GridManager player1GridManager { get; private set; }
    public S_GridManager player2GridManager { get; private set; }

    // -- Character's health script's references -- //
    public S_CharacterHealth player1CharacterHealth { get; private set; }
    public S_CharacterHealth player2CharacterHealth { get; private set; }

    // -- Character's adrenaline script's references -- //
    public S_CharacterAdrenaline player1CharacterAdrenaline { get; private set; }
    public S_CharacterAdrenaline player2CharacterAdrenaline { get; private set; }

    // -- Character's money script's references -- //
    public S_CharacterMoney player1CharacterMoney { get; private set; }
    public S_CharacterMoney player2CharacterMoney { get; private set; }

    // -- Character's experience script's references -- //
    // TODO : Uncomment when there is an S_CharacterXP script 
    //public S_CharacterXP player1CharacterXP { get; private set; }
    //public S_CharacterXP player2CharacterXP { get; private set; }

    // Local variable that store the _mapIndex variable's value (this variable is needed for the _mapIndex getter setter to exist)
    int __mapIndex = 2;

    /// <summary> Private int variable that when change will manage if we go outside mapSelection's range,
    /// will launch the end of the game if any player won,
    /// and will update the game background sprite according to the players points </summary>
    private int _mapIndex
    {
        get { return __mapIndex; }
        set
        {
            __mapIndex = value;

            // Check if we go outside mapSelection's range
            if (__mapIndex < 0)
            {
                __mapIndex = 0;
            }
            else if (__mapIndex > mapSelection.Count - 1)
            {
                __mapIndex = mapSelection.Count - 1;
            }

            // Check if any player won
            if (player1ScorePoint >= _pointsNeededToWin)
            {
                _endMenuManager.WhoWin(true);
                return;
            }

            if (player2ScorePoint >= _pointsNeededToWin)
            {
                _endMenuManager.WhoWin(false);
                return;
            }

            // Updating the map according to the players points
            _gameBackgroundSpriteRenderer.sprite = mapSelection[__mapIndex];
        }
    }
    #endregion

    #region Serialized variables

    [Header("Background references :")]
    public List<Sprite> mapSelection = new(new Sprite[5]);

    [Header("Characters stats's references :")]
    [SerializeField] S_CharacterStats _character1Stats;
    [SerializeField] S_CharacterStats _character2Stats;

    [Header("Cooldown between actions :")]
    [SerializeField] private float _cooldownBetweenPlayerActions;
    #endregion

    #region Private variable

    // -- Character manager's reference -- //
    S_CharacterManager _characterManager;

    // -- Game's game mode -- //
    S_GameModeInvoker.GameModes _currentGameMode;

    // -- Players's actions -- //
    int _startingPlayerActionNumber = 3;
    int _playerActionNumber;

    // -- Informations showns to the player -- //
    float _turnTimerTime;
    int _currentRoundNumber = 0;

    // -- Text UIs who shows to the player informations -- //
    TextMeshProUGUI _turnTimerTextUI;
    TextMeshProUGUI _playerTurnTextUI;
    TextMeshProUGUI _playerActionsLeftTextUI;
    TextMeshProUGUI _totalTurnsTextUI;

    // -- Players action preventer visual game objects's references -- //
    GameObject _player1ActionPreventerVisualGameObject;
    GameObject _player2ActionPreventerVisualGameObject;

    // -- Score management -- //
    // ("_loseCoefficient" is used to manage score point on the Classic game mode)
    int _loseCoefficient = 1;
    int _pointsNeededToWin = 3;

    // -- End menu manager's reference -- //
    S_EndMenuManager _endMenuManager;

    // -- Game background sprite renderer's reference -- //
    SpriteRenderer _gameBackgroundSpriteRenderer;

    // -- Money management -- //
    int _moneyToHadToPlayer1WhenHeLose;
    int _moneyToHadToPlayer2WhenHeLose;
    #endregion

    #endregion

    #region Methods

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndDestructionOfTheSecondOne);
    }

    private void Start()
    {
        #region Setting variables

        #region Getter / Setter variables

        // -- Unit managers's references -- //
        player1unitManager = S_UnitManagersHandler.Instance.player1UnitManager;
        player2unitManager = S_UnitManagersHandler.Instance.player2UnitManager;

        // -- Unit call button's references -- //
        player1UnitCallButton = S_UnitCallButtonHandler.Instance.player1UnitCallButton;
        player2UnitCallButton = S_UnitCallButtonHandler.Instance.player2UnitCallButton;

        // -- Players's inputs game object's references -- //
        player1InputsGameObject = S_PlayerInputsHandler.Instance.player1InputsGameObject;
        player2InputsGameObject = S_PlayerInputsHandler.Instance.player2InputsGameObject;

        // -- Players's grid managers's references -- //
        player1GridManager = S_GridManagersHandler.Instance.player1GridManager;
        player2GridManager = S_GridManagersHandler.Instance.player2GridManager;
        #endregion

        #region Private variables

        // -- Text UIs who shows to the player informations -- //

        // Creation of a local variable to avoid calling Instance 4 times
        S_BattleUIsReferencesHandler _battleUIsReferencesHandler = S_BattleUIsReferencesHandler.Instance;

        _turnTimerTextUI = _battleUIsReferencesHandler.turnTimerTextUI;
        _playerTurnTextUI = _battleUIsReferencesHandler.playerTurnTextUI;
        _playerActionsLeftTextUI = _battleUIsReferencesHandler.playerActionsLeftTextUI;
        _totalTurnsTextUI = _battleUIsReferencesHandler.totalTurnsTextUI;

        // -- Players action preventer visual game objects's references -- //
        _player1ActionPreventerVisualGameObject = S_PlayersActionPreventerVisualUIReferencesHandler.Instance.player1ActionPreventerVisualGameObject;
        _player2ActionPreventerVisualGameObject = S_PlayersActionPreventerVisualUIReferencesHandler.Instance.player2ActionPreventerVisualGameObject;

        // -- End menu manager's reference -- //
        _endMenuManager = S_EndMenuManager.Instance;

        // -- Game background sprite renderer's reference -- //
        _gameBackgroundSpriteRenderer = S_GameBackgroundSizeUpdaterManager.Instance.GetComponent<SpriteRenderer>();
        #endregion

        #endregion

        #region Game mode management

        // Getting the game mode chosen by the player
        _currentGameMode = S_CrossSceneDataManager.Instance.gameMode;

        // Setting up the number of round needed to win the game for one player
        switch (_currentGameMode)
        {
            // NOTE : For this only three game modes the number of points needed to win is the same, maybe that will change if we add others game modes
            // that is why there is the variable set to 3 in every case

            case S_GameModeInvoker.GameModes.Classic:
                _pointsNeededToWin = 3;
                break;

            case S_GameModeInvoker.GameModes.Domination:
                _pointsNeededToWin = 3;
                break;

            case S_GameModeInvoker.GameModes.SuddenDeath:
                _pointsNeededToWin = 3;
                player1ScorePoint = 2;
                player2ScorePoint = 2;
                break;
        }

        #endregion

        #region First turn management

        _playerActionNumber = _startingPlayerActionNumber;

        // Randomly determine the player who will play first in the initial turn
        RandomStartTurn();

        // Setting the initial map sprite index to the middle of the available maps
        _mapIndex = (int)(mapSelection.Count/ 2f);

        #endregion

        #region Characters management

        // Setting up character manager reference
        _characterManager = S_CharacterManager.Instance;

        // Creating the player's character
        _characterManager.SpawnCharacter(_character1Stats, true);
        _characterManager.SpawnCharacter(_character2Stats, false);

        // Setting up character's adrenaline, health, money and xp script references
        player1CharacterAdrenaline = _characterManager.player1CharacterGameObject.GetComponent<S_CharacterAdrenaline>();
        player2CharacterAdrenaline = _characterManager.player2CharacterGameObject.GetComponent<S_CharacterAdrenaline>();

        player1CharacterHealth = _characterManager.player1CharacterGameObject.GetComponent<S_CharacterHealth>();
        player2CharacterHealth = _characterManager.player2CharacterGameObject.GetComponent<S_CharacterHealth>();

        player1CharacterMoney = _characterManager.player1CharacterGameObject.GetComponent<S_CharacterMoney>();
        player2CharacterMoney = _characterManager.player2CharacterGameObject.GetComponent<S_CharacterMoney>();

        //player1CharacterXP = _characterManager.player1CharacterGameObject.GetComponent<S_CharacterXP>();
        //player2CharacterXP = _characterManager.player2CharacterGameObject.GetComponent<S_CharacterXP>();

        // Enable / disable special capacity button's interaction
        player1CharacterAdrenaline.RecieveNewTurnInfo(isPlayer1Turn);
        player2CharacterAdrenaline.RecieveNewTurnInfo(isPlayer1Turn);

        // Updates the character's score visuals
        player1CharacterHealth.RecieveScoreInfo(player1ScorePoint, true);
        player2CharacterHealth.RecieveScoreInfo(player2ScorePoint, false);

        #endregion
    }

    private void Update()
    {
        if (currentTurn != TurnEmun.TransitionTurn)
        {
            // Decrease of the turn timer
            _turnTimerTime -= Time.deltaTime;

            // Display the rounded timer in seconds in a text
            _turnTimerTextUI.text = "Remaining time : " + ((int)_turnTimerTime).ToString();

            // Display the current round number
            _playerActionsLeftTextUI.text = "Turn : " + _currentRoundNumber.ToString();

            // Display the player's number of action left he have 
            _totalTurnsTextUI.text = "Remaining actions : " + _playerActionNumber;

            // Check if the timer is equal or less to 0, if yes then end the turn
            if (_turnTimerTime <= 0.0f)
            {
                EndTurn();
            }
        }
    }

    #region Turns handling

    /// <summary> Randomly determine the player who will play first in the initial turn. </summary>
    private void RandomStartTurn()
    {
        // Take a random number beetween 0 and 2 (2 excluded)
        int randomNumber = UnityEngine.Random.Range(0, 2);

        switch (randomNumber)
        {
            case 0:
                currentTurn = TurnEmun.Player1Turn;
                break;

            case 1:
                currentTurn = TurnEmun.Player2Turn;
                break;
        }
    }

    /// <summary> End the turn of the player who played and let the other player play,
    /// reset the timer to 60s and adds 1 to the current round number </summary>
    public void EndTurn()
    {
        if (currentTurn == TurnEmun.TransitionTurn)
        {
            if (isPlayer1Turn)
            {
                currentTurn = TurnEmun.Player2Turn;
            }
            else
            {
                currentTurn = TurnEmun.Player1Turn;
            }
        }
        else
        {
            currentTurn = TurnEmun.TransitionTurn;
        }

        // Enable / disable special capacity button's interaction
        player1CharacterAdrenaline.RecieveNewTurnInfo(isPlayer1Turn);
        player2CharacterAdrenaline.RecieveNewTurnInfo(isPlayer1Turn);
    }
    #endregion

    /// <summary> Handle the players's score, map changement, the launching the end game if the conditions are reached and if not, reloading of a new round </summary>
    public void HandlePlayerLose(bool p_isPlayer1Dead)
    {
        // Used to modify (increase / decrease) the mapIndex variable depending on the game mode
        int _mapIndexModifier = 1;

        // To optimize (to avoid having to call "_currentGameMode == S_GameModeInvoker.GameModes.Domination" multiple time)
        bool _isDominationGameMode = false;

        // Set in our temporary variable is we are in Domination game mode
        if (_currentGameMode != S_GameModeInvoker.GameModes.Domination)
            _isDominationGameMode = true;

        if (!_isDominationGameMode)
        {
            _mapIndexModifier = 1 * _loseCoefficient;
        }

        // If the player2 won
        if (p_isPlayer1Dead)
        {
            if (_isDominationGameMode)
            {
                // If the player1 have the advantage we decrease his score
                if (player1ScorePoint > 0)
                    player1ScorePoint--;
                else
                    player2ScorePoint++;
            }
            else
            {
                player2ScorePoint++;
            }

            _mapIndex += _mapIndexModifier;

            // Changing the money that will be gived to each players
            _moneyToHadToPlayer1WhenHeLose = 5;
            _moneyToHadToPlayer2WhenHeLose = 10;
        }
        else
        {
            if (_isDominationGameMode)
            {
                // If the player1 have the advantage and player1 won
                if (player2ScorePoint > 0)
                    player2ScorePoint--;
                else
                    player1ScorePoint++;
            }
            else
            {
                player1ScorePoint++;
            }

            _mapIndex -= _mapIndexModifier;

            // Changing the money that will be gived to each players
            _moneyToHadToPlayer1WhenHeLose = 10;
            _moneyToHadToPlayer2WhenHeLose = 5;
        }

        currentTurn = TurnEmun.TransitionTurn;

        if (_isDominationGameMode)
            _loseCoefficient++;

        #region Characters management

        // Updates the character's score visuals
        player1CharacterHealth.RecieveScoreInfo(player1ScorePoint, true);
        player2CharacterHealth.RecieveScoreInfo(player2ScorePoint, false);

        // Reset players character's stats
        player1CharacterHealth.ResetHealthStats();
        player2CharacterHealth.ResetHealthStats();

        player1CharacterAdrenaline.ResetAdrenalineStats();
        player2CharacterAdrenaline.ResetAdrenalineStats();

        // Players's money management
        player1CharacterMoney.AddMoney(_moneyToHadToPlayer1WhenHeLose);
        player2CharacterMoney.AddMoney(_moneyToHadToPlayer2WhenHeLose);
        #endregion
    }

    /// <summary>
    /// TODO : After the merge of the refactored S_GameManager this function will be deleted,
    /// for the one who used it (S_Unit) 
    /// it will be replaced by the HandleUnitCallButtonInteraction function in the S_UnitCallHandler
    /// </summary>
    [Obsolete]
    public void UnitCallOnOff(int p_playerNumber, bool p_isActive)
    {
        switch (p_playerNumber)
        {
            case 1:
                player1UnitCallButton.interactable = p_isActive;
                break;
            case 2:
                player2UnitCallButton.interactable = p_isActive;
                break;
            default:
                Debug.LogError("ERROR ! The given player number '" + p_playerNumber + "' is incorrect, it is not planned in the switch");
                break;
        }
    }

    public void StartTurnCheckUnit()
    {
        // To avoid having to manage two grid manager variables
        // we create a local variable nammed "gridManager" it contain the S_GridManager we will use later,
        // this variable will change depending if it's player1 turn and vice versa. 
        S_GridManager gridManager = player1GridManager;

        if (!isPlayer1Turn)
        {
            gridManager = player2GridManager;
        }

        if (currentTurn == TurnEmun.TransitionTurn)
        {
            for (int i = 0; i < gridManager.AllUnitPerColumn.Count; i++)
            {
                for (int j = 0; j < Mathf.Abs(gridManager.AllUnitPerColumn[i].Count); j++)
                {
                    Unit unit = gridManager.AllUnitPerColumn[i][j];

                    unit.ReturnToBaseTile();
                }
            }
            bool formationAttacking = false;
            // We loop throught all grid's tiles, looking for a unit
            for (int i = 0; i < gridManager.unitManager.UnitColumn.Count; i++)
            {
                for (int j = 0; j < Mathf.Abs(gridManager.unitManager.UnitColumn[i].Count); j++)
                {
                    Unit unit = gridManager.unitManager.UnitColumn[i][j];

                    unit.AttackCharge();
                    if (unit.mustAttack)
                    {
                        formationAttacking = true;
                    }

                }
            }
            if (!formationAttacking)
            {
                EndTurn();
            }
        }
    }

    #region Action points handling

    public void ReduceActionPointBy1()
    {
        _playerActionNumber -= 1;

        if (currentTurn == TurnEmun.Player1Turn)
        {
            // Detect in the player1 grid if there are at least three units that are aligned vertically or horizontally
            player1unitManager.UnitCombo(3);
        }
        else if (currentTurn == TurnEmun.Player2Turn)
        {
            // Detect in the player2 grid if there are at least three units that are aligned vertically or horizontally
            player2unitManager.UnitCombo(3);
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
    #endregion

    private IEnumerator LaunchActionCooldown()
    {
        // Disable all the players inputs
        player1InputsGameObject.SetActive(false);
        player2InputsGameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(_cooldownBetweenPlayerActions);

        // Enable all the players inputs
        player1InputsGameObject.SetActive(true);
        player2InputsGameObject.SetActive(true);
    }

    /// <summary> Enable / disable according  the BoxCollider2D of all units,  </summary>
    public void DeactivateGrid()
    {
        /*for (int x = 0; x < player1GridManager.width; x++)
        {
            for (int y = 0; y < Mathf.Abs(player1GridManager.height); y++)
            {
                if (currentTurn == TurnEmun.Player1Turn)
                {
                    player2GridManager.gridList[x][y].GetComponent<BoxCollider2D>().enabled = false;
                    player1GridManager.gridList[x][y].GetComponent<BoxCollider2D>().enabled = true;

                }
                else if (currentTurn == TurnEmun.Player2Turn)
                {
                    player1GridManager.gridList[x][y].GetComponent<BoxCollider2D>().enabled = false;
                    player2GridManager.gridList[x][y].GetComponent<BoxCollider2D>().enabled = true;
                }
            }
        }*/

        if (currentTurn == TurnEmun.Player1Turn)
        {
            foreach (Unit unit in player1GridManager.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = true;
            }
            foreach (Unit unit in player2GridManager.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else if (currentTurn == TurnEmun.Player2Turn)
        {
            foreach (Unit unit in player1GridManager.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = false;
            }
            foreach (Unit unit in player2GridManager.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        else
        {
            foreach (Unit unit in player1GridManager.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = true;
            }
            foreach (Unit unit in player2GridManager.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
    #endregion
}