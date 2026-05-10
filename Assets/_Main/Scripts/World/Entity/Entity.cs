using System;
using System.Collections.Generic;
using Data.State;
using EntityStatus;
using EntitySystem;
using Helper.EntityManagement;
using NaughtyAttributes;
using Template;
using UnityEngine;

namespace WorldObject
{
    public class Entity : MonoBehaviour
    {
        [Header("Editor Only")]
        public EntityTemplate template;
        
        [Header("Runtime Only")]
        [ReadOnly] public List<SystemBase> systems = new();
        [SerializeReference][ReadOnly] public List<StatusBase> statuses = new();
        [ReadOnly] public GameObject visual;
        
        [Header("State")]
        [ReadOnly] public EntityState state;

        public EntityStatusesManager StatusesManager { get; private set; }
        public EntitySystemsManager SystemsManager { get; private set; }
        public EntityTransformsManager TransformsManager { get; private set; }
        
        public void Bind(EntityState setState)
        {
            state = setState;
            
            StatusesManager = new EntityStatusesManager(this);
            state.statuses.ForEach(StatusesManager.AddStatus);
            
            SystemsManager = new EntitySystemsManager(this);
            state.systems.ForEach(SystemsManager.AddSystem);
            
            TransformsManager = new EntityTransformsManager(this);
            TransformsManager.ApplyPosition(state.currentPosition);
            TransformsManager.ApplyRotation(state.currentRotation);
            
            UpdateVisual(state.prefabPath);
            
            state.initialized = true;
        }
        
        public void Unbind()
        {
            systems.ForEach(system => system.Unregister(this));
            systems.Clear();
            
            statuses.ForEach(status => status.Deactivate(this));
            statuses.Clear();
        }

        private void UpdateVisual(string prefabPath)
        {
            if (string.IsNullOrEmpty(prefabPath)) return;
            if (visual != null) Destroy(visual);
            var prefab = R.Get<GameObject>(prefabPath);
            visual = Instantiate(prefab, transform, false);
            visual.name = "Visual";
        }
        
        public EntityState GenerateState()
        {
            gameObject.layer = LayerMask.NameToLayer("Entity");
            
            if (template == null && state == null)
            { throw new Exception($"{name} has no state or template to initialize from"); }
            
            if (state is not { initialized: true })
            { state = template.State; state.initialized = true; }
            
            state.currentPosition = transform.position;
            state.currentRotation = transform.eulerAngles;
            return state;
        }
    }
}
