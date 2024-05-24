using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_CharacterHealth : MonoBehaviour
{
    #region Variables
    struct ScoreImageGameObjects
    {
        GameObject score1Image;
        GameObject score2Image;
        GameObject score3Image;
    }

    [Header("References :")]
    [SerializeField] Image _healthBarFillSprite;
    [SerializeField] TextMeshProUGUI _healthText;
    [SerializeField] ScoreImageGameObjects _scoreImageGameObjects;



    bool _isPlayer1Character;
    int _currentHP = 45;
    int _maxHP = 50;

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
        S_GameManager.Instance.HandlePlayerLose(_isPlayer1Character);
    }

    // -- TO DEBUG -- //
    #region TO DEBUG
    private void Start()
    {
        currentHP = 25;
    }

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