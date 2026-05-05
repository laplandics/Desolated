using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Stage
{
    public abstract class BaseStage : MonoBehaviour
    {
        protected List<Entity.Entity> Entities = new();
        
        public void BindData(Data.Proxy.Stage stage)
        {
            var entities = FindObjectsByType<Entity.Entity>(FindObjectsInactive.Include);
            foreach (var entityProxy in stage.Entities)
            {
                var id = entityProxy.ID;
                var entity = entities.FirstOrDefault(e => e.id == id);
                if (entity == null) continue;
                entity.BindData(entityProxy);
                Entities.Add(entity);
            }
            
            OnLoad();
        }

        protected virtual void OnLoad() {}
        protected virtual void OnUnload() {}
    }
}