using System;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_CharacterHealth : MonoBehaviour
{
    #region Struct

    [Serializable]
    struct ScoreImages
    {
        public Image score1Image;
        public Image score2Image;
        public Image score3Image;
    }
    #endregion

    #region Variables

    [SerializeField] ScoreImages _scoreImages;

    [Header("References :")]
    [SerializeField] Image _healthBarFillSprite;
    [SerializeField] TextMeshProUGUI _healthText;

    S_GameManager _gameManager;

    bool _isPlayer1Character;
    int _maxHP;
    int _currentHP;

    // Cheat code's part
    bool _areCheatCodesEnable;
    S_GameManager.StatChangementCheatCodes _healthStatsChangementCheatCodes;

    Sprite _emptyScorePoint;
    Sprite _scorePointFilled;
    #endregion

    #region Getter / Setter
    public int currentHP
    {
        get { return _currentHP; }
        set
        {
            if (value < _currentHP)
            {
                if (_isPlayer1Character)
                    _gameManager.player1CharacterAdrenaline.currentAdrenaline += _currentHP - value;
                    
                else
                    _gameManager.player2CharacterAdrenaline.currentAdrenaline += _currentHP - value;
            }

            _currentHP = value;

            if (_currentHP <= 0)
            {
                _currentHP = 0;
                CharacterDie();
            }
            else if (_currentHP > _maxHP)
            {
                _currentHP = _maxHP;
            }

            // Update the HP bar, and the HP text
            UpdateHealthUIs();
        }
    }

    public int maxHP
    {
        get { return _maxHP; }
    }
    #endregion

    #region Methods

    private void Start()
    {
        _gameManager = S_GameManager.Instance;
    }

    public void RecieveCharacterHealthStats(
        int p_maxHP,
        bool p_isPlayer1Character,
        Sprite p_emptyScorePoint,
        Sprite p_scorePointFilled,

        // Cheat code's part
        bool p_areCheatCodesEnable,
        S_GameManager.StatChangementCheatCodes p_healthStatsChangementCheatCodes)
    {
        // Setting up class variables
        _maxHP = p_maxHP;
        _isPlayer1Character = p_isPlayer1Character;
        _emptyScorePoint = p_emptyScorePoint;
        _scorePointFilled = p_scorePointFilled;

        _healthStatsChangementCheatCodes = p_healthStatsChangementCheatCodes;
        _areCheatCodesEnable = p_areCheatCodesEnable;

        // Setting up character HP to the max
        currentHP = _maxHP;
    }

    /// <summary> Updates the character's score visuals </summary>
    public void RecieveScoreInfo(int p_score, bool p_isPlayer1Character)
    {
        // Security
        if (p_isPlayer1Character != _isPlayer1Character || !p_isPlayer1Character != !_isPlayer1Character)
        {
            return;
        }

        // Changing character's score visuals according to the "p_score" parameter
        switch (p_score)
        {
            case 0:
                _scoreImages.score1Image.sprite = _emptyScorePoint;
                _scoreImages.score2Image.sprite = _emptyScorePoint;
                _scoreImages.score3Image.sprite = _emptyScorePoint;
                break;

            case 1:
                _scoreImages.score1Image.sprite = _scorePointFilled;
                _scoreImages.score2Image.sprite = _emptyScorePoint;
                _scoreImages.score3Image.sprite = _emptyScorePoint;
                break;

            case 2:
                _scoreImages.score1Image.sprite = _scorePointFilled;
                _scoreImages.score2Image.sprite = _scorePointFilled;
                _scoreImages.score3Image.sprite = _emptyScorePoint;
                break;

            case 3:
                _scoreImages.score1Image.sprite = _scorePointFilled;
                _scoreImages.score2Image.sprite = _scorePointFilled;
                _scoreImages.score3Image.sprite = _scorePointFilled;
                break;

            default:
                break;
        }
    }

    public void ResetHealthStats()
    {
        currentHP = maxHP;
    }

    void UpdateHealthUIs()
    {
        // Updating the text
        _healthText.text = _currentHP + " / " + _maxHP + " HP";

        // Updating the bar

        // Note : We use a float cast "(float)" to avoid having a division of two int values,
        // if that was the case the division can't return us a float
        _healthBarFillSprite.fillAmount = (float)_currentHP / _maxHP; 
    }

    void CharacterDie()
    {
        S_GameManager.Instance.HandlePlayerLose(_isPlayer1Character);
    }

    // -- TO DEBUG -- //
    #region TO DEBUG
    private void Update()
    {
        if (_areCheatCodesEnable)
        {
            if (_isPlayer1Character)
            {
                if (Input.GetKeyDown(_healthStatsChangementCheatCodes.firstPlayer1StatsChangement.input))
                    currentHP += _healthStatsChangementCheatCodes.firstPlayer1StatsChangement.statChangementValue;

                if (Input.GetKeyDown(_healthStatsChangementCheatCodes.secondPlayer1StatsChangement.input))
                    currentHP += _healthStatsChangementCheatCodes.secondPlayer1StatsChangement.statChangementValue;
            }
            else
            {
                if (Input.GetKeyDown(_healthStatsChangementCheatCodes.firstPlayer2StatsChangement.input))
                    currentHP += _healthStatsChangementCheatCodes.firstPlayer2StatsChangement.statChangementValue;

                if (Input.GetKeyDown(_healthStatsChangementCheatCodes.secondPlayer2StatsChangement.input))
                    currentHP += _healthStatsChangementCheatCodes.secondPlayer2StatsChangement.statChangementValue;
            }
        }
    }
    #endregion
    #endregion
}