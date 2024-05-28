using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.UI;

public class S_GameManager : MonoBehaviour
{
    public static S_GameManager Instance;

    public int player1Point { get; private set; }
    public int player2Point { get; private set; }
    public bool isPlayer1Turn { get; private set; } = true;

    [Header("Background references :")]
    public List<Sprite> mapSelection = new(new Sprite[5]);
    [SerializeField] private Image _currentSprite; // sprite that display the map 

    [Header("Panel references :")]
    [SerializeField] private GameObject _panelPlayer1;
    [SerializeField] private GameObject _panelPlayer2;

    [Header("UnitCall buttons :")]
    [SerializeField] private GameObject _player1Unitcall;
    [SerializeField] private GameObject _player2Unitcall;

    [Header("Turn references :")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _playerTurnText;
    [SerializeField] private TextMeshProUGUI _turnsText;

    [Header("Character's references :")]
    [SerializeField] S_CharacterManager _characterManager;
    [SerializeField] S_CharacterStats _character1Stats;
    [SerializeField] S_CharacterStats _character2Stats;
    [SerializeField] public S_UnitManager unitManagerP1;
    [SerializeField] public S_UnitManager unitManagerP2;

    [Header("Player 1 and player 2 end screen :")]
    [SerializeField] private GameObject _player1EndScreen;
    [SerializeField] private GameObject _player2EndScreen;

    [Header("Cooldown between actions :")]
    [SerializeField] private float _cooldown;

    [Header("player Inputs :")]
    [SerializeField] private GameObject _playerInput1;
    [SerializeField] private GameObject _playerInput2;

    [Header("Player's grid :")]
    [SerializeField] private S_GridManager _player1Grid;
    [SerializeField] private S_GridManager _player2Grid;

    // Character's adrenaline script references
    S_CharacterAdrenaline _player1CharacterAdrenaline;
    S_CharacterAdrenaline _player2CharacterAdrenaline;

    // Character's health script references
    public S_CharacterHealth _player1CharacterHealth;
    public S_CharacterHealth _player2CharacterHealth;

    private float _targetTime;
    private int _currentTurnNumber;
    private int _playerActionNumber;


    private Sprite _currentMap;
    
    private int _intMap = 2;    // int for the current time in the list

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.DestructionOfTheSecondOne); 
        DontDestroyOnLoad(transform.parent);
    }

    private void Start()
    {

        _targetTime = 30f;
        _currentTurnNumber = 1;
        _playerActionNumber = 3;
        _timerText.text = _targetTime.ToString();
        _currentMap = mapSelection[_intMap];  // set the current map on start
        _currentSprite.sprite = mapSelection[_intMap]; // set the current sprite on start

        _player1EndScreen.SetActive(false);
        _player2EndScreen.SetActive(false);

        RandomStartTurn();
        
        #region Characters management

        // Setting up character manager reference
        _characterManager = S_CharacterManager.Instance;

        // Creating the player's character
        _characterManager.SpawnCharacter(_character1Stats, true);
        _characterManager.SpawnCharacter(_character2Stats, false);

        // Setting up character's adrenaline and health script references
        _player1CharacterAdrenaline = _characterManager.player1CharacterGameObject.GetComponent<S_CharacterAdrenaline>();
        _player2CharacterAdrenaline = _characterManager.player2CharacterGameObject.GetComponent<S_CharacterAdrenaline>();

        _player1CharacterHealth = _characterManager.player1CharacterGameObject.GetComponent<S_CharacterHealth>();
        _player2CharacterHealth = _characterManager.player2CharacterGameObject.GetComponent<S_CharacterHealth>();

        // Enable / disable special capacity button's interaction
        _player1CharacterAdrenaline.RecieveNewTurnInfo(isPlayer1Turn);
        _player2CharacterAdrenaline.RecieveNewTurnInfo(isPlayer1Turn);

        // Updates the character's score visuals
        _player1CharacterHealth.RecieveScoreInfo(player1Point, true);
        _player2CharacterHealth.RecieveScoreInfo(player2Point, false);

        #endregion

        if (isPlayer1Turn)
        {
            _playerTurnText.text = "Player 1 Turn";
            _panelPlayer1.SetActive(false);
            _panelPlayer2.SetActive(true);
        }
        else if (!isPlayer1Turn)
        {
            _playerTurnText.text = "Player 2 Turn";
            _panelPlayer1.SetActive(true);
            _panelPlayer2.SetActive(false);
        }
    }


    private void Update()
    {
        _targetTime -= Time.deltaTime; //decrease the timer 
        _timerText.text = "Remaining Time : " + ((int)_targetTime).ToString(); // display the timer in a text and rounded off in seconds
        _turnsText.text = "Turns : " + _currentTurnNumber.ToString(); //display the current round number

        if (_targetTime <= 0.0f) //check if the timer is equal or less to 0
        {
            EndTurn();
        }
    }


    private void RandomStartTurn() // Allows to randomise wich player starts at the start of the game
    {
        int random = Random.Range(0, 2);

        switch (random)
        {
            case 0:
                isPlayer1Turn = false;
                _player1Unitcall.GetComponent<Button>().interactable = false;
                break;
            case 1:
                isPlayer1Turn = true;
                _player2Unitcall.GetComponent<Button>().interactable = false;
                break;
        }
    }
    

    /// <summary> 
    /// Change the map when the player lose a point or win a point and add a point to the player 1 or 2
    /// and checks if the player 1 or 2 wins 
    /// </summary>
    /// <param name="p_isPlayer1Dead"></param>
    public void HandlePlayerLose(bool p_isPlayer1Dead)
    {
        
        
        if (p_isPlayer1Dead) 
        {
            player2Point += 1;
            isPlayer1Turn = true;
            _intMap += 1;
        }
        else
        {
            player1Point += 1;
            isPlayer1Turn = false;
            _intMap -= 1;
        }

        if (player1Point >= 3)
        {
            _player1EndScreen.SetActive(true);
        }

        if (player2Point >= 3)
        {
            _player2EndScreen.SetActive(true);
        }

        #region Characters management
        // Updates the character's score visuals
        _player1CharacterHealth.RecieveScoreInfo(player1Point, true);
        _player2CharacterHealth.RecieveScoreInfo(player2Point, false);

        // Reset players character's stats
        _player1CharacterHealth.ResetHealthStats();
        _player2CharacterHealth.ResetHealthStats();

        _player1CharacterAdrenaline.ResetAdrenalineStats();
        _player2CharacterAdrenaline.ResetAdrenalineStats();
        #endregion

        _currentMap = mapSelection[_intMap];
        _currentSprite.sprite = mapSelection[_intMap];
    }

    public void EndTurn() // change the turn of the player and reset the timer to 60s and adds 1 to the current round number
    {
        if (isPlayer1Turn)
        {
            isPlayer1Turn = false;
            _playerTurnText.text = "Player 2 Turn";
            _panelPlayer1.SetActive(true);
            _player1Unitcall.GetComponent<Button>().interactable = false;
            _player2Unitcall.GetComponent<Button>().interactable = true;
            StartTurnCheckUnit();
            Deactivategrid(!isPlayer1Turn);
            _panelPlayer2.SetActive(false);
        }

        else if (!isPlayer1Turn)
        {
            isPlayer1Turn = true;
            _currentTurnNumber += 1;
            _playerTurnText.text = "Player 1 Turn";
            _panelPlayer1.SetActive(false);
            _player1Unitcall.GetComponent<Button>().interactable = true;
            _player2Unitcall.GetComponent<Button>().interactable = false;
            StartTurnCheckUnit();
            Deactivategrid(isPlayer1Turn);
            _panelPlayer2.SetActive(true);
        }
        
        _targetTime = 30.0f;
        _playerActionNumber = 3;

        // Enable / disable special capacity button's interaction
        _player1CharacterAdrenaline.RecieveNewTurnInfo(isPlayer1Turn);
        _player2CharacterAdrenaline.RecieveNewTurnInfo(isPlayer1Turn);
    }

    public void StartTurnCheckUnit()
    {
        if (S_GameManager.Instance.isPlayer1Turn)
        {
            for (int i = 0; i < _player1Grid.width; i++)
            {
                for (int j = 0; j < Mathf.Abs(_player1Grid.height); j++)
                {
                    if (_player1Grid.gridList[i][j].unit == null) continue;
                    _player1Grid.gridList[i][j].unit.AttackCharge();
                }
            }
        }
        else
        {
            for (int i = 0; i < _player2Grid.width; i++)
            {
                for (int j = 0; j < Mathf.Abs(_player2Grid.height); j++)
                {
                    if (_player2Grid.gridList[i][j].unit == null) continue;
                    _player2Grid.gridList[i][j].unit.AttackCharge();
                }
            }
        }

    }

    public void ReduceActionPointBy1()
    {
        _playerActionNumber -= 1;
        if (isPlayer1Turn)
        {
            unitManagerP1.UnitCombo(3);
        }
        else
        {
            unitManagerP2.UnitCombo(3);
        }
        StartCoroutine("Cooldown");
        if (_playerActionNumber <= 0)
        {
            EndTurn();
        }
    }

    public void IncreaseActionPointBy1()
    {
        _playerActionNumber += 1;
    }

    private IEnumerator Cooldown()
    {
        _playerInput1.SetActive(false);
        _playerInput2.SetActive(false);
        yield return new WaitForSecondsRealtime(_cooldown);
        _playerInput1.SetActive(true);
        _playerInput2.SetActive(true);
    }

    public void Deactivategrid(bool p_Playerturn)
    {
        if (S_GameManager.Instance.isPlayer1Turn)
        {
            for (int i = 0; i < _player1Grid.width; i++)
            {
                for (int j = 0; j < Mathf.Abs(_player1Grid.height); j++)
                {
                    _player1Grid.gridList[i][j].GetComponent<Collider2D>().enabled = true;
                    _player2Grid.gridList[i][j].GetComponent<Collider2D>().enabled = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < _player2Grid.width; i++)
            {
                for (int j = 0; j < Mathf.Abs(_player2Grid.height); j++)
                {
                    _player1Grid.gridList[i][j].GetComponent<Collider2D>().enabled = false;
                    _player2Grid.gridList[i][j].GetComponent<Collider2D>().enabled = true;
                }
            }
        }
    }
}