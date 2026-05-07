using System;
using System.Collections.Generic;
using Constant;
using Data;
using Entity.Ability;

namespace EditorTools.DataConverter
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

            var entityAbilities = new Dictionary<EntityModules, string>();
            foreach (var ability in entity.GetComponents<BaseModule>())
            {
                var key = ability.ModuleType;
                var value = ability.ParseToString();
                entityAbilities.Add(key, value);
            }

            state.Modules = entityAbilities;
            
            return state;
        }
    }
}