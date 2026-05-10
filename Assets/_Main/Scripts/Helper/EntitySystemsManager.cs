using EntitySystem;
using WorldObject;

namespace Helper.EntityManagement
{
    public class EntitySystemsManager
    {
        private readonly Entity _owner;

        public EntitySystemsManager(Entity owner)
        {
            _owner = owner;
        }
        
        public void AddSystem(string systemPath)
        {
            var system = R.Get<SystemBase>(systemPath);
            if (_owner.systems.Contains(system)) return;
            system.Register(_owner);
            _owner.systems.Add(system);

            if (_owner.state.systems.Contains(systemPath)) return;
            _owner.state.systems.Add(systemPath);
        }

        public void RemoveSystem(string systemPath)
        {
            var system = R.Get<SystemBase>(systemPath);
            if (!_owner.systems.Contains(system)) return;
            system.Unregister(_owner);
            _owner.systems.Remove(system);
            
            if (!_owner.state.systems.Contains(systemPath)) return;
            _owner.state.systems.Remove(systemPath);
        }
    }
}