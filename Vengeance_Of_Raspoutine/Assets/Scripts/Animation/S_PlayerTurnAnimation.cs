using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_PlayerTurnAnimation : MonoBehaviour
{
    [SerializeField] private S_GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _playerTurnText;
    [SerializeField] private Sprite _character1SpriteImage;
    [SerializeField] private Sprite _character2SpriteImage;
    [SerializeField] private Animator _animatorPlayerTurn;

    public string animationName;

    void Update()
    {
        AnimatorStateInfo stateInfo = _animatorPlayerTurn.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(animationName))
        {
            if (stateInfo.normalizedTime >= 1f)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                _animatorPlayerTurn.Update(0f);
            }
        }
    }

    public void PlayTurnAnimation(GameObject p_characterImage)
    {
        if (_gameManager.isPlayer1Turn == true) 
        {
            _playerTurnText.text = "Player 1 turn";
            p_characterImage.GetComponent<SpriteRenderer>().sprite = _character1SpriteImage;
        }

        else
        {
            _playerTurnText.text = "Player 2 turn";
            p_characterImage.GetComponent<SpriteRenderer>().sprite = _character2SpriteImage;
        }
    }
}
