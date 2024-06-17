using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class S_SwapButtonsHandler : MonoBehaviour
{
    public static S_SwapButtonsHandler Instance;

    public Button player1SwapButton;
    public Button player2SwapButton;
    private ColorBlock player1ButtonColors;
    private ColorBlock player2ButtonColors;
    public Color swapActivated;
    public Color swapDeactivated = new Color(1, 1, 1);
    public TextMeshProUGUI player1ButtonText;
    public TextMeshProUGUI player2ButtonText;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndDestructionOfTheSecondOne);
    }

    private void Start()
    {
        player1ButtonColors = player1SwapButton.colors;
        player2ButtonColors = player2SwapButton.colors;
        player1ButtonText.text = "Swap " + S_GameManager.Instance.swapCounterP1;
        player2ButtonText.text = "Swap " + S_GameManager.Instance.swapCounterP2;
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

    public void HandleSwapUnitButtonEffects(bool p_isPlayer1Affected, bool p_fxEnable)
    {
        if (p_isPlayer1Affected)
        {
            if (p_fxEnable)
            {
                player1ButtonColors.selectedColor = swapActivated;
            }
            else
            {
                player1ButtonColors.selectedColor = swapDeactivated;
            }
            player1SwapButton.colors = player1ButtonColors;
        }
        else
        {
            if (p_fxEnable)
            {
                player2ButtonColors.selectedColor = swapActivated;
            }
            else
            {
                player2ButtonColors.selectedColor = swapDeactivated;
            }
            player2SwapButton.colors= player2ButtonColors;
        }
    }
}