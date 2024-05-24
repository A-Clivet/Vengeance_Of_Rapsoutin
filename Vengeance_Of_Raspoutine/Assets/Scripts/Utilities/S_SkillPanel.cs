using TMPro;
using UnityEngine;

public class S_SkillPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshpro;
    private bool _isVisible = false;
    public void SetDesc(string p_desc)
    {
        _textMeshpro.text = p_desc;
    }

    public void ShowPanel()
    {
        _isVisible = !_isVisible;
        gameObject.SetActive(_isVisible);
    }
}
