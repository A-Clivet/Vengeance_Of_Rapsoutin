using UnityEngine;

public class S_GameModeInvoker : MonoBehaviour
{
    // NOTE : This script have to be in a button object

    public enum GameModes
    {
        Classic,
        Domination,
        SuddenDeath
    }

    [SerializeReference] GameModes _gameMode;

    /// <summary> This function will tell to the S_GameManager that the Game mode of the game has changed,
    /// that will launch a new game (changing scene to MainGame) the type of game will be the value of _gameModes variable (set in the button) </summary>
    /// <param name = "p_gameModeInvoker"> Put S_GameModeInvoker script reference </param>
    public void LaunchNewGame(S_GameModeInvoker p_gameModeInvoker)
    {
        S_CrossSceneDataManager.Instance.gameMode = p_gameModeInvoker._gameMode;
    }
}