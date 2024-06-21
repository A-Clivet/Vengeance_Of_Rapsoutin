using UnityEngine;

namespace S_BehaviorTree
{
    public abstract class S_Tree : MonoBehaviour
    {
        protected Node _root = null;

        public void CallTree()
        {
            if (_root != null)
                _root.Evaluate();
        }

        protected abstract Node SetupTree();        
    }
}
