using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class S_SkillTreeDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private TextMeshProUGUI skillTextDescription;
    [SerializeField] private TextMeshProUGUI skillTextEffect;
    [SerializeField] private S_SpecialCapacityStats soText;

    private void Start()
    {
        descriptionPanel.SetActive(false);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionPanel.SetActive(true);
        skillTextDescription.text = soText.capacityDescription;
        skillTextEffect.text = soText.capacityEffectDescription;
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionPanel.SetActive(false);
    }
}
