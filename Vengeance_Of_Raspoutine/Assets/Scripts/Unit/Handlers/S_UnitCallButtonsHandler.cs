using UnityEngine;
using UnityEngine.UI;

public class S_UnitCallButtonHandler : MonoBehaviour
{
    #region Variables

    public static S_UnitCallButtonHandler Instance;

    [Header("References :")]
    public S_UnitCall player1UnitCall;
    public S_UnitCall player2UnitCall;

    Button player1UnitCallButton;
    Button player2UnitCallButton;
    #endregion

    #region Methods
    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);

        player1UnitCallButton = player1UnitCall.gameObject.GetComponent<Button>();
        player2UnitCallButton = player2UnitCall.gameObject.GetComponent<Button>();
    }

    /// <summary> Handles the interaction state of the unit call buttons for both players.
    /// Sets the interactable state of the unit call button for player 1 or player 2 based on the provided parameters. </summary>
    /// <param name = "p_isPlayer1UnitCallButtonChanged"> Indicates if the unit call button for player 1 should be changed. </param>
    /// <param name = "p_isButtonInteractionGivenIsEnabled"> The new interactable state to be set for the unit call button. </param>
    public void HandleUnitCallButtonInteraction(bool p_isPlayer1UnitCallButtonChanged, bool p_isButtonInteractionGivenIsEnabled)
    {
        if (p_isPlayer1UnitCallButtonChanged)
        {
            player1UnitCallButton.interactable = p_isButtonInteractionGivenIsEnabled;
        }
        else
        {
            player2UnitCallButton.interactable = p_isButtonInteractionGivenIsEnabled;
        }
    }

    public void CallUnitsForAllPlayers()
    {
        player1UnitCall.UnitCalling();
        player2UnitCall.UnitCalling();
    }

    #endregion
}