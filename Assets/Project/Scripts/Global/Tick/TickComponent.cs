using UnityEngine;

namespace Game.Scripts
{
    /// <summary>
    /// Sub-components of game objects should be inherited from this.
    /// </summary>
    public abstract class TickComponent : MonoBehaviour, ITickComponent
    {
        private int _id;

        public int Id => _id;

        public void SetId(int tickId)
        {
            _id = tickId;
        }

        public virtual void Init()
        {
            // Override this method without base;
        }

        public virtual void Enable()
        {
            // Override this method without base;
        }

        public virtual void PhysicsTick()
        {
            // Override this method without base;
        }

        public virtual void Tick()
        {
            // Override this method without base;
        }

        public virtual void CameraTick()
        {
            // Override this method without base;
        }

        public virtual void Disable()
        {
            // Override this method without base;
        }

        public virtual void Dispose()
        {
            // Override this method without base;
        }
    }
}
