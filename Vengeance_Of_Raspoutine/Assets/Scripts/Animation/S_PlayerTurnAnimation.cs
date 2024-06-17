using System.Collections;
using TMPro;
using UnityEngine;

public class S_PlayerTurnAnimation : MonoBehaviour
{
    [SerializeField] private S_GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _playerTurnText;
    [SerializeField] private Sprite _character1SpriteImage;
    [SerializeField] private Sprite _character2SpriteImage;

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
}