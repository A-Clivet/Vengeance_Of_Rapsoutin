using S_BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_UseAbility : Node
{
    private S_SpecialCapacityManager _ability;
    private S_SpecialCapacityStats _abilityStat;

    public S_UseAbility(S_SpecialCapacityManager p_ability, S_SpecialCapacityStats p_abilityStat)
    {
        _ability = p_ability;
        _abilityStat = p_abilityStat;
    }

    public override NodeState Evaluate()
    {
        _ability.LaunchSpecialCapacity(_abilityStat, false);

        pr_state = NodeState.SUCCESS;
        return pr_state;
    }

}