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

    /// <summary> Enable / disable the BoxCollider2D of all units, that means players can or cannot interact with all units anymore </summary>
    public void HandleAllUnitInteractions(bool p_isAllUnitInteractionEnable)
    {
        foreach (Unit unit in player1GridManager.unitList)
        {
            unit.GetComponent<BoxCollider2D>().enabled = p_isAllUnitInteractionEnable;
        }

        foreach (Unit unit in player2GridManager.unitList)
        {
            unit.GetComponent<BoxCollider2D>().enabled = p_isAllUnitInteractionEnable;
        }
    }
}