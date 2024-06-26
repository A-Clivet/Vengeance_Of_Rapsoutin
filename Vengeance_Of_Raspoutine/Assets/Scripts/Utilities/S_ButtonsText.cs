using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_ButtonsText : MonoBehaviour
{
    private Color _transparenci = new Color(0,0,0, 217);
    
    
    public void OpacitiText()
    {
        gameObject.GetComponent<TextMeshProUGUI>().color = _transparenci;
    }
}
