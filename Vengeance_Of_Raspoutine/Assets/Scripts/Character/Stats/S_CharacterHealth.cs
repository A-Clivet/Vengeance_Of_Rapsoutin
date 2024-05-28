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

    bool _isPlayer1Character;
    int _maxHP;
    int _currentHP;

    Sprite _emptyScorePoint;
    Sprite _scorePointFilled;
    #endregion

    #region Getter / Setter
    public int currentHP
    {
        get { return _currentHP; }
        set
        {
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

    public void RecieveCharacterHealthStats(int p_maxHP, bool p_isPlayer1Character, Sprite p_emptyScorePoint, Sprite p_scorePointFilled)
    {
        // Setting up class variables
        _maxHP = p_maxHP;
        _isPlayer1Character = p_isPlayer1Character;
        _emptyScorePoint = p_emptyScorePoint;
        _scorePointFilled = p_scorePointFilled;

        // Setting up character HP to the max
        currentHP = _maxHP;
    }

    /// <summary> Updates the character's score visuals </summary>
    public void RecieveScoreInfo(int p_score, bool p_isPlayer1Character)
    {
        // Security
        if (p_isPlayer1Character != _isPlayer1Character || !p_isPlayer1Character != !_isPlayer1Character)
        {
            Debug.LogError("ERROR ! You said that the first character should have the second character score");
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
                Debug.LogError("The score given [" + p_score + "] is not planned in the switch.");
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
        _healthText.text = _currentHP + " HP / " + _maxHP + " HP";

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
        if (!_isPlayer1Character)
        {
            if (Input.GetKeyDown(KeyCode.F))
                currentHP += 5;

            if (Input.GetKeyDown(KeyCode.G))
                currentHP -= 5;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.V))
                currentHP += 5;

            if (Input.GetKeyDown(KeyCode.B))
                currentHP -= 5;
        }

    }
    #endregion
    #endregion
}