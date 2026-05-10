using System;
using WorldObject;

namespace EntityStatus
{
    [Serializable]
    public abstract class StatusBase
    {
        public virtual void Activate(Entity entity) {}
        public virtual void Deactivate(Entity entity) {}
    }
}