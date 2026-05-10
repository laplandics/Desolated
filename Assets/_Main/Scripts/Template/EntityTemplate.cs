using System;
using System.Collections.Generic;
using System.Linq;
using Data.State;
using EntityStatus;
using EntitySystem;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace Template
{
    [CreateAssetMenu(fileName = "EntityTemplate_[ENTITY_KEY]", menuName = "Template/Entity")]
    public class EntityTemplate : ScriptableObject
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int health;
        [SerializeField][Range(0.0f, 30.0f)] private float speed;
        
        [Space(10)]
        [SerializeField] private List<SystemBase> systems;
        [SerializeField] private List<StatusChoice> statuses;
        
        [Space(10)]
        [SerializeField][ReadOnly] private EntityState entityState;

        public EntityState State => FormState();

        private EntityState FormState()
        {
            var newState = new EntityState();
            var prefix = ": " + Guid.NewGuid();
            newState.id = entityState.key + prefix;
            newState.key = entityState.key;
            newState.speed = entityState.speed;
            newState.health = entityState.health;
            newState.prefabPath = entityState.prefabPath;
            newState.systems = entityState.systems.ToList();
            newState.statuses = entityState.statuses.ToList();
            return newState;
        }

        [Button]
        private void SetValues()
        {
            #if UNITY_EDITOR
            entityState.key = name.Split('_')[1];
            entityState.health = health;
            entityState.speed = speed;
            
            if (prefab != null)
            {
                var fullPath = AssetDatabase.GetAssetPath(prefab);
                var path = Tools.PathHelper.AssetPathToResourcePath(fullPath);
                entityState.prefabPath = path;
            }
            
            entityState.systems.Clear();
            if (systems is { Count: > 0 })
            {
                foreach (var system in systems)
                {
                    var fullPath = AssetDatabase.GetAssetPath(system);
                    var path = Tools.PathHelper.AssetPathToResourcePath(fullPath);
                    entityState.systems.Add(path);
                }
            }

            entityState.statuses.Clear();
            if (statuses is { Count: > 0 })
            {
                foreach (var choice in statuses)
                { entityState.statuses.Add(choice.status); }
            }
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            #endif
        }
    }

    [Serializable]
    public class StatusChoice
    {
        [Dropdown(nameof(Statuses))] public string status;
        private List<string> Statuses => Tools.ReflectionTool
            .GetSubclasses<StatusBase>()
            .Select(t => t.Name)
            .ToList();
    }
}