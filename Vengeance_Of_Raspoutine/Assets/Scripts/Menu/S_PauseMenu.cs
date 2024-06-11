using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class S_PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private GameObject _settingsCanvas;

    private void Start()
    {
        _pauseCanvas.SetActive(false);
        _settingsCanvas.SetActive(false);
    }

    public void OnEscapeButton(InputAction.CallbackContext p_ctx) // open or close the canvas with escape button
    {
        if (p_ctx.performed)
        {
            if (_pauseCanvas.activeSelf)
            {
                _pauseCanvas.SetActive(false);
                Time.timeScale = 1.0f;
            }
            else if (!_pauseCanvas.activeSelf && !_settingsCanvas.activeSelf)
            {
                _pauseCanvas.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    public void ResumeButton()
    {
        Time.timeScale = 1f;
        _pauseCanvas.SetActive(false);
    }

    public void LeaveButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
