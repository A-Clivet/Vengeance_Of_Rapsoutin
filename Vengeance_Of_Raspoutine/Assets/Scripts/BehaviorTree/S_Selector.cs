using System.Collections.Generic;

namespace BehaviorTree
{
    public class S_Selector : Node
    {
        public S_Selector() : base() { }
        public S_Selector(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        pr_state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        pr_state = NodeState.RUNNING;
                        return pr_state;
                    default:
                        continue;
                }
            }
            pr_state = NodeState.FAILURE;
            return pr_state;
        }
    }
}