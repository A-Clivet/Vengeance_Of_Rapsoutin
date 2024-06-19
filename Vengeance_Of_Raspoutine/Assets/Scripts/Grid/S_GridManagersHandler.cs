using UnityEngine;

public class S_GridManagersHandler : MonoBehaviour
{
    public static S_GridManagersHandler Instance;

    [Header("References :")]
    public S_GridManager player1GridManager;
    public S_GridManager player2GridManager;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
    }
}