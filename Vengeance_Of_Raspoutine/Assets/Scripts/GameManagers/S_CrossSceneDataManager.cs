using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static S_UnitSelectorMenu;

public class S_CrossSceneDataManager : MonoBehaviour
{
    public static S_CrossSceneDataManager Instance;

    public S_GameModeInvoker.GameModes gameMode;

    public List<PlayersSelectedUnit> player1SelectedUnits;
    public List<PlayersSelectedUnit> player2SelectedUnits;

    public bool vsIA;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.DestructionOfTheSecondOne);
        DontDestroyOnLoad(transform.parent);
    }
    public void VsIAOrPlayer(bool p_IsVsIA)
    {
        vsIA = p_IsVsIA;
    }
}