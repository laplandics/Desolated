using Constant;
using Newtonsoft.Json;
using UnityEngine;

namespace Entity.Ability
{
    public abstract class BaseModule : MonoBehaviour
    {
        public Entity Owner { get; set; }
        public abstract EntityModules ModuleType { get; }
        
        public virtual void OnAdd() { }
        public virtual void OnRemove() { }

        public abstract void ParseToState(string stateString);
        public abstract string ParseToString();
        
        protected T GetState<T>(string state) => JsonConvert.DeserializeObject<T>(state);
        protected string GetState(object state) => JsonConvert.SerializeObject(state);
    }
}