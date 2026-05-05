using System.Collections.Generic;
using ObservableCollections;
using R3;

namespace Data.State
{
    public class Stage : DataState
    {
        public List<Entity> Entities { get; set; }
    }
}

namespace Data.Proxy
{
    public class Stage : DataProxy
    {
        public State.Stage State { get; }
        
        public ObservableList<Entity> Entities { get; }
        
        public Stage(DataState origin) : base(origin)
        {
            State = GetState<State.Stage>();
            
            Entities = new ObservableList<Entity>();
            State.Entities.ForEach(entity => Entities.Add(new Entity(entity)));
            Entities.ObserveAdd().Subscribe(e => State.Entities.Add(e.Value.State));
            Entities.ObserveRemove().Subscribe(e => State.Entities.Remove(e.Value.State));
        }
    }
}