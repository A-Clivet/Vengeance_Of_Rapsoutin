using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class NeutralNode : Node
{
    public NeutralNode() : base() { }
    public NeutralNode(List<Node> children) : base(children) { }


    public override NodeState Evaluate()
    {
        //code for the behavior here
        state = NodeState.FAILURE;


        //Debug.Log($"{this} : {state}");
        return state;
    }
}