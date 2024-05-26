using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_CharacterHealth : MonoBehaviour
{
    #region Variables
    [Serializable] struct ScoreImages
    {
        public Image score1Image;
        public Image score2Image;
        public Image score3Image;
    }

    [Header("References :")]
    [SerializeField] Image _healthBarFillSprite;
    [SerializeField] TextMeshProUGUI _healthText;
    [SerializeField] ScoreImages _scoreImages;

    bool _isPlayer1Character;
    int _maxHP;
    int _currentHP;

    S_GameManager _gameManager;

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
    public void RecieveCharacterHealthStats(int p_maxHP, bool p_isPlayer1Character)
    {
        // Setting up class variables
        _maxHP = p_maxHP;
        _isPlayer1Character = p_isPlayer1Character;

        // Setting up character HP to the max
        currentHP = _maxHP;
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
        _gameManager.HandlePlayerLose(_isPlayer1Character);


    }

    // -- TO DEBUG -- //
    #region TO DEBUG
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            currentHP += 5;

        if (Input.GetKeyDown(KeyCode.G))
            currentHP -= 5;
    }
    #endregion
    #endregion
}