using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_CharacterSpecialCapacity : MonoBehaviour
{
    #region Variables
    [Header("References :")]
    [SerializeField] GameObject _specialCapacityDescriptionFolder;
    [SerializeField] TextMeshProUGUI _capacityNameTextUI;
    [SerializeField] TextMeshProUGUI _capacityDescriptionTextUI;
    [SerializeField] TextMeshProUGUI _capacityEffectTextUI;
    #endregion

    #region Methods
    public void RecieveSpecialCapacityStats(string p_capacityName, string p_capacityDescription, string p_capacityEffectDescription)
    {
        _capacityNameTextUI.text = p_capacityName;
        _capacityDescriptionTextUI.text = p_capacityDescription;
        _capacityEffectTextUI.text = p_capacityEffectDescription;
    }

    public void ShowSpecialCapacityDescriptionUIs(InputAction.CallbackContext p_context)
    {
        if (p_context.performed)
        {
            if (!_specialCapacityDescriptionFolder.activeSelf)
            {
                _specialCapacityDescriptionFolder.SetActive(true);
            }
            else
            {
                _specialCapacityDescriptionFolder.SetActive(false);
            }
        }
    }
    #endregion
}