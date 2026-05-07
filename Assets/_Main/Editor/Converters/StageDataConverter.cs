using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EditorTools.DataConverter
{
    [CreateAssetMenu(fileName = "StageDataConverter", menuName = "Editor/Converters/StageDataConverter")]
    public class StageDataConverter : BaseConverter
    {
        protected override string Label => SceneManager.GetActiveScene().name;

        public override DataState GetState()
        {
            var state = new Data.State.Stage();
            
            var sceneEntities = FindObjectsByType<Entity.Entity>(FindObjectsInactive.Include);
            var entityConverter = CreateInstance<EntityDataConverter>();
            
            var entitiesStates = new List<Data.State.Entity>();
            foreach (var entity in sceneEntities)
            {
                entityConverter.entity = entity;
                var entityState = entityConverter.GetState();
                entitiesStates.Add(entityState as Data.State.Entity);
            }
            
            state.Entities = entitiesStates;
            return state;
        }
    }
}