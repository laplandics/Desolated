using System.Collections.Generic;
using Data.State;
using ObservableCollections;
using UnityEngine;
using WorldObject;

namespace Utils
{
    public class Entities
    {
        public EntityStorage Storage { get; private set; } = new();
        public EntityBuilder Builder => new();
        
        public class EntityStorage
        {
            public IReadOnlyObservableDictionary<string, Entity> ObserveAll => _observeAll;
            private readonly ObservableDictionary<string, Entity> _observeAll = new();
            
            public IReadOnlyDictionary<string, Entity> All => _all;
            private readonly Dictionary<string, Entity> _all = new();
            
            public void Add(Entity entity)
            {
                if (!_all.TryAdd(entity.state.id, entity)) return;
                _observeAll.Add(entity.state.id, entity);
            }

            public void Remove(Entity entity)
            {
                if (!_all.Remove(entity.state.id)) return;
                _observeAll.Remove(entity.state.id);
            }
        }

        public class EntityBuilder
        {
            private EntityState _buildingState;
            
            public EntityBuilder New(EntityState state)
            { _buildingState = state; return this; }

            public EntityBuilder AtPosition(Vector3 position)
            { _buildingState.currentPosition = position; return this; }

            public EntityBuilder WithRotation(Vector3 angles)
            { _buildingState.currentRotation = angles; return this; }
            
            public EntityBuilder WithTargetPosition(Vector3 position)
            { _buildingState.targetPosition = position; return this; }

            public EntityBuilder WithTargetRotation(Vector3 angles)
            { _buildingState.targetRotation = angles; return this; }

            public EntityBuilder AddSystem(string systemPath)
            {
                if (!_buildingState.systems.Contains(systemPath))
                { _buildingState.systems.Add(systemPath); }
                return this;
            }

            public EntityBuilder AddStatus(string status)
            {
                if (!_buildingState.statuses.Contains(status))
                    _buildingState.statuses.Add(status);
                return this;
            }
            
            public void Build(out Entity entity)
            {
                var key = _buildingState.key;
                var entityObject = new GameObject(key);
                var entityScript = entityObject.AddComponent<Entity>();
                entityScript.state = _buildingState;
                
                Main.CurrentSceneConfig.Value.Entities.Add(_buildingState);
                entity = entityScript;
            }
            
            public void DestroyEntity(Entity entity)
            {
                if (entity == null) return;
                Main.CurrentSceneConfig.Value.Entities.Remove(entity.state);
                Object.Destroy(entity.gameObject);
            }
        }
    }
}