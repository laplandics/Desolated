using System;
using Config;
using Data.State;
using MyAttributes;
using UnityEngine;

namespace Data.Config
{
    [Serializable]
    public class PlayerData : ConfigData
    {
        public string lastVisitedStageConfigPath;
        [Space]
        [MyReadOnly] public EntityState playerEntityState;
    }
}