using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class S_SwapButtonsHandler : MonoBehaviour
{
    public static S_SwapButtonsHandler Instance;

    public Button player1SwapButton;
    public Button player2SwapButton;
    public GameObject player1SwapButtonActivated;
    public GameObject player2SwapButtonActivated;
    public GameObject player1SwapButtonDeactivated;
    public GameObject player2SwapButtonDeactivated;
    public TextMeshProUGUI player1ButtonText;
    public TextMeshProUGUI player2ButtonText;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndDestructionOfTheSecondOne);
    }

    private void Start()
    {
        player1ButtonText.text = S_GameManager.Instance.swapCounterP1.ToString();
        player2ButtonText.text = S_GameManager.Instance.swapCounterP2.ToString();
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
                player1SwapButtonDeactivated.SetActive(false);
                player1SwapButtonActivated.SetActive(true);
            }
            else
            {
                player1SwapButtonActivated.SetActive(false);
                player1SwapButtonDeactivated.SetActive(true);
            }
        }
        else
        {
            if (p_fxEnable)
            {
                player2SwapButtonActivated.SetActive(true);
                player2SwapButtonDeactivated.SetActive(false);
            }
            else
            {
                player2SwapButtonActivated.SetActive(false);
                player2SwapButtonDeactivated.SetActive(true);
            }
        }
    }
}