using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_GameManager : MonoBehaviour
{
    [Header("Background references :")]
    public List<Sprite> mapSelection = new(new Sprite[5]);
    [SerializeField] private Image _currentSprite; // sprite that display the map 

    [Header("Turn references :")]
    [SerializeField] private TextMeshProUGUI _timerText;
    
    [Header("Character's stats references :")]
    [SerializeField] S_CharacterStats _character1Stats;
    [SerializeField] S_CharacterStats _character2Stats;

    private float _targetTime;
    private bool _isPlayer1Turn = true;
    
    private int _player1Point;
    private int _player2Point;
    private Sprite _currentMap;
    
    private int _intMap = 2;// int for the current time in the list


    private void Start()
    {
        _targetTime = 3f;
        _timerText.text = _targetTime.ToString();
        _currentMap = mapSelection[_intMap];  // set the current map on start
        _currentSprite.sprite = mapSelection[_intMap]; // set the current sprite on start

        S_CharacterManager.Instance.SpawnCharacter(_character1Stats, true);
        S_CharacterManager.Instance.SpawnCharacter(_character2Stats, false);
    }


    private void Update()
    {
        _targetTime -= Time.deltaTime; //decrease the timer 
        _timerText.text = ((int)_targetTime).ToString(); // display the timer in a text and rounded off in seconds

        if (_targetTime <= 0.0f) //check if the timer is equal or less to 0
        {
            TimerEnded();
        }
    }


    /// <summary> Change the map when the player lose a point or win a point and add a point to the player 1 or 2 </summary>
    /// <param name="p_isPlayer1Dead"></param>
    public void HandlePlayerLose(bool p_isPlayer1Dead)
    {
        if (p_isPlayer1Dead) 
        {
            _player1Point += 1;
            _intMap -= 1;
        }
        else
        {
            _player2Point += 1;
            _intMap += 1;
        }

        _currentMap = mapSelection[_intMap];
        _currentSprite.sprite = mapSelection[_intMap];

        // TODO : Show a UI of defeat to give feedback to the player
    }

    private void TimerEnded() // change the turn of the player and reset the timer to 60s
    {
        if (_isPlayer1Turn)
        {
            _isPlayer1Turn = false;
        }
        else if (!_isPlayer1Turn)
        {
            _isPlayer1Turn = true;
        }

        _targetTime = 60.0f;
    }
}