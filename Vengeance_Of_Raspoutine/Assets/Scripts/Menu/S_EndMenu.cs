using UnityEngine;
using UnityEngine.SceneManagement;

public class S_EndMenu : MonoBehaviour
{
    public void WhoWin(bool p_isPlayer1Win)
    {
        if (p_isPlayer1Win)
        {
            SceneManager.LoadSceneAsync("EndMenuPlayer1");
        }
        else
        {
            SceneManager.LoadSceneAsync("EndMenuPlayer2");
        }
    }

    public void OnRematchButton()
    {
        SceneManager.LoadSceneAsync("MainGame"); 
    }

    public void OnMainMenuButton()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}