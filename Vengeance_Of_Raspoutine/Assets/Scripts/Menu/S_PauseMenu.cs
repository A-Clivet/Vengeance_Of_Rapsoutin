using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class S_PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseCanvas;


    private void Start()
    {
        _pauseCanvas.SetActive(false);
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
            else
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

    public void SettingsButton()
    {

    }

    public void LeaveButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
