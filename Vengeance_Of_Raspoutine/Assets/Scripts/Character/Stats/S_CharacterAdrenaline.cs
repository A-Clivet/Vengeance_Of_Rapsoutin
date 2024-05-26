using UnityEngine;
using UnityEngine.UI;

public class S_CharacterAdrenaline : MonoBehaviour
{
    #region Variables
    [Header("References :")]
    [SerializeField] Image _specialCapacityChargingSprite;

    bool _isPlayer1Character;
    S_SpecialCapacityStats _specialCapacity;
    int _currentAdrenaline = 0;
    int _maxAdrenaline = 50;

    #region Getter / Setter
    public int currentAdrenaline
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

    public int maxAdrenaline
    {
        get { return _maxAdrenaline; }
    }
    #endregion

    #endregion

    #region Methods

    public void RecieveCharacterAdrenalineStats(int p_maxAdrenaline, S_SpecialCapacityStats p_specialCapacity, bool p_isPlayer1Character)
    {
        // Setting up class variables
        _maxAdrenaline = p_maxAdrenaline;
        _specialCapacity = p_specialCapacity;
        _isPlayer1Character = p_isPlayer1Character;

        // Setting up character Adrenaline to the max
        currentAdrenaline = _maxAdrenaline;
    }

    /// <summary> This function is built to be used when the special capacity's button is pressed,
    /// it checks if the character associated with the button has full adrenaline, if yes then it launch the character's special capacity </summary>
    public void SpecialCapacityButtonPressed()
    {
        // Check if the character has full adrenaline
        if (currentAdrenaline == _maxAdrenaline)
        {
            currentAdrenaline = 0;

            // Launch special capacity
            StartCoroutine(S_SpecialCapacityManager.Instance.LaunchSpecialCapacity(_specialCapacity, _isPlayer1Character));
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
    #region TO DEBUG
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            currentAdrenaline += 5;

        if (Input.GetKeyDown(KeyCode.J))
            currentAdrenaline -= 5;
    }
    #endregion
    #endregion
}