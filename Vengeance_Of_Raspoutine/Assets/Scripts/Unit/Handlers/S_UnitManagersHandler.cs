using UnityEngine;

public class S_UnitManagersHandler : MonoBehaviour
{
    public static S_UnitManagersHandler Instance;

    [Header("References :")]
    public S_UnitManager player1UnitManager;
    public S_UnitManager player2UnitManager;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
    }
}