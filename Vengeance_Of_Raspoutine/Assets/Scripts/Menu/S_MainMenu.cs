using UnityEngine;
using UnityEngine.SceneManagement;

public class S_MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject _canvasOption;
    [SerializeField] private GameObject _canvasMainMenu;
    [SerializeField] private GameObject _canvasCredits;
    [SerializeField] private GameObject _canvasTuto;

    private S_TutoManager _tutoManager;


    private void Start()
    {
        _canvasMainMenu.SetActive(true);
        _canvasOption.SetActive(false);
        _canvasCredits.SetActive(false);
        _canvasTuto.SetActive(false);
        _tutoManager = S_TutoManager.Instance;
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void OnExitGameButton()
    {
        Application.Quit();
    }

    public void BackButton(GameObject p_currentCanvas)
    {
        SwitchCanvas(_canvasMainMenu, p_currentCanvas);
        _tutoManager.intTuto = 0;
        _tutoManager.ResetTutoOrder(0);
        _tutoManager.DesactivateButtonTuto();
    }

    private void SwitchCanvas(GameObject p_canvas , GameObject p_desactivateCanvas)
    {
        p_desactivateCanvas.SetActive(false);
        p_canvas.SetActive(true);
    }
}
