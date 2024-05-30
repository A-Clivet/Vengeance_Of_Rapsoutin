using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_CanUseAbility : Node
{
    private S_SpecialCapacityManager _ability;

    private S_CharacterAdrenaline _characterAdrenaline;

    public S_CanUseAbility(S_SpecialCapacityManager p_ability, S_CharacterAdrenaline p_characterAdrenaline)
    {
        _ability = p_ability;
        _characterAdrenaline = p_characterAdrenaline;
    }

    public override NodeState Evaluate()
    {
        if(_characterAdrenaline.currentAdrenaline >= _characterAdrenaline.maxAdrenaline)
        {
            pr_state = NodeState.SUCCESS;
            return pr_state;
        }

        pr_state = NodeState.FAILURE;
        return pr_state;
    }

}
