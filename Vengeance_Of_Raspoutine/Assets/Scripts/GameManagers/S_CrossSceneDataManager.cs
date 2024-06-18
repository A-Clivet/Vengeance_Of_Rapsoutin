using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static S_UnitSelectorMenu;

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

    public List<PlayersSelectedUnit> player1SelectedUnits;
    public List<PlayersSelectedUnit> player2SelectedUnits;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.DestructionOfTheSecondOne);
        DontDestroyOnLoad(transform.parent);
    }
}