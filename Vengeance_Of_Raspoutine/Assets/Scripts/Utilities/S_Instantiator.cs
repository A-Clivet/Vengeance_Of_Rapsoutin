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
        Instance = ReturnInstance(this, Instance, InstanceConflictResolutions.WarningAndPause);
    }

    /// <summary> 
    /// If there is no existing instance, returns the instance of the specified script type,
    /// else it handles the conflict according to the specified resolution type set. 
    /// 
    /// <para>
    /// <example> Method utilization example: 
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
    /// </code> </example> </para> </summary>
    /// <typeparam name = "T"> The type of the script to return an instance of. </typeparam>
    /// <param name = "_className"> The type of the script to instantiate. </param>
    /// <param name = "_instanceVariable"> The "Instance" variable you have created in your class. </param>
    /// <param name = "_instanceConflictResolution"> Defines how to resolve conflicts when multiple instances are detected. </param>
    /// <returns> The instance of the specified script type.</returns>
    public T ReturnInstance<T>(T _className, T _instanceVariable, InstanceConflictResolutions _instanceConflictResolution) where T : MonoBehaviour
    {
        if (_instanceVariable != null)
        {
            HandleInstanceConflict(_className, _instanceConflictResolution);

            return _instanceVariable;
        }
        else
        {
            return _className;
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