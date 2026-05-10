using System.Collections.Generic;
using UnityEngine;
using WorldObject;

namespace EntitySystem
{
    public abstract class SystemBase : ScriptableObject
    {
        protected readonly Dictionary<string, Entity> RegisteredEntitiesMap = new();
        
        public void Register(Entity entity) => RegisteredEntitiesMap.TryAdd(entity.state.id, entity);
        public void Unregister(Entity entity) => RegisteredEntitiesMap.Remove(entity.state.id);
        
    }
}