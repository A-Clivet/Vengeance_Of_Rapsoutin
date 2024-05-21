using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_CharacterHealth : MonoBehaviour
{
    #region Variables
    [Header("References :")]
    [SerializeField] Image _healthBarFillSprite;
    [SerializeField] TextMeshProUGUI _healthText;

    bool _isPlayer1Character; // TO USE WHEN THE FUNCTION "CharacterDie" will be write
    int _currentHP = 45;
    int _maxHP = 50;

    #endregion

    #region Getter / Setter
    public int CurrentHP
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

    public int MaxHP
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
        CurrentHP = _maxHP;
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
        // TODO : End the game + add one victory to the other user
        // This function will call a GameManager's function
    }

    // -- TO DEBUG -- //
    private void Start()
    {
        CurrentHP = 25;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            CurrentHP += 5;

        if (Input.GetKeyDown(KeyCode.G))
            CurrentHP -= 5;
    }
    #endregion
}