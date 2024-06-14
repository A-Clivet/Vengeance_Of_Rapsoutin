using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_BattleUIsReferencesHandler : MonoBehaviour
{
    public static S_BattleUIsReferencesHandler Instance;

    [Header("References :")]
    public TextMeshProUGUI turnTimerTextUI;
    public TextMeshProUGUI playerTurnTextUI;
    public TextMeshProUGUI playerActionsLeftTextUI;
    public TextMeshProUGUI totalTurnsTextUI;
    public Button skipTurnButtonUI; 

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
    }
}