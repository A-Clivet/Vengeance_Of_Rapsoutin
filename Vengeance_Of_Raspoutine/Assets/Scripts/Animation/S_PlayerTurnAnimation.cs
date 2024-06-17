using System.Collections;
using TMPro;
using UnityEngine;

public class S_PlayerTurnAnimation : MonoBehaviour
{
    [SerializeField] private S_GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _playerTurnText;
    [SerializeField] private Sprite _character1SpriteImage;
    [SerializeField] private Sprite _character2SpriteImage;
    [SerializeField] private Animator _animatorPlayerTurn;
    [SerializeField] private Animator _animatorWeather;

    //private bool _isFinished = false;

    public string animationName;

    private void Start()
    {
        _animatorWeather.SetBool("_isWeatherAnimFinished", true);
        _animatorPlayerTurn.SetBool("_isPlayerTurnAnimFinished", false);
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = _animatorPlayerTurn.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo weatherStateInfo = _animatorWeather.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(animationName))
        {
            if (stateInfo.normalizedTime >= 1f)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                _animatorPlayerTurn.Update(0f);
            }
        }
        StartCoroutine(waitTheEndOfWeatherAnim());
    }

    public void PlayTurnAnimation(GameObject p_characterImage)
    {
        if (_gameManager.isPlayer1Turn == true)
        {
            _playerTurnText.text = "Player 1 turn";
            p_characterImage.GetComponent<SpriteRenderer>().sprite = _character1SpriteImage;
            transform.GetChild(0).gameObject.SetActive(true);
        }

        else
        {
            _playerTurnText.text = "Player 2 turn";
            p_characterImage.GetComponent<SpriteRenderer>().sprite = _character2SpriteImage;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private IEnumerator waitTheEndOfWeatherAnim()
    {
        if (_animatorWeather.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            _animatorPlayerTurn.SetBool("PlayAnimation", true);
            _animatorWeather.SetBool("_isWeatherAnimFinished", false);
        }
        yield return new WaitForSeconds(4f);
    }
}