using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class S_PauseMenu : MonoBehaviour
{
    [Header("References :")]
    [SerializeField] GameObject _pauseCanvas;
    [SerializeField] GameObject _settingsCanvas;
    [SerializeField] GameObject _chooseSettingMenuGameObject;
    [SerializeField] GameObject _settingsMenuGameObject;
    [SerializeField] GameObject _rebindControlMenuGameObject;

    S_GameManager _gameManager;
    S_UnitCallButtonHandler _unitCallButtonHandler;
    S_GridManagersHandler _gridManagersHandler;
    S_SwapButtonsHandler _swapButtonsHandler;
    S_BattleUIsReferencesHandler _battleUIsReferencesHandler;

    private void Start()
    {
        _gameManager = S_GameManager.Instance;
        _unitCallButtonHandler = S_UnitCallButtonHandler.Instance;
        _gridManagersHandler = S_GridManagersHandler.Instance;
        _swapButtonsHandler = S_SwapButtonsHandler.Instance;
        _battleUIsReferencesHandler = S_BattleUIsReferencesHandler.Instance;

        _pauseCanvas.SetActive(false);
        _settingsCanvas.SetActive(false);
        _chooseSettingMenuGameObject.SetActive(false);
        _settingsMenuGameObject.SetActive(false);
        _rebindControlMenuGameObject.SetActive(false);
    }

    /// <summary> Open or close the pause menu when the escape button is pressed </summary>
    public void OnEscapeButton(InputAction.CallbackContext p_context) 
    {
        if (p_context.performed)
        {
            // We close the paune menu if it was open...
            if (_settingsCanvas.activeSelf)
            {
                HandlePauseMenuVisibility(false);
            }
            // ...otherwise we open it
            else
            {
                HandlePauseMenuVisibility(true);
            }
        }
    }

    public void HandlePauseMenuVisibility(bool p_newPauseMenuVisibility)
    {
        bool _newInGameUIsVisibility = !p_newPauseMenuVisibility;

        // We change players unit call buttons interactability
        _unitCallButtonHandler.HandleUnitCallButtonInteraction(true, _newInGameUIsVisibility);
        _unitCallButtonHandler.HandleUnitCallButtonInteraction(false, _newInGameUIsVisibility);

        // We change players swap unit buttons interactability
        _swapButtonsHandler.HandleSwapUnitButtonInteraction(true, _newInGameUIsVisibility);
        _swapButtonsHandler.HandleSwapUnitButtonInteraction(false, _newInGameUIsVisibility);

        
        if (p_newPauseMenuVisibility)
            // We change all units interactability to false 
            _gridManagersHandler.HandleAllUnitInteractions(_newInGameUIsVisibility);
        else
            // We change all units interactability accordingly to what player is playing 
            _gameManager.DeactivateGrid();

        // We change skip button interactability
        _battleUIsReferencesHandler.skipTurnButtonUI.interactable = _newInGameUIsVisibility;

        // We change all pause menu UIs visibility
        _settingsCanvas.SetActive(p_newPauseMenuVisibility);
        _pauseCanvas.SetActive(p_newPauseMenuVisibility);

        if (!p_newPauseMenuVisibility)
        {
            _rebindControlMenuGameObject.SetActive(p_newPauseMenuVisibility);
            _chooseSettingMenuGameObject.SetActive(p_newPauseMenuVisibility);
            _settingsMenuGameObject.SetActive(p_newPauseMenuVisibility);
        }

        // We change the delta time of the game accordingly to the given parameter
        Time.timeScale = p_newPauseMenuVisibility ? 0 : 1;
    }

    public void ClosePauseMenu()
    {
        // We unable players unit call buttons
        _unitCallButtonHandler.HandleUnitCallButtonInteraction(true, true);
        _unitCallButtonHandler.HandleUnitCallButtonInteraction(false, true);

        _swapButtonsHandler.HandleSwapUnitButtonInteraction(true, true);
        _swapButtonsHandler.HandleSwapUnitButtonInteraction(false, true);

        // We able all units interactions
        _gridManagersHandler.HandleAllUnitInteractions(true);

        _battleUIsReferencesHandler.skipTurnButtonUI.interactable = true;

        // We disable all pause menu UIs
        _chooseSettingMenuGameObject.SetActive(false);
        _settingsMenuGameObject.SetActive(false);
        _rebindControlMenuGameObject.SetActive(false);
        _settingsCanvas.SetActive(false);
        _pauseCanvas.SetActive(false);

        Time.timeScale = 1f;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}