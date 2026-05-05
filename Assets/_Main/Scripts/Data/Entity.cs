using R3;
using UnityEngine;

namespace Data.State
{
    public class Entity : DataState
    {
        public string ID { get; set; }
        
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
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
        
        public Entity(DataState origin) : base(origin)
        {
            State = GetState<State.Entity>();
            ID = State.ID;
            
            Position = new ReactiveProperty<Vector3>(State.Position);
            Position.Skip(1).Subscribe(position => State.Position = position);
            
            Rotation = new ReactiveProperty<Vector3>(State.Rotation);
            Rotation.Skip(1).Subscribe(rotation => State.Rotation = rotation);
        }
    }
}