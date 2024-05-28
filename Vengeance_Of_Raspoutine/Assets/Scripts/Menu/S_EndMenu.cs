using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_EndMenu : MonoBehaviour
{

    [HideInInspector] public bool _player1Win = true;
    [SerializeField] private TextMeshProUGUI _timerText;


    private void Start()
    {
        WhoWin();  
    }

    public void WhoWin()
    {
        if (_player1Win)
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
