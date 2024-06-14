using UnityEngine;
using UnityEngine.UI;

public class S_SwapButtonsHandler : MonoBehaviour
{
    public static S_SwapButtonsHandler Instance;

    public Button player1SwapButton;
    public Button player2SwapButton;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndDestructionOfTheSecondOne);
    }

    /// <summary> Handles the interaction state of the swap unit buttons for both players.
    /// Sets the interactable state of the swap unit button for player 1 or player 2 based on the provided parameters. </summary>
    /// <param name = "p_isPlayer1SwapUnitButtonChanged"> Indicates if the swap unit button for player 1 should be changed. </param>
    /// <param name = "p_isButtonInteractionGivenIsEnabled"> The new interactable state to be set for the swap unit button. </param>
    public void HandleSwapUnitButtonInteraction(bool p_isPlayer1SwapUnitButtonChanged, bool p_isButtonInteractionGivenIsEnabled)
    {
        if (p_isPlayer1SwapUnitButtonChanged)
        {
            player1SwapButton.interactable = p_isButtonInteractionGivenIsEnabled;
        }
        else
        {
            player2SwapButton.interactable = p_isButtonInteractionGivenIsEnabled;
        }
    }
}