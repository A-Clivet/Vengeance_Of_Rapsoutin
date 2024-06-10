using UnityEngine;

public class S_UpgradeHandler : MonoBehaviour
{
    public static S_UpgradeHandler Instance;

    public GameObject _player1UpgradeCanvas;
    public GameObject _player2UpgradeCanvas;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
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

        S_GameManager.Instance.isGameRunning = true;
        Time.timeScale = 1.0f;
    }
}
