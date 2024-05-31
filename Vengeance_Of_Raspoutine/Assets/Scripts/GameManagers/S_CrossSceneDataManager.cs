using UnityEngine;
using UnityEngine.SceneManagement;

public class S_CrossSceneDataManager : MonoBehaviour
{
    public static S_CrossSceneDataManager Instance;

    // Game's game mode variables
    private S_GameModeInvoker.GameModes _gameMode;

    /// <summary> Will set gameMode to the given value, and will load the Scene "MainGame" </summary>
    public S_GameModeInvoker.GameModes gameMode
    {
        get { return _gameMode; }
        set
        {
            _gameMode = value;

            SceneManager.LoadSceneAsync("MainGame");
        }
    }

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.DestructionOfTheSecondOne);
        DontDestroyOnLoad(transform.parent);
    }
}