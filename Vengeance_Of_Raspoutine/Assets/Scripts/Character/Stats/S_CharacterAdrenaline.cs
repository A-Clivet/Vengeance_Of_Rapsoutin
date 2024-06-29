using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class S_CharacterAdrenaline : MonoBehaviour
{
    #region Variables
    [Header("References :")]
    [SerializeField] Image _specialCapacityChargingSprite;
    [SerializeField] Image _specialCapacityIcon;
    [SerializeField] Button _speacialCapacityButton;

    [Header("Stats :")]
    [SerializeField] Color _initialSpecialCapacityIconColor;
    [SerializeField] Color _newSpecialCapacityIconColor;

    S_GameManager _gameManager;

    bool _isPlayer1Character;
    int _currentAdrenaline;
    int _maxAdrenaline;

    // Cheat code's part
    bool _areCheatCodesEnable;
    S_GameManager.StatChangementCheatCodes _adrenalineStatsChangementCheatCodes;

    Coroutine _launchSpecialCapacityCoroutine;

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

    public S_SpecialCapacityStats specialCapacity { get; set; }
    #endregion

    #endregion

    #region Methods
    private void Start()
    {
        _gameManager = S_GameManager.Instance;
    }

    public void RecieveCharacterAdrenalineStats(
        int p_maxAdrenaline,
        S_SpecialCapacityStats p_specialCapacity,
        bool p_isPlayer1Character,

        // Cheat code part
        bool p_areCheatCodesEnable,
        S_GameManager.StatChangementCheatCodes p_adrenalineStatsChangementCheatCodes)
    {
        // Setting up class variables
        _maxAdrenaline = p_maxAdrenaline;
        specialCapacity = p_specialCapacity;
        _isPlayer1Character = p_isPlayer1Character;

        _areCheatCodesEnable = p_areCheatCodesEnable;
        _adrenalineStatsChangementCheatCodes = p_adrenalineStatsChangementCheatCodes;

        // Setting up character Adrenaline to 0
        currentAdrenaline = 0;
    }

    /// <summary> Use to enable / disable special capacity button's interaction </summary>
    public void RecieveNewTurnInfo(bool p_isPlayer1Turn)
    {
        if (p_isPlayer1Turn == _isPlayer1Character || !p_isPlayer1Turn == !_isPlayer1Character)
            _speacialCapacityButton.interactable = true;
        else
            _speacialCapacityButton.interactable = false;

        // Changing the special capacity icon to the good one, if it was not  
        if (_specialCapacityIcon.sprite != specialCapacity.capacitySprite)
            _specialCapacityIcon.sprite = specialCapacity.capacitySprite;
    }

    /// <summary> This function is built to be used when the special capacity's button is pressed,
    /// it checks if the character associated with the button has full adrenaline, if yes then it launch the character's special capacity </summary>
    public void SpecialCapacityButtonPressed()
    {
        // Check if the character has full adrenaline
        if (currentAdrenaline == _maxAdrenaline)
        {
            // In the case that we destroy enemy's units (witch will gave us action points) we save the actual action point number
            int previusActionPointsNumber = _gameManager.playerActionNumber;

            currentAdrenaline = 0;

            S_GridManagersHandler.Instance.HandleAllUnitInteractions(false);

            // Launch special capacity
            _launchSpecialCapacityCoroutine = StartCoroutine(S_SpecialCapacityManager.Instance.LaunchSpecialCapacity(specialCapacity, _isPlayer1Character));

            if (!specialCapacity.isUltimateSpecialCapacity)
            {
                // Waits until the coroutine given ends
                StartCoroutine(SpecialCapacityCooldown(previusActionPointsNumber));
            }
        }
    }

    /// <summary> Waits until the coroutine "LaunchSpecialCapacity" ends, and reset correctly player's grid, and set the correct action point number </summary>
    IEnumerator SpecialCapacityCooldown(int p_previusActionPointsNumber)
    {
        yield return _launchSpecialCapacityCoroutine;

        // Will reactivate grids depending on the player who plays
        _gameManager.DeactivateGrid();

        // If we don't set directly the value into the variable is to pass through verifications
        _gameManager.playerActionNumber = p_previusActionPointsNumber;

        _gameManager.ReduceActionPointBy1();
    }

    public void ResetAdrenalineStats()
    {
        currentAdrenaline = 0;
    }

    void UpdateAdrenalineUIs()
    {
        // Updating the special capacity charging sprite

        // Note : We use a float cast "(float)" to avoid having a division of two int values,
        // if that was the case the division can't return us a float
        _specialCapacityChargingSprite.fillAmount = (float)_currentAdrenaline / _maxAdrenaline;

        // Changing the special capacity icon color
        if (currentAdrenaline == maxAdrenaline)
            _specialCapacityIcon.color = _newSpecialCapacityIconColor;
        else
            _specialCapacityIcon.color = _initialSpecialCapacityIconColor;
    }

    // -- Cheat codes -- //
    private void Update()
    {
        if (_areCheatCodesEnable)
        {
            if (_isPlayer1Character)
            {
                if (Input.GetKeyDown(_adrenalineStatsChangementCheatCodes.firstPlayer1StatsChangement.input))
                    currentAdrenaline += _adrenalineStatsChangementCheatCodes.firstPlayer1StatsChangement.statChangementValue;

                if (Input.GetKeyDown(_adrenalineStatsChangementCheatCodes.secondPlayer1StatsChangement.input))
                    currentAdrenaline += _adrenalineStatsChangementCheatCodes.secondPlayer1StatsChangement.statChangementValue;
            }
            else
            {
                if (Input.GetKeyDown(_adrenalineStatsChangementCheatCodes.firstPlayer2StatsChangement.input))
                    currentAdrenaline += _adrenalineStatsChangementCheatCodes.firstPlayer2StatsChangement.statChangementValue;

                if (Input.GetKeyDown(_adrenalineStatsChangementCheatCodes.secondPlayer2StatsChangement.input))
                    currentAdrenaline += _adrenalineStatsChangementCheatCodes.secondPlayer2StatsChangement.statChangementValue;
            }
        }
    }
    #endregion
}