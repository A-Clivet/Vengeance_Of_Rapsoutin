using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_GameManager : MonoBehaviour
{
    public static S_GameManager Instance;

    public int player1Point { get; private set; }
    public int player2Point { get; private set; }

    [Header("Background references :")]
    public List<Sprite> mapSelection = new(new Sprite[5]);
    [SerializeField] private Image _currentSprite; // sprite that display the map 

    [Header("Turn references :")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _playerTurnText;
    [SerializeField] private TextMeshProUGUI _turnsText;
    
    [Header("Character's stats references :")]
    [SerializeField] S_CharacterStats _character1Stats;
    [SerializeField] S_CharacterStats _character2Stats;

    [Header("Player 1 and player 2 end screen :")]
    [SerializeField] private GameObject _player1EndScreen;
    [SerializeField] private GameObject _player2EndScreen;

    private float _targetTime;
    private int _currentTurnNumber;
    private int _playerActionNumber;
    private bool _randomTurn;
    private bool _isPlayer1Turn = true;
    
    private Sprite _currentMap;
    
    private int _intMap = 2;    // int for the current time in the list

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.DestructionOfTheSecondOne); 
        DontDestroyOnLoad(transform.parent);
    }

    private void Start()
    {
        _targetTime = 3f;
        _currentTurnNumber = 1;
        _playerActionNumber = 3;
        _timerText.text = _targetTime.ToString();
        _currentMap = mapSelection[_intMap];  // set the current map on start
        _currentSprite.sprite = mapSelection[_intMap]; // set the current sprite on start

        _player1EndScreen.SetActive(false);
        _player2EndScreen.SetActive(false);

        S_CharacterManager.Instance.SpawnCharacter(_character1Stats, true);
        S_CharacterManager.Instance.SpawnCharacter(_character2Stats, false);
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

        if(_isPlayer1Turn) 
        {
            _playerTurnText.text = "Player 1 Turn";
        }

        else if (!_isPlayer1Turn)
        { 
            _playerTurnText.text = "Player 2 Turn";
        }
    }


    /// <summary> Change the map when the player lose a point or win a point and add a point to the player 1 or 2
    ///           and checks if the player 1 or 2 wins </summary>
    /// <param name="p_isPlayer1Dead"></param>
    public void HandlePlayerLose(bool p_isPlayer1Dead)
    {
        if (p_isPlayer1Dead) 
        {
            player1Point += 1;
            if (player1Point >= 1)
            {
                _randomTurn = true;
            }
            if (_randomTurn)
            {
                int random = Random.Range(0, 1);
                switch (random)
                {
                    case 0:
                        _isPlayer1Turn = false;
                        break;
                    case 1:
                        _isPlayer1Turn = true;  
                        break;
                }
            }
            _intMap -= 1;
        }
        else
        {
            player2Point += 1;
            if (player2Point >= 1)
            {
                _randomTurn = true;
            }
            if (_randomTurn)
            {
                int random = Random.Range(0, 1);
                switch (random)
                {
                    case 0:
                        _isPlayer1Turn = false;
                        break;
                    case 1:
                        _isPlayer1Turn = true;
                        break;
                }
            }
            _intMap += 1;
        }

        if (player1Point >= 3)
        {
            _player1EndScreen.SetActive(true);
        }

        if (player2Point >= 3)
        {
            _player2EndScreen.SetActive(true);
        }

        _currentMap = mapSelection[_intMap];
        _currentSprite.sprite = mapSelection[_intMap];

    }

    public void EndTurn() // change the turn of the player and reset the timer to 60s and adds 1 to the current round number
    {
        if (_isPlayer1Turn)
        {
            _isPlayer1Turn = false;
            
        }
        else if (!_isPlayer1Turn)
        {
            _isPlayer1Turn = true;
            _currentTurnNumber += 1;
        }
        _targetTime = 60.0f;
        _playerActionNumber = 3;
    }

    public void ReduceActionPointBy1()
    {
        _playerActionNumber -= 1;

        if (_playerActionNumber <= 0)
        {
            EndTurn();
        }
    }

    public void IncreaseActionPointBy1()
    {
        _playerActionNumber += 1;
    }
}