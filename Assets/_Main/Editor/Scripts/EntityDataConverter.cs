using System;
using Data;

namespace EditorTools
{
    public class EntityDataConverter : BaseConverter
    {
        public Entity.Entity entity;
        protected override string Label => "Entity";

        public override DataState GetState()
        {
            entity.id = Guid.NewGuid().ToString();
            
            var state = new Data.State.Entity();
            state.ID = entity.id;
            state.Position = entity.transform.position;
            state.Rotation = entity.transform.eulerAngles;
            
            return state;
        }
    }
}