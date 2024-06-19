using UnityEngine;
using UnityEngine.UI;

public class S_SwapButtonsHandler : MonoBehaviour
{
    public Button player1SwapButton;
    public Button player2SwapButton;
    public static S_SwapButtonsHandler Instance;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndDestructionOfTheSecondOne);
    }
}
