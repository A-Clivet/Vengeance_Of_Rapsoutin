using UnityEngine;

public class S_PlayerInputsHandler : MonoBehaviour
{
    public static S_PlayerInputsHandler Instance;

    [Header("References :")]
    public GameObject player1InputsGameObject;
    public GameObject player2InputsGameObject;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
    }
}