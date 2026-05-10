using System;
using System.Collections.Generic;
using Config;
using Data.State;
using MyAttributes;
using UnityEngine;

namespace Data.Config
{
    [Serializable]
    public class SceneData : ConfigData
    {
        public string sceneScreen;
        public string sceneCameraConfigPath;
        public ScenePlayerInfo playerInfo;
        
        [Space][MyReadOnly] public List<EntityState> entities;
    }

    [Serializable]
    public class ScenePlayerInfo
    {
        public Vector3 initialPosition;
        public Vector3 initialRotation;
    }
}