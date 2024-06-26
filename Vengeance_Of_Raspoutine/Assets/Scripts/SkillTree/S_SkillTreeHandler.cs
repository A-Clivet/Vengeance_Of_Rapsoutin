using System.Collections;
using UnityEngine;

public class S_SkillTreeHandler : MonoBehaviour
{
    public static S_SkillTreeHandler Instance;

    [Header("References :")]
    public GameObject player1SkillTree;
    public GameObject player2SkillTree;

    [SerializeField] private GameObject _animPlayerCanvas;

    S_GameManager _gameManager;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
    }

    private void Start()
    {
        _gameManager = S_GameManager.Instance;
    }

    public void NextSkillTree()
    {
        player1SkillTree.SetActive(false);
        player2SkillTree.SetActive(true);
    }

    public void Finished()
    {
        player2SkillTree.SetActive(false);
        S_RemoveUnit.Instance.RemoveAllUnits();
        S_GameManager.Instance.HandlePlayerLose(S_GameManager.Instance.isLastPlayerDeadIsPlayer1);
        _animPlayerCanvas.SetActive(false);
        S_GameManager.Instance.isGameRunning = true;
        StartCoroutine(AnimSwitchSkillTree());
    }

    public IEnumerator AnimSwitchSkillTree() // Coroutine to manage weather animation and switch to player animation after the first round 
    {
        S_UnitCallButtonHandler.Instance.player1UnitCallButton.interactable = false;
        S_UnitCallButtonHandler.Instance.player2UnitCallButton.interactable = false;

        S_SwapButtonsHandler.Instance.player1SwapButton.interactable = false;
        S_SwapButtonsHandler.Instance.player2SwapButton.interactable = false;

        S_WeatherAnimation.Instance.PlayWeatherAnimation();
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(2.5f);
        _animPlayerCanvas.SetActive(true);
        S_WeatherAnimation.Instance.PlayAnimAfterTheOther();
        yield return new WaitForSeconds(3f);

        S_GameManager.Instance.player1UnitCall.UnitCalling();
        S_GameManager.Instance.player2UnitCall.UnitCalling();

        S_SwapButtonsHandler.Instance.HandleSwapUnitButtonInteraction(S_GameManager.Instance.isPlayer1Turn, true);

        S_UnitCallButtonHandler.Instance.HandleUnitCallButtonInteraction(S_GameManager.Instance.isPlayer1Turn, true);
    }
}