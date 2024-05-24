using UnityEngine;

public class S_Instantiator : MonoBehaviour
{
    #region Variables
    public static S_Instantiator Instance;

    public enum InstanceConflictResolutions
    {
        Warning,
        WarningAndPause,
        WarningAndDestructionOfTheSecondOne,
        DestructionOfTheSecondOne,
    }
    #endregion

    #region Methods
    private void Awake()
    {
        Instance = ReturnInstance(this, Instance, InstanceConflictResolutions.DestructionOfTheSecondOne);
    }

    /// <summary> 
    /// If there is no existing instance, returns the instance of the specified script type,
    /// else it handles the conflict according to the specified resolution type set. 
    /// 
    /// <para> Method utilization example: </para>
    /// <example>
    /// <code> 
    /// public class CLASS_NAME : MonoBehaviour
    /// {
    ///     public static CLASS_NAME Instance;
    /// 
    ///     public void Awake()
    ///     {
    ///         Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.p_instanceConflictResolution.WarningAndPause);
    ///     }
    /// }
    /// </code> </example> </summary>
    /// <typeparam characterName = "T"> The type of the script to return an instance of. </typeparam>
    /// <param characterName = "p_className"> The type of the script to instantiate. </param>
    /// <param characterName = "p_instanceVariable"> The "Instance" variable you have created in your class. </param>
    /// <param characterName = "p_instanceConflictResolution"> Defines how to resolve conflicts when multiple instances are detected. </param>
    /// <returns> The instance of the specified script type.</returns>
    public T ReturnInstance<T>(T p_className, T p_instanceVariable, InstanceConflictResolutions p_instanceConflictResolution) where T : MonoBehaviour
    {

        if (p_instanceVariable != null)
        {
            HandleInstanceConflict(p_className, p_instanceConflictResolution);

            return p_instanceVariable;
        }
        else
        {
            return p_className;
        }
    }

    void HandleInstanceConflict<T>(T p_className, InstanceConflictResolutions p_instanceConflictResolution) where T : MonoBehaviour
    {
        switch (p_instanceConflictResolution)
        {
            case InstanceConflictResolutions.Warning:
                Debug.LogWarning("WARNING! There are multiple [" + p_className.ToString() + "] scripts in the scene.");
                break;

            case InstanceConflictResolutions.WarningAndPause:
                Debug.LogWarning("WARNING! There are multiple [" + p_className.ToString() + "] scripts in the scene. UNITY IS PAUSED.");
                Debug.Break();
                break;

            case InstanceConflictResolutions.WarningAndDestructionOfTheSecondOne:
                Debug.LogWarning("WARNING! There are multiple [" + p_className.ToString() + "] scripts in the scene. THE SECOND SCRIPT'S GAMEOBJECT HAS BEEN DESTROYED.");
                Destroy(p_className.gameObject);
                break;

            case InstanceConflictResolutions.DestructionOfTheSecondOne:
                Destroy(p_className.gameObject);
                break;

            default:
                Debug.LogError("The conflict resolution type given [" + p_instanceConflictResolution.ToString() + "] is not planned in the switch.");
                break;
        }
    }
    #endregion
}