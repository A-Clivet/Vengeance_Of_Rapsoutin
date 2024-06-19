using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_UpgradeHandler : MonoBehaviour
{
    public static S_UpgradeHandler Instance;

    public GameObject _player1UpgradeCanvas;
    public GameObject _player2UpgradeCanvas;
    [SerializeField] private GameObject _animPlayerCanvas;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
    }

    public void Finished() //Button that manages the last troop upgrade canvas to start a new game
    {
        _player2UpgradeCanvas.SetActive(false);
        S_GameManager.Instance.HandlePlayerLose(S_GameManager.Instance.isLastPlayerDeadIsPlayer1);
        _animPlayerCanvas.SetActive(false);
        S_GameManager.Instance.isGameRunning = true;
        StartCoroutine(AnimSwitch());
    }

    public IEnumerator AnimSwitch() // Coroutine to manage weather animation and switch to player animation after the first round 
    {
        S_WeatherAnimation.Instance.PlayWeatherAnimation();
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(2.5f);
        _animPlayerCanvas.SetActive(true);
        S_WeatherAnimation.Instance.PlayAnimAfterTheOther();
        yield return new WaitForSeconds(3f);
    }
}
