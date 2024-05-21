using UnityEngine;
using UnityEngine.UI;

public class S_CharacterAdrenaline : MonoBehaviour
{
    #region Variables
    [Header("References :")]
    [SerializeField] Image _specialCapacityChargingSprite;

    bool _isPlayer1Character;
    int _currentAdrenaline = 0;
    int _maxAdrenaline = 50;

    #region Getter / Setter
    public int CurrentAdrenaline
    {
        get { return _currentAdrenaline; }
        set
        {
            _currentAdrenaline = value;

            if (_currentAdrenaline <= 0)
            {
                _currentAdrenaline = 0;
            }
            else if (_currentAdrenaline > _maxAdrenaline)
            {
                _currentAdrenaline = _maxAdrenaline;
            }

            // Update the Adrenaline sprite size
            UpdateAdrenalineUIs();
        }
    }

    public int MaxAdrenaline
    {
        get { return _maxAdrenaline; }
    }
    #endregion

    #endregion

    #region Methods

    public void RecieveCharacterAdrenalineStats(int p_maxAdrenaline, bool p_isPlayer1Character)
    {
        // Setting up class variables
        _maxAdrenaline = p_maxAdrenaline;
        _isPlayer1Character = p_isPlayer1Character;

        // Setting up character HP to the max
        CurrentAdrenaline = _maxAdrenaline;
    }

    /// <summary> This function is built to be used when the special capacity's button is pressed,
    /// it checks if the character associated with the button has full adrenaline, if yes then it launch the character's special capacity </summary>
    public void SpecialCapacityButtonPressed()
    {
        // Check if the turn correspond  player who own of the button
        if (_isPlayer1Character)
        {

        }

        // Check if the character has full adrenaline
        if (_currentAdrenaline == _maxAdrenaline)
        {
            _currentAdrenaline = 0;

            // Launch special capacity
        }
    }

    void UpdateAdrenalineUIs()
    {
        // Updating the special capacity charging sprite

        // Note : We use a float cast "(float)" to avoid having a division of two int values,
        // if that was the case the division can't return us a float
        _specialCapacityChargingSprite.fillAmount = (float)_currentAdrenaline / _maxAdrenaline;
    }

    // -- TO DEBUG -- //
    private void Start()
    {
        CurrentAdrenaline = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            CurrentAdrenaline += 5;

        if (Input.GetKeyDown(KeyCode.J))
            CurrentAdrenaline -= 5;
    }
    #endregion
}