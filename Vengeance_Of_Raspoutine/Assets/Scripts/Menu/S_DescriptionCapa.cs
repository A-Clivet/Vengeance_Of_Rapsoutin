using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class S_DescriptionCapa : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject descriptionCapaPanel;
    [SerializeField] private GameObject descriptionUnit1Panel;
    [SerializeField] private GameObject descriptionUnit2Panel;
    [SerializeField] private GameObject descriptionUnit3Panel;

    public int _idDescription;

    private void Start()
    {
        descriptionCapaPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) //when the mouse hovers over the skill buttons, the correct skill description is displayed
    {
        switch (_idDescription)
        {
            case 1:
                descriptionCapaPanel.SetActive(true);
                break;

            case 2:
                descriptionUnit1Panel.SetActive(true);
                break;

            case 3:
                descriptionUnit2Panel.SetActive(true);
                break;

            case 4:
                descriptionUnit3Panel.SetActive(true);
                break;

        }
    }

    public void OnPointerExit(PointerEventData eventData)// desactivate the panel of the skill description
    {
        switch (_idDescription)
        {
            case 1:
                descriptionCapaPanel.SetActive(false);
                break;

            case 2:
                descriptionUnit1Panel.SetActive(false);
                break;

            case 3:
                descriptionUnit2Panel.SetActive(false);
                break;

            case 4:
                descriptionUnit3Panel.SetActive(false);
                break;

        }
    }
}
