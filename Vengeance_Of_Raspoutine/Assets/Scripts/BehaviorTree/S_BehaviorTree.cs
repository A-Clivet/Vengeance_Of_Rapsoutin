using UnityEngine;

namespace S_BehaviorTree
{
    public abstract class S_Tree : MonoBehaviour
    {
        private Node _root = null;

        protected void Start()
        {
            _root = SetupTree();
        }

        public void CallTree()
        {
            if (_root != null)
                _root.Evaluate();
        }

        protected abstract Node SetupTree();        
    }
}
