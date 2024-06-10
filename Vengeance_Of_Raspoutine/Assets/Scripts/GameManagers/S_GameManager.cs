using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

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
                _player1ActionPreventerPanel.SetActive(false);
                _player2ActionPreventerPanel.SetActive(true);

                // Change the player turn text to the corresponding value
                _playerTurnText.text = "Player 1 turn";

                StartTurnCheckUnit();

                DeactivateGrid();

                ResetActionPoint();
            }
            else if (_currentTurn == TurnEmun.Player2Turn)
            {
                gameObject.GetComponent<S_RasputinIATree>().CallTree();

                // Same as for the player1 but for the player2

                isPlayer1Turn = false;

                _currentRoundNumber++;

                player1UnitCallButton.interactable = false;
                player2UnitCallButton.interactable = true;

                _player1ActionPreventerPanel.SetActive(true);
                _player2ActionPreventerPanel.SetActive(false);

                _playerTurnText.text = "Player 2 turn";

                StartTurnCheckUnit();

                DeactivateGrid();

                ResetActionPoint();
            }
            else if (_currentTurn == TurnEmun.TransitionTurn)
            {
                // In case if this turn is too long we diseable all possible interactions for all players

                player1UnitCallButton.interactable = false;
                player2UnitCallButton.interactable = false;
                DeactivateGrid();
                StartTurnCheckUnit();
                
                // NOTE : You can uncomment the code below if you see that the player can, for exemple, grap a unit after he finish his turn
                // (WARNING THO : That make the screen flash thats why it's currently commented)

                //_player1ActionPreventerPanel.SetActive(true);
                //_player2ActionPreventerPanel.SetActive(true);
            }
            else
            {
                Debug.LogError(
                    "ERROR ! You tryed to change the variable '" + currentTurn.ToString() + "' to '" + value.ToString() + 
                    "' but it's not planned into the variable's code. UNITY IS PAUSED !"
                );
                Debug.Break();
                return;
            }

            _targetTime = 60.0f;
        }
    }

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
                _endMenu.WhoWin(true);
                return;
            }

            if (player2ScorePoint >= _pointsNeededToWin)
            {
                _endMenu.WhoWin(false);
                return;
            }

            // Updating the map according to the players points
            _gameBackgroundSpriteRenderer.sprite = mapSelection[__mapIndex];
        }
    }

    #endregion

    [Header("Player's inputs GameObject references :")]
    public GameObject player1Inputs;
    public GameObject player2Inputs;

    [Header("UnitCall buttons :")]
    public Button player1UnitCallButton;
    public Button player2UnitCallButton;

    [Header("Unit manager's references")]
    public S_UnitManager unitManagerP1;
    public S_UnitManager unitManagerP2;

    [Header("Background references :")]
    [SerializeField] private SpriteRenderer _gameBackgroundSpriteRenderer;
    public List<Sprite> mapSelection = new(new Sprite[5]);

    [Header("Panel references :")]
    [SerializeField] private GameObject _player1ActionPreventerPanel;
    [SerializeField] private GameObject _player2ActionPreventerPanel;

    [Header("Turn references :")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _playerTurnText;
    [SerializeField] private TextMeshProUGUI _turnsText;
    [SerializeField] private TextMeshProUGUI _actionsText;

    [Header("Characters stats's references :")]
    [SerializeField] S_CharacterStats _character1Stats;
    [SerializeField] S_CharacterStats _character2Stats;

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

    // Game's game mode
    S_GameModeInvoker.GameModes _currentGameMode;

    int _pointsNeededToWin = 3;

    float _targetTime;
    int _currentRoundNumber = 0;
    int _playerActionNumber;

    int _loseCoefficient = 1;
    #endregion

    #region Methods

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndDestructionOfTheSecondOne);
    }

    private void Start()
    {
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
            _targetTime -= Time.deltaTime;

            // Display the rounded timer in seconds in a text
            _timerText.text = "Remaining time : " + ((int)_targetTime).ToString();

            // Display the current round number
            _turnsText.text = "Turn : " + _currentRoundNumber.ToString();

            // Display the player's number of action left he have 
            _actionsText.text = "Remaining actions : " + _playerActionNumber;

            // Check if the timer is equal or less to 0, if yes then end the turn
            if (_targetTime <= 0.0f)
            {
                EndTurn();
            }
        }
    }

    /// <summary> Randomly determine the player who will play first in the initial turn. </summary>
    private void RandomStartTurn()
    {
        // Take a random number beetween 0 and 2 (2 excluded)
        int randomNumber = Random.Range(0, 2);

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
                currentTurn=TurnEmun.Player1Turn;
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

    /// <summary> Change the map when the player lose a point or win a point and add a point to the player 1 or 2
    /// and checks if the player 1 or 2 wins </summary>
    public void HandlePlayerLose(bool p_isPlayer1Dead)
    {
        // Add a score point to the player who won, let the player who lost play the first in the new round,
        // change the map progression according to the entire game 

        if (_currentGameMode == S_GameModeInvoker.GameModes.Domination)
        {
            // If player2 won
            if (p_isPlayer1Dead)
            {
                // If the player2 have the advantage
                if (player1ScorePoint > 0)
                {
                    player1ScorePoint--;
                }
                else
                {
                    player2ScorePoint++;
                }

                currentTurn = TurnEmun.Player1Turn;

                _mapIndex += 1;

                player2CharacterMoney.AddMoney(10);
                player1CharacterMoney.AddMoney(5);
            }
            // If player1 won
            else
            {
                // If the player1 have the advantage and player2 won
                if (player2ScorePoint > 0)
                {
                    player2ScorePoint--;
                }
                else
                {
                    player1ScorePoint++;
                }

                currentTurn = TurnEmun.Player2Turn;

                _mapIndex -= 1;

                player2CharacterMoney.AddMoney(5);
                player1CharacterMoney.AddMoney(10);
            }
        }
        // If we are in other game mode "Classic", "Sudden Death"
        else
        {
            if (p_isPlayer1Dead)
            {
                player2ScorePoint++;

                currentTurn = TurnEmun.Player1Turn;

                _mapIndex += 1 * _loseCoefficient;

                player2CharacterMoney.AddMoney(10);
                player1CharacterMoney.AddMoney(5);
            }
            else
            {
                player1ScorePoint++;

                currentTurn = TurnEmun.Player2Turn;

                _mapIndex -= 1 * _loseCoefficient;

                player2CharacterMoney.AddMoney(5);
                player1CharacterMoney.AddMoney(10);
            }

            if (_currentGameMode == S_GameModeInvoker.GameModes.Classic)
                _loseCoefficient++;
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
    }

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
                Debug.LogError("Player number incorrect");
                break;
        }
    }

    public void StartTurnCheckUnit()
    {
        // To avoid having to manage two grid manager variables
        // we create a local variable nammed "gridManager" it contain the S_GridManager we will use later,
        // this variable will change depending if it's player1 turn and vice versa. 
        S_GridManager gridManager = _player1GridManager;


        if (isPlayer1Turn)
        {
            gridManager = _player1GridManager;
        }
        else
        {
            gridManager = _player2GridManager;
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

    public void ResetActionPoint()
    {
        _playerActionNumber = 3;
    }

    public void ReduceActionPointBy1()
    {
        _playerActionNumber -= 1;

        if (currentTurn == TurnEmun.Player1Turn)
        {
            unitManagerP1.UnitCombo(3);
        }
        else
        {
            if (_playerActionNumber > 0)
            {
                gameObject.GetComponent<S_RasputinIATree>().CallTree();
            }

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
                if (currentTurn == TurnEmun.Player1Turn)
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
        if (currentTurn == TurnEmun.Player1Turn)
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
        else if (currentTurn == TurnEmun.Player2Turn)
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
        else
        {
            foreach (Unit unit in _player1GridManager.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = true;
            }
            foreach (Unit unit in _player2GridManager.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
    #endregion
}