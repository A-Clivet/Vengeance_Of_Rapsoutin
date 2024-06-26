using UnityEngine;
using UnityEngine.SceneManagement;

public class S_EndMenuManager : MonoBehaviour
{
    public static S_EndMenuManager Instance;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
    }

    public void WhoWin(bool p_isPlayer1Win)
    {
        if (p_isPlayer1Win)
        {
            SceneManager.LoadScene("EndMenuPlayer1");
        }
        else
        {
            SceneManager.LoadScene("EndMenuPlayer2");
        }
    }

    public void OnRematchButton()
    {
        SceneManager.LoadScene("MainGame"); 
    }

    public void OnMainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}