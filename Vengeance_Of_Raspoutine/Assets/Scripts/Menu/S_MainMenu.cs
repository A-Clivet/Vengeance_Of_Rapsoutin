using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject _canvasOption;
    [SerializeField] private GameObject _canvasMainMenu;
    [SerializeField] private GameObject _canvasCredits;
    [SerializeField] private GameObject _canvasTuto;


    private void Start()
    {
        _canvasMainMenu.SetActive(true);
        _canvasOption.SetActive(false);
        _canvasCredits.SetActive(false);
        _canvasTuto.SetActive(false);
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void OnOptionButton()
    {
        _canvasMainMenu.SetActive(false);
        _canvasOption.SetActive(true);
    }

    public void OnTutoButton()
    {
        _canvasMainMenu.SetActive(false);
        _canvasTuto.SetActive(true);
    }

    public void OnCreditsButton()
    {
        _canvasMainMenu.SetActive(false);
        _canvasCredits.SetActive(true);
    }

    public void OnExitGameButton()
    {
        Application.Quit();
    }

    public void OnBackButton()
    {

    }
}
