using System.Collections.Generic;
using Constant;
using ObservableCollections;
using R3;
using UnityEngine;

namespace Data.State
{
    public class Entity : DataState
    {
        public string ID { get; set; }
        
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        
        public Dictionary<EntityModules, string> Modules { get; set; }
    }
}

namespace Data.Proxy
{
    public class Entity : DataProxy
    {
        public State.Entity State { get; }
        
        public string ID { get; }
        
        public ReactiveProperty<Vector3> Position { get; }
        public ReactiveProperty<Vector3> Rotation { get; }
        
        public ObservableDictionary<EntityModules, string> Modules { get; }
        
        public Entity(DataState origin) : base(origin)
        {
            State = GetState<State.Entity>();
            ID = State.ID;
            
            Position = new ReactiveProperty<Vector3>(State.Position);
            Position.Skip(1).Subscribe(position => State.Position = position);
            
            Rotation = new ReactiveProperty<Vector3>(State.Rotation);
            Rotation.Skip(1).Subscribe(rotation => State.Rotation = rotation);
            
            Modules = new ObservableDictionary<EntityModules, string>();
            foreach (var (mod, state) in State.Modules) Modules.Add(mod, state);
            Modules.ObserveDictionaryRemove().Subscribe(e => State.Modules.Remove(e.Key));
            Modules.ObserveDictionaryAdd().Subscribe(e => State.Modules.Add(e.Key, e.Value));
            Modules.ObserveDictionaryReplace().Subscribe(e => State.Modules[e.Key] = e.NewValue);
        }
    }
}