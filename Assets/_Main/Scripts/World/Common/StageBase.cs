using System.Collections.Generic;
using System.Linq;
using Config;
using Data.State;
using ObservableCollections;
using R3;
using UnityEngine;
using Utils;

namespace WorldObject
{
    public abstract class StageBase : MonoBehaviour
    {
        protected readonly Dictionary<string, Entity> EntitiesMap = new();
        
        private SceneConfig _sceneConfig;
        private CompositeDisposable _updateEntitiesDisposables = new();
        
        public void BindData(SceneConfig config)
        {
            _sceneConfig = config;
            
            _updateEntitiesDisposables.Add(config.Entities.ObserveAdd().Subscribe(e => BindEntity(e.Value)));
            _updateEntitiesDisposables.Add(config.Entities.ObserveRemove().Subscribe(e => UnbindEntity(e.Value)));
            
            config.Entities.ForEach(BindEntity);

            Main.CurrentPlayerConfig.Value.LastVisitedStageConfig.Value = config;
            OnLoad();
        }

        public void UnbindData()
        {
            _sceneConfig.Entities.ForEach(UnbindEntity);
            _updateEntitiesDisposables?.Dispose();
            OnUnload();
        }

        private void BindEntity(EntityState eState)
        {
            var entities = FindObjectsByType<Entity>();
            var entity = entities.FirstOrDefault(e => e.state.id == eState.id);
            if (entity == null) entity = SpawnNewEntity(eState);
            entity.Bind(eState);
            EntitiesMap.TryAdd(eState.id, entity);
            
            G.Resolve<Entities>().Storage.Add(entity);
        }

        private Entity SpawnNewEntity(EntityState eState)
        {
            G.Resolve<Entities>().Builder
                .New(eState)
                .BuildSilent(out var entity);
            return entity;
        }

        private void UnbindEntity(EntityState eState)
        {
            var id = eState.id;
            if (!EntitiesMap.Remove(id, out var entity)) return;
            entity.Unbind();
            
            G.Resolve<Entities>().Storage.Remove(entity);
        }
        
        protected virtual void OnLoad() { }
        protected virtual void OnUnload() { }
    }
}