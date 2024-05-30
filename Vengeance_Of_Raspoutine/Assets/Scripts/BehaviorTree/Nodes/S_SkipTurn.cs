using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class S_SkipTurn : Node
{
    private S_GameManager _gameManager;

    public S_SkipTurn(S_GameManager p_gameManager)
    {
        _gameManager = p_gameManager;
    }

    public override NodeState Evaluate()
    {
        _gameManager.EndTurn(); //calls the function EndTurn in the gameManager to skip the ai turn

        pr_state = NodeState.SUCCESS;


        return pr_state;
    }
}
