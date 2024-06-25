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
                currentRoundNumber++;

                // Change the interactability of the unit call's buttons to the corresponding value
                _unitCallButtonHandler.HandleUnitCallButtonInteraction(true, true);
                _unitCallButtonHandler.HandleUnitCallButtonInteraction(false, false);

                // Reset the number of actions
                playerActionNumber = 3;
            }
            else if (_currentTurn == TurnEmun.Player2Turn)
            {
                // Same as for the player1 but for the player2

                isPlayer1Turn = false;

                currentRoundNumber++;

                _unitCallButtonHandler.HandleUnitCallButtonInteraction(true, false);
                _unitCallButtonHandler.HandleUnitCallButtonInteraction(false, true);


                playerActionNumber = 3;
            }
            else if (_currentTurn == TurnEmun.TransitionTurn)
            {
                // In case if this turn is too long we disable all possible interactions for all players
                _unitCallButtonHandler.HandleUnitCallButtonInteraction(true, false);
                _unitCallButtonHandler.HandleUnitCallButtonInteraction(false, false);
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

            StartTurnCheckUnit();

            DeactivateGrid();

            _turnTimerTime = 60.0f;
        }
    }

    public bool isPlayer1Turn { get; private set; } = true;

    // -- Player's action point -- //
    public int playerActionNumber { get; set; }

    // -- Player's score variables -- //
    public int player1ScorePoint { get; private set; }
    public int player2ScorePoint { get; private set; }

    // -- Unit managers's references -- //
    public S_UnitManager player1unitManager { get; private set; }
    public S_UnitManager player2unitManager { get; private set; }

    // -- Unit call references -- //
    public S_UnitCall player1UnitCall { get; private set; }
    public S_UnitCall player2UnitCall { get; private set; }

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

    public S_CharacterXP player1CharacterXP { get; private set; }
    public S_CharacterXP player2CharacterXP { get; private set; }

    // Pause management
    public bool isGameRunning { get; set; } = true;

    public bool isLastPlayerDeadIsPlayer1 { get; private set; }

    public int swapCounterP1 { get; private set; }
    public int swapCounterP2 { get; private set; }

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
                _doesOnePlayerWonTheGame = true;

                _endMenuManager.WhoWin(true);
                return;
            }

            if (player2ScorePoint >= _pointsNeededToWin)
            {
                _doesOnePlayerWonTheGame = true;

                _endMenuManager.WhoWin(false);
                return;
            }

            // Call the start units for all players
            //_unitCallButtonHandler.CallUnitsForAllPlayers();

            // Updating the map according to the players points
            _gameBackgroundSpriteRenderer.sprite = mapSelection[__mapIndex];

            S_WeatherEvent.Instance.EventProbability();
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

    [Header("Animation player turn :")]
    [SerializeField] S_PlayerTurnAnimation _playerTurnAnimationScript;
    [SerializeField] private GameObject _characterImage;
    [SerializeField] private GameObject _playerTurnAnimationGO;

    [SerializeField] private GameObject _canvasAnimPlayer;
    
    [Header("Timer :")]
    [SerializeField] private Image _fill;
    #endregion

    #region Private variable

    // -- Character manager's reference -- //
    S_CharacterManager _characterManager;

    // -- Game's game mode -- //
    S_GameModeInvoker.GameModes _currentGameMode;

    // -- Players's actions -- //
    int _startingPlayerActionNumber = 3;

    // -- Informations showns to the player -- //
    float _turnTimerTime;
    float _maxTime = 60;
    Color _startTimer = new Color(0,1,144/255f);
    Color _middleTimer = new Color(1,196/255f,0);
    Color _endTimer = new Color(198/255f,4/255f,4/255f);
    public int currentRoundNumber = 0;

    // -- Text UIs who shows to the player informations -- //
    TextMeshProUGUI _playerActionsLeftTextUI;
    TextMeshProUGUI _totalTurnsTextUI;

    // -- Score management -- //
    // ("_loseCoefficient" is used to manage score point on the Classic game mode)
    int _loseCoefficient = 1;

    int _pointsNeededToWin = 3;

    bool _doesOnePlayerWonTheGame = false;

    // -- End menu manager's reference -- //
    S_EndMenuManager _endMenuManager;

    // Unit call button handler's reference
    public S_UnitCallButtonHandler _unitCallButtonHandler;

    // -- Game background sprite renderer's reference -- //
    SpriteRenderer _gameBackgroundSpriteRenderer;

    // -- Money management -- //
    int _moneyToGiveToPlayer1;
    int _moneyToGiveToPlayer2;

    /// <summary> Number of turn between each weather event (will launch a weather event when it reach is goal) </summary>
    int _playersPlayed = 0;
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
        player1UnitCall = S_UnitCallButtonHandler.Instance.player1UnitCall;
        player2UnitCall = S_UnitCallButtonHandler.Instance.player2UnitCall;

        // -- Players's inputs game object's references -- //
        player1InputsGameObject = S_PlayerInputsHandler.Instance.player1InputsGameObject;
        player2InputsGameObject = S_PlayerInputsHandler.Instance.player2InputsGameObject;

        // -- Players's grid managers's references -- //
        player1GridManager = S_GridManagersHandler.Instance.player1GridManager;
        player2GridManager = S_GridManagersHandler.Instance.player2GridManager;

        swapCounterP1 = 3;
        swapCounterP2 = 3;
        #endregion

        #region Private variables

        // -- Text UIs who shows to the player informations -- //

        // Creation of a local variable to avoid calling Instance 4 times
        S_BattleUIsReferencesHandler _battleUIsReferencesHandler = S_BattleUIsReferencesHandler.Instance;

        _playerActionsLeftTextUI = _battleUIsReferencesHandler.playerActionsLeftTextUI;
        _totalTurnsTextUI = _battleUIsReferencesHandler.totalTurnsTextUI;

        // -- End menu manager's reference -- //
        _endMenuManager = S_EndMenuManager.Instance;

        // -- Unit call button handler's reference -- //
        _unitCallButtonHandler = S_UnitCallButtonHandler.Instance;

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

        playerActionNumber = _startingPlayerActionNumber;

        
        // Setting the initial map sprite index to the middle of the available maps
        _mapIndex = (int)(mapSelection.Count/ 2f);

        // Randomly determine the player who will play first in the initial turn
        RandomStartTurn();

        _playerTurnAnimationScript.PlayTurnAnimation(_characterImage);

        if (S_CrossSceneDataManager.Instance.vsIA && currentTurn == TurnEmun.Player2Turn)
        {
           StartCoroutine(gameObject.GetComponent<S_RasputinIATree>().LaunchIa());
        }
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

        player1CharacterXP = _characterManager.player1CharacterGameObject.GetComponent<S_CharacterXP>();
        player2CharacterXP = _characterManager.player2CharacterGameObject.GetComponent<S_CharacterXP>();

        // Enable / disable special capacity button's interaction
        player1CharacterAdrenaline.RecieveNewTurnInfo(isPlayer1Turn);
        player2CharacterAdrenaline.RecieveNewTurnInfo(isPlayer1Turn);

        // Updates the character's score visuals
        player1CharacterHealth.RecieveScoreInfo(player1ScorePoint, true);
        player2CharacterHealth.RecieveScoreInfo(player2ScorePoint, false);
        #endregion

        S_GameManager.Instance.player1UnitCall.UnitCalling();
        S_GameManager.Instance.player2UnitCall.UnitCalling();

        DeactivateGrid();
    }

    private void Update()
    {
        if (currentTurn != TurnEmun.TransitionTurn)
        {
            // Decrease of the turn timer
            _turnTimerTime -= Time.deltaTime;


            // reduces the timer circle
            _fill.fillAmount = _turnTimerTime / _maxTime;
            
            // Color the timer circle
            if (_turnTimerTime > _maxTime / 2)
            {
                // 30's first second : green to yellow
                float lerpValue = (60 - _turnTimerTime) / (_maxTime / 2);
                _fill.color = Color.Lerp(_startTimer, _middleTimer, lerpValue);
            }
            else
            {
                // 30's last second : yellow to red
                float lerpValue = (_maxTime / 2 - _turnTimerTime) / (_maxTime / 2);
                _fill.color = Color.Lerp(_middleTimer, _endTimer, lerpValue);
            }

            // Display the current round number
            _playerActionsLeftTextUI.text = "Turn : " + currentRoundNumber.ToString();

            // Display the player's number of action left he have 
            _totalTurnsTextUI.text = "Remaining actions : " + playerActionNumber;

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
                S_SwapButtonsHandler.Instance.HandleSwapUnitButtonInteraction(false, false);
                break;

            case 1:
                currentTurn = TurnEmun.Player2Turn;
                S_SwapButtonsHandler.Instance.HandleSwapUnitButtonInteraction(true, false);
                break;
        }

        S_WeatherEvent.Instance.EventProbability();
        S_WeatherAnimation.Instance.PlayWeatherAnimation();

    }

    /// <summary> End the turn of the player who played and let the other player play,
    /// reset the timer to 60s and adds 1 to the current round number </summary>
    public void EndTurn()
    {
        if (player1GridManager.unitSelected != null)
        {
            player1GridManager.unitSelected.highlight.SetActive(false);
            player1GridManager.unitSelected = null;
        }
        if (player2GridManager.unitSelected != null)
        {
            player2GridManager.unitSelected.highlight.SetActive(false);
            player2GridManager.unitSelected = null;
        }
        S_SwapButtonsHandler.Instance.HandleSwapUnitButtonInteraction(!isPlayer1Turn, true);
        S_SwapButtonsHandler.Instance.HandleSwapUnitButtonInteraction(isPlayer1Turn, false);
        S_SwapButtonsHandler.Instance.HandleSwapUnitButtonEffects(isPlayer1Turn, false);
        if (currentTurn == TurnEmun.TransitionTurn)
        {
            // Re-organize all the unit in each player grid (removes gaps in grids)
            player1GridManager.UnitPriorityCheck();
            player2GridManager.UnitPriorityCheck();

            #region Weather event handling

            _playersPlayed++;

            // When the twos player have played
            if (_playersPlayed >= 2) 
            {
                S_WeatherEvent.Instance.currentEvent?.Invoke();
                _playersPlayed = 0;
            }
            #endregion

            if (isPlayer1Turn)
            {
                currentTurn = TurnEmun.Player2Turn;
                if (S_CrossSceneDataManager.Instance.vsIA)
                {
                    StartCoroutine(gameObject.GetComponent<S_RasputinIATree>().LaunchIa());
                }
            }
            else
            {
                currentTurn = TurnEmun.Player1Turn;
            }
            _playerTurnAnimationGO.SetActive(true);
            _playerTurnAnimationScript.PlayTurnAnimation(_characterImage);

        }
        else
        {
            _playerTurnAnimationGO.SetActive(false);
            currentTurn = TurnEmun.TransitionTurn;
        }
        

        // Enable / disable special capacity button's interaction
        player1CharacterAdrenaline.RecieveNewTurnInfo(isPlayer1Turn);
        player2CharacterAdrenaline.RecieveNewTurnInfo(isPlayer1Turn);
        player1GridManager.isSwapping = false;
        player2GridManager.isSwapping = false;
    }
    #endregion

    /// <summary> Handle the players's score, map changement, the launching the end game if the conditions are reached and if not, reloading of a new round </summary>
    public void HandlePlayerLose(bool p_isPlayer1Dead)
    {
        #region Skill tree handling

        // When the skill tree handler script will call the HandlePlayerLose function, it will be able to see who was the killed player
        isLastPlayerDeadIsPlayer1 = p_isPlayer1Dead;

        int _roundWinnerScorePoint = player1ScorePoint;

        // Changing the money that will be given to each players
        _moneyToGiveToPlayer1 = 10;
        _moneyToGiveToPlayer2 = 5;

        if (p_isPlayer1Dead)
        {
            _roundWinnerScorePoint = player2ScorePoint;

            // Changing the money that will be given to each players
            _moneyToGiveToPlayer1 = 5;
            _moneyToGiveToPlayer2 = 10;
        }

        // Players's money management
        player1CharacterMoney.AddMoney(_moneyToGiveToPlayer1);
        player2CharacterMoney.AddMoney(_moneyToGiveToPlayer2);

        // If the game is not in pause, and if any player has not won
        if (isGameRunning && _roundWinnerScorePoint < _pointsNeededToWin - 1)
        {
            isGameRunning = false;
            Time.timeScale = 0;

            foreach (Unit unit in player1GridManager.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = false;
            }

            foreach (Unit unit in player2GridManager.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = false;
            }

            S_SkillTreeHandler.Instance.player1SkillTree.SetActive(true);

            return;
        }
        #endregion

        // Destroy all unit on all grids, and recall UnitCall for the two players
        //S_RemoveUnit.Instance.RemoveAllUnits();

        // Used to modify (increase / decrease) the mapIndex variable depending on the game mode
        int _mapIndexModifier = 1;

        // To optimize (to avoid having to call "_currentGameMode == S_GameModeInvoker.GameModes.Domination" multiple time)
        bool _isDominationGameMode = false;

        // Set in our temporary variable is we are in Domination game mode
        if (_currentGameMode == S_GameModeInvoker.GameModes.Domination)
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

            // Will stop the execution if one player won the game
            if (_doesOnePlayerWonTheGame)
                return;
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

            // Will stop the execution if one player won the game
            if (_doesOnePlayerWonTheGame)
                return;
        }

        currentTurn = TurnEmun.TransitionTurn;

        _loseCoefficient++;

        // Call the start units for all players
        _unitCallButtonHandler.player1UnitCall.firstUnitCalled = false;
        _unitCallButtonHandler.player2UnitCall.firstUnitCalled = false;

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

        S_RemoveUnit.Instance.RemoveAllUnits();

        swapCounterP1 = 3;
        swapCounterP2 = 3;

        S_SwapButtonsHandler.Instance.player1SwapButton.interactable = p_isPlayer1Dead;
        S_SwapButtonsHandler.Instance.player2SwapButton.interactable = !p_isPlayer1Dead;
        S_SwapButtonsHandler.Instance.player1ButtonText.text = swapCounterP1.ToString();
        S_SwapButtonsHandler.Instance.player2ButtonText.text = swapCounterP2.ToString();

        if (!p_isPlayer1Dead)
        {
            currentTurn = TurnEmun.Player2Turn;
            isPlayer1Turn = false;
        }
        else
        {
            currentTurn = TurnEmun.Player1Turn;
            isPlayer1Turn = true;
        }
        _playerTurnAnimationScript.PlayTurnAnimation(_characterImage);
        if (S_CrossSceneDataManager.Instance.vsIA && currentTurn == TurnEmun.Player2Turn)
        {
            StartCoroutine(gameObject.GetComponent<S_RasputinIATree>().LaunchIa());
        }

    }

    /// <summary> Handle the players's score, map changement, the launching the end game if the conditions are reached and if not, reloading of a new round </summary>
    public void HandlePlayerLoseI(bool isPlayer1Dead)
    {
        isLastPlayerDeadIsPlayer1 = isPlayer1Dead;

        UpdateScore(isPlayer1Dead);

        if (ShouldPauseGame(isPlayer1Dead))
        {
            PauseGame();
            return;
        }

        ResetRound(isPlayer1Dead);

        UpdateCharactersStats();
    }

    private void UpdateScore(bool isPlayer1Dead)
    {
        // Changing the money that will be given to each players corresponding to who loses
        _moneyToGiveToPlayer1 = isPlayer1Dead ? 5 : 10;
        _moneyToGiveToPlayer2 = isPlayer1Dead ? 10 : 5;

        player1CharacterMoney.AddMoney(_moneyToGiveToPlayer1);
        player2CharacterMoney.AddMoney(_moneyToGiveToPlayer2);
    }

    private bool ShouldPauseGame(bool isPlayer1Dead)
    {
        // 
        int _roundWinnerScorePoints = isPlayer1Dead ? player2ScorePoint : player1ScorePoint;

        return isGameRunning && (_roundWinnerScorePoints < _pointsNeededToWin - 1);
    }

    private void PauseGame()
    {
        isGameRunning = false;
        Time.timeScale = 0;

        foreach (Unit unit in player1GridManager.unitList)
        {
            unit.GetComponent<BoxCollider2D>().enabled = false;
        }

        foreach (Unit unit in player2GridManager.unitList)
        {
            unit.GetComponent<BoxCollider2D>().enabled = false;
        }
        S_UnitCallButtonHandler.Instance.player1UnitCall.enabled = false;
        S_UnitCallButtonHandler.Instance.player2UnitCall.enabled = false;

        S_SkillTreeHandler.Instance.player1SkillTree.SetActive(true);
    }

    private void ResetRound(bool isPlayer1Dead)
    {
        S_RemoveUnit.Instance.RemoveAllUnits();
        _unitCallButtonHandler.CallUnitsForAllPlayers();

        int mapIndexModifier = (_currentGameMode == S_GameModeInvoker.GameModes.Domination) ? 0 : _loseCoefficient;

        UpdateScores(isPlayer1Dead, mapIndexModifier);

        currentTurn = TurnEmun.TransitionTurn;
        _loseCoefficient++;
    }

    private void UpdateScores(bool isPlayer1Dead, int mapIndexModifier)
    {
        if (_currentGameMode == S_GameModeInvoker.GameModes.Domination)
        {
            UpdateScoresInDominationMode(isPlayer1Dead);
        }
        else
        {
            if (isPlayer1Dead)
            {
                player2ScorePoint++;
                _mapIndex += mapIndexModifier;
            }
            else
            {
                player1ScorePoint++;
                _mapIndex -= mapIndexModifier;
            }
        }
    }

    private void UpdateScoresInDominationMode(bool isPlayer1Dead)
    {
        if (isPlayer1Dead)
        {
            if (player1ScorePoint > 0) 
                player1ScorePoint--;
            else 
                player2ScorePoint++;
        }
        else
        {
            if (player2ScorePoint > 0) 
                player2ScorePoint--;
            else 
                player1ScorePoint++;
        }
    }

    private void UpdateCharactersStats()
    {
        // Updates the character's score visuals
        player1CharacterHealth.RecieveScoreInfo(player1ScorePoint, true);
        player2CharacterHealth.RecieveScoreInfo(player2ScorePoint, false);

        player1CharacterHealth.ResetHealthStats();
        player2CharacterHealth.ResetHealthStats();

        // Reset players character's stats
        player1CharacterAdrenaline.ResetAdrenalineStats();
        player2CharacterAdrenaline.ResetAdrenalineStats();
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
            foreach (Unit unit in gridManager.unitList)
            {
                unit.ReturnToBaseTile();
                if (unit.state == 3)
                {
                    unit.freeze.SetActive(false);
                    unit.state = 0;
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

    /// <summary> Reduces the number of Action Points of the player playing by one </summary>
    public void ReduceActionPointBy1()
    {
        playerActionNumber -= 1;

        if (currentTurn == TurnEmun.Player1Turn)
        {
            // Detect in the player1 grid if there are at least three units that are aligned vertically or horizontally
            player1unitManager.UnitCombo(3);

            //We now check if the action of removing a unit created a combo, if yes then we cancel the decrease of actionUnitPoint
            if (S_RemoveUnit.Instance.NbCombo < player1unitManager.UnitColumn.Count && S_RemoveUnit.Instance.removing)
            {
                playerActionNumber +=1;
            }
        }
        else if (currentTurn == TurnEmun.Player2Turn)
        {
            // Detect in the player2 grid if there are at least three units that are aligned vertically or horizontally
            player2unitManager.UnitCombo(3);

            //We now check if the action of removing a unit created a combo, if yes then we cancel the decrease of actionUnitPoint
            if (S_RemoveUnit.Instance.NbCombo < player2unitManager.UnitColumn.Count && S_RemoveUnit.Instance.removing)
            {
                playerActionNumber+=1;
            }
            if (playerActionNumber > 0)
            {
                if (S_CrossSceneDataManager.Instance.vsIA)
                {
                    StartCoroutine(gameObject.GetComponent<S_RasputinIATree>().LaunchIa());
                }
            }
        }
        if(S_RemoveUnit.Instance.removing)
        {
            S_RemoveUnit.Instance.removing = false;
        }


        _unitCallButtonHandler.player1UnitCall.CallAmountUpdate();
        _unitCallButtonHandler.player2UnitCall.CallAmountUpdate();

        // Action time cooldown
        StartCoroutine(LaunchActionCooldown());


        // Ends the player who played's turn if he doeasn't have any action points left
        if (playerActionNumber <= 0)
        {
            EndTurn();
            if (S_CrossSceneDataManager.Instance.vsIA && currentTurn == TurnEmun.Player2Turn)
            {
                StartCoroutine(gameObject.GetComponent<S_RasputinIATree>().LaunchIa());
            }
        }
    }

    public void IncreaseActionPointBy1()
    {
        playerActionNumber += 1;
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

    public void ReduceSwapCounter(bool p_isPlayer1Affected)
    {
        if (p_isPlayer1Affected)
        {
            swapCounterP1--;
        }
        else
        {
            swapCounterP2--;
        }
    }
    #endregion
}