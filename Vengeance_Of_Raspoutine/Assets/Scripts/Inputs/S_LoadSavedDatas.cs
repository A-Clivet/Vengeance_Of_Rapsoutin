using UnityEngine;
using UnityEngine.InputSystem;

public class S_LoadSavedDatas : MonoBehaviour
{
    [SerializeField] private InputActionAsset _actionAsset;
    private void Start()
    {
        //Searches through the bindings of the action map to check for any overrides in PlayerPrefs.
        for (int i=0; i < _actionAsset.actionMaps[0].actions.Count; i++)
        {
            if (PlayerPrefs.HasKey(_actionAsset.actionMaps[0].actions[i].name))
            {
                _actionAsset.actionMaps[0].actions[i].ApplyBindingOverride(PlayerPrefs.GetString(_actionAsset.actionMaps[0].actions[i].name));
            }
        }
    }
}
