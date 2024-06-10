using UnityEngine;

public class S_SkillTreeHandler : MonoBehaviour
{
    public static S_SkillTreeHandler Instance;

    [Header("References :")]
    public GameObject player1SkillTree;
    public GameObject player2SkillTree;

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
        S_UpgradeHandler.Instance._player1UpgradeCanvas.SetActive(true);
    }
}