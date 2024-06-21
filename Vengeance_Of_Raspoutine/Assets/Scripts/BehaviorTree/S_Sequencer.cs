using System.Collections.Generic;

namespace S_BehaviorTree
{
    public class S_Sequencer : Node
    {
        public S_Sequencer() : base() { }
        public S_Sequencer(List<Node> children) : base(children) { }


        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        pr_state = NodeState.FAILURE;
                        return pr_state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        pr_state = NodeState.SUCCESS;
                        return pr_state;
                }
            }

            pr_state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return pr_state;

        }
    }
}