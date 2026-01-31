using UnityEngine;

namespace AI
{
    public abstract class BaseStateBehavior : MonoBehaviour,IStateBehavior
    {
        public virtual void Start()
        {
            enabled = false;
        }

        public virtual void StartBehavior()
        {
            enabled = true;
        }

        public virtual void StopBehavior()
        {
            enabled = false;
        }

        public virtual bool Completed()
        {
            return true;
        }
    }
}
