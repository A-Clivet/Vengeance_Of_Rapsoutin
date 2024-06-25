using System;
using System.Collections;
using UnityEngine.Animations;
using UnityEngine;

public class S_UnitDestructionAnimationManager : MonoBehaviour
{
    // -- Variables -- //
    public static S_UnitDestructionAnimationManager Instance;

    public enum UnitDestructionAnimationsEnum
    {
        Explosion,
        HellFireEffect,
        Pak,
        Pouf,
    }

    [Serializable]
    struct UnitDestructionAnimatorControllers
    {
        public RuntimeAnimatorController Explosion;
        public RuntimeAnimatorController HellFireEffect;
        public RuntimeAnimatorController Pak;
        public RuntimeAnimatorController Pouf;
    }

    [Header("References :")]
    [SerializeField] GameObject _unitAnimationDestructionPrefab;

    [Header("Animation's references :")]
    [SerializeField] UnitDestructionAnimatorControllers _unitDestructionAnimatorControllers;

    // -- Methods -- //
    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
    }

    /// <summary>
    /// BEWARE IT'S A COROUTINE SO USE "StartCoroutine()" TO CALL THIS FUNCTION !
    /// <example>
    /// <code> 
    /// Exemple :
    /// StartCoroutine(S_UnitDestructionAnimationManager.Instance.HandleUnitAnimationDestruction(
    ///     S_UnitDestructionAnimationManager.UnitDestructionAnimationsEnum.Explosion,
    ///     *YOUR UNIT REFERENCE*
    /// ));
    /// </code> </example>
    /// <para> Create a GameObject with a Animator in it, it is located inside of the GO_UnitDestructionAnimationManager. 
    /// Play the given animation, and destroy the GameObject when the animation is finished </para>
    /// </summary>
    public IEnumerator HandleUnitAnimationDestruction(UnitDestructionAnimationsEnum p_unitDestructionAnimationsEnum, Unit p_unit)
    {
        // Creation of the GameObject that will play the animation
        GameObject _unitAnimationDestructionGameObject = Instantiate(_unitAnimationDestructionPrefab, transform);

        // Change the created GameObject's position to the unit position
        _unitAnimationDestructionGameObject.transform.SetPositionAndRotation(p_unit.transform.position, Quaternion.identity);

        // We get the correct animation (RuntimeAnimatorController) form the given p_unitDestructionAnimationsEnum argument
        RuntimeAnimatorController _animatorController = GetCorrectAnimatorController(p_unitDestructionAnimationsEnum);

        // Get the Animator of the created GameObject, assign the _animatorController to it, and start the animation
        Animator _animator = _unitAnimationDestructionGameObject.GetComponent<Animator>();
        _animator.runtimeAnimatorController = _animatorController;

        // Wait until the animation is done
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

        // Destroy the GameObject
        Destroy(_unitAnimationDestructionGameObject);
    }

    RuntimeAnimatorController GetCorrectAnimatorController(UnitDestructionAnimationsEnum p_unitDestructionAnimationsEnum)
    {
        switch (p_unitDestructionAnimationsEnum)
        {
            case UnitDestructionAnimationsEnum.Explosion:
                return _unitDestructionAnimatorControllers.Explosion;

            case UnitDestructionAnimationsEnum.HellFireEffect:
                return _unitDestructionAnimatorControllers.HellFireEffect;

            case UnitDestructionAnimationsEnum.Pak:
                return _unitDestructionAnimatorControllers.Pak;

            case UnitDestructionAnimationsEnum.Pouf:
                return _unitDestructionAnimatorControllers.Pouf;

            default:
                Debug.LogError("ERROR ! The given p_unitDestructionAnimationsEnum argument '" + p_unitDestructionAnimationsEnum.ToString() + "' is not planned in the switch");
                return null;
        }
    }
}