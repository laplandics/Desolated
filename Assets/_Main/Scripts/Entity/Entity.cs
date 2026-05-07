using System.Collections.Generic;
using System.Linq;
using Constant;
using Entity.Ability;
using NaughtyAttributes;
using ObservableCollections;
using R3;
using UnityEngine;

namespace Entity
{
    public abstract class Entity : MonoBehaviour
    {
        [ReadOnly] public string id;
        
        private readonly ObservableDictionary<EntityModules, BaseModule> _modulesMap = new();
        public IReadOnlyObservableDictionary<EntityModules, BaseModule> Modules => _modulesMap;
        
        public void BindData(Data.Proxy.Entity entity)
        {
            transform.position = entity.Position.Value;
            transform.rotation = Quaternion.Euler(entity.Rotation.Value);
            entity.Modules.ObserveDictionaryRemove().Subscribe(e => RemoveModule(e.Key));
            entity.Modules.ObserveDictionaryAdd().Subscribe(e => AddModule(e.Key, e.Value));
            
            var defaultModules = gameObject.GetComponents<BaseModule>();
            foreach (var module in defaultModules) _modulesMap.Add(module.ModuleType, module);
            foreach (var module in entity.Modules) AddModule(module.Key, module.Value);

            CheckModules();
            OnReady();
        }

        public void AddModule(EntityModules module, string state)
        {
            BaseModule newModule = null;
            try
            {
                if (_modulesMap.TryGetValue(module, out newModule)) return;
                var moduleTypes = Tools.ReflectionTool.GetSubclasses<BaseModule>();
                var moduleType = moduleTypes.FirstOrDefault(a => a.Name == module.ToString());
                if (moduleType == null) return;
                newModule = gameObject.AddComponent(moduleType) as BaseModule;
            }
            finally
            {
                if (newModule != null)
                {
                    newModule.Owner = this;
                    newModule.ParseToState(state);
                    newModule.OnAdd();
                    _modulesMap.TryAdd(module, newModule);
                }
            }
        }

        public void RemoveModule(EntityModules module)
        {
            if (!_modulesMap.TryGetValue(module, out var moduleInstance)) return;
            moduleInstance.OnRemove();
            _modulesMap.Remove(module);
        }

        public bool TryGetModule<T>(EntityModules module, out T requestedModule) where T : BaseModule
        {
            requestedModule = null;
            if (!Modules.TryGetValue(module, out var moduleInstance)) return false;
            if (moduleInstance is not T requested) return false;
            requestedModule = requested;
            return true;
        }

        protected virtual void OnReady() {}

        private void CheckModules()
        {
            foreach (var module in _modulesMap)
            {
                if (module.Value.Owner == this) continue;
                Debug.LogWarning($"Module {module.Key} was not initialized. " +
                                 $"Entity: {gameObject.name} ({id})");
            }
        }
    }
}
