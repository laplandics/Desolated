using System.Collections.Generic;
using EntitySystem;

namespace Utils
{
    public class Systems
    {
        private HashSet<SystemBase> _entitiesSystems = new();

        public Systems()
        {
            var allSystems = R.GetAll<SystemBase>("System");
            foreach (var system in allSystems)
            { system.UnregisterAll(); _entitiesSystems.Add(system); }
        }
    }
}