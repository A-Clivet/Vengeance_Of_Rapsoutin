using UnityEngine;

public class S_PlayersActionPreventerVisualUIReferencesHandler : MonoBehaviour
{
    public static S_PlayersActionPreventerVisualUIReferencesHandler Instance;

    [Header("References :")]
    public GameObject player1ActionPreventerVisualGameObject;
    public GameObject player2ActionPreventerVisualGameObject;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
    }
}