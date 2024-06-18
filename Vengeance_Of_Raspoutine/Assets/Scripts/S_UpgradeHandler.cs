using System.Collections;
using UnityEngine;

public class S_UpgradeHandler : MonoBehaviour
{
    public static S_UpgradeHandler Instance;

    public GameObject _player1UpgradeCanvas;
    public GameObject _player2UpgradeCanvas;
    [SerializeField] private GameObject _animPlayerCanvas;
    private bool _isFinish = false; 

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
    }

    private void Start()
    {
        S_WeatherAnimation.Instance.hasPlayedPlayer = false;
        S_WeatherAnimation.Instance.hasPlayedWeather = false;
    }

    public void NextBtn()
    {
        _player1UpgradeCanvas.SetActive(false);
        _player2UpgradeCanvas.SetActive(true);
    }

    public void Finished()
    {
        _player2UpgradeCanvas.SetActive(false);
        
        S_GameManager.Instance.HandlePlayerLose(S_GameManager.Instance.isLastPlayerDeadIsPlayer1);
        _animPlayerCanvas.SetActive(false);

        S_GameManager.Instance.isGameRunning = true;
        StartCoroutine(AnimSwitch());
    }

    public IEnumerator AnimSwitch()
    {
        
        S_WeatherAnimation.Instance.PlayWeatherAnimation();
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(2.5f);
        _animPlayerCanvas.SetActive(true);
        S_WeatherAnimation.Instance.PlayAnimAfterTheOther();
        yield return new WaitForSeconds(3f);
        
    }
}
