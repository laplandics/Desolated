using System;
using WorldObject;

namespace EntityStatus
{
    [Serializable]
    public abstract class StatusBase
    {
        public string name;
        protected Entity Owner;
        
        public void Activate(Entity entity)
        { Owner = entity; name = GetType().Name; OnActivated(); }

        public void Deactivate(Entity entity)
        { Owner = null; name = ""; OnDeactivated(); }
        
        protected virtual void OnActivated() {}
        protected virtual void OnDeactivated() {}
    }
}