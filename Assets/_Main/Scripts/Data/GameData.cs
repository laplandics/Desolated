using System;
using Config;
using Constant;

namespace Data.Config
{
    [Serializable]
    public class GameData : ConfigData
    {
        public EncounterStates encounterState;
    }
}