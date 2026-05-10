using System;
using System.Linq;
using EntityStatus;
using WorldObject;

namespace Helper.EntityManagement
{
    public class EntityStatusesManager
    {
        private readonly Entity _owner;

        public EntityStatusesManager(Entity owner)
        {
            _owner = owner;
        }
        
        public void AddStatus(string statusName)
        {
            var stat = Tools.ReflectionTool.GetTypeByName(statusName);
            if (Activator.CreateInstance(stat) is not StatusBase baseStat) return;
            _owner.statuses.Add(baseStat);
            baseStat.Activate(_owner);
            
            if (_owner.state.statuses.Contains(statusName)) return;
            _owner.state.statuses.Add(statusName);
        }
        
        public void RemoveStatus(string statusName)
        {
            var statusBase = _owner.statuses.FirstOrDefault(status =>
                status.GetType().Name == statusName);
            if (statusBase == null) return;
            _owner.statuses.Remove(statusBase);
            statusBase.Deactivate(_owner);
            
            if (!_owner.state.statuses.Contains(statusName)) return;
            _owner.state.statuses.Remove(statusName);
        }
    }
}