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
                S_UnitCallButtonHandler.Instance.player1UnitCall.enabled = true;
                S_UnitCallButtonHandler.Instance.player2UnitCall.enabled = true;
                S_GameManager.Instance.DeactivateGrid();

                _pauseCanvas.SetActive(false);
                Time.timeScale = 1.0f;
            }
            else if (!_pauseCanvas.activeSelf && !_settingsCanvas.activeSelf)
            {
                foreach (Unit unit in S_GameManager.Instance.player1GridManager.unitList)
                {
                    unit.GetComponent<BoxCollider2D>().enabled = false;
                }

                foreach (Unit unit in S_GameManager.Instance.player2GridManager.unitList)
                {
                    unit.GetComponent<BoxCollider2D>().enabled = false;
                }
                S_UnitCallButtonHandler.Instance.player1UnitCall.enabled = false;
                S_UnitCallButtonHandler.Instance.player2UnitCall.enabled = false;
                _pauseCanvas.SetActive(true);
                Time.timeScale = 0f;

            }
        }
    }

    public void ResumeButton()
    {
        Time.timeScale = 1f;

        S_UnitCallButtonHandler.Instance.player1UnitCall.enabled = true;
        S_UnitCallButtonHandler.Instance.player2UnitCall.enabled = true;
        S_GameManager.Instance.DeactivateGrid();

        _pauseCanvas.SetActive(false);

    }

    public void LeaveButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
