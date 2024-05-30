using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_EndMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;

    public void WhoWin(bool p_isPlayer1Win)
    {
        if (p_isPlayer1Win)
        {
            _timerText.text = "Player 1 ";
        }
        else
        {
            _timerText.text = "Player 2 ";
        }
    }

    public void OnRematchButton()
    {
        SceneManager.LoadScene("MainGame"); 
    }

    public void OnLeaveButton()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
