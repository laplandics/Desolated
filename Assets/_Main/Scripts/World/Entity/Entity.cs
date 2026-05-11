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
            SystemsManager = new EntitySystemsManager(this);
            StatusesManager = new EntityStatusesManager(this);
            TransformsManager = new EntityTransformsManager(this);
            
            state = setState;
            
            state.statuses.ForEach(StatusesManager.AddStatus);
            state.systems.ForEach(SystemsManager.AddSystem);
            TransformsManager.ApplyPosition(state.currentPosition);
            TransformsManager.ApplyRotation(state.currentRotation);
            
            UpdateVisual(state.prefabPath);
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

            if (template == null)
            { throw new Exception($"{name} has no template to initialize from"); }

            state = template.State;
            
            state.currentPosition = transform.position;
            state.currentRotation = transform.eulerAngles;
            return state;
        }
    }
}
