using System.Collections.Generic;
using UnityEngine;
using WorldObject;

namespace EntitySystem
{
    public abstract class SystemBase : ScriptableObject
    {
        protected readonly Dictionary<string, Entity> RegisteredEntitiesMap = new();
        
        public void Register(Entity entity)
        {
            RegisteredEntitiesMap.TryAdd(entity.state.id, entity);
            OnEntityRegistered(entity);
        }

        public void Unregister(Entity entity)
        {
            RegisteredEntitiesMap.Remove(entity.state.id);
            OnEntityUnregistered(entity);
        }

        public void UnregisterAll() => RegisteredEntitiesMap.Clear();

        protected virtual void OnEntityRegistered(Entity entity) { }
        protected virtual void OnEntityUnregistered(Entity entity) { }
    }
}