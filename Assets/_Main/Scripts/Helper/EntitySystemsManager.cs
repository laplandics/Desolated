using System.Collections.Generic;
using EntitySystem;
using WorldObject;

namespace Helper.EntityManagement
{
    public class EntitySystemsManager
    {
        private readonly Entity _owner;
        public Dictionary<string, SystemBase> SystemsMap { get; }
        
        public EntitySystemsManager(Entity owner)
        {
            _owner = owner;
            SystemsMap = new Dictionary<string, SystemBase>();
        }
        
        public void AddSystem(string systemPath)
        {
            var system = R.Get<SystemBase>(systemPath);
            if (_owner.systems.Contains(system)) return;
            system.Register(_owner);
            _owner.systems.Add(system);
            
            SystemsMap.Add(systemPath, system);
            if (_owner.state.systems.Contains(systemPath)) return;
            _owner.state.systems.Add(systemPath);
        }

        public void RemoveSystem(string systemPath)
        {
            var system = R.Get<SystemBase>(systemPath);
            if (!_owner.systems.Contains(system)) return;
            system.Unregister(_owner);
            _owner.systems.Remove(system);
            
            SystemsMap.Remove(systemPath);
            if (!_owner.state.systems.Contains(systemPath)) return;
            _owner.state.systems.Remove(systemPath);
        }
    }
}