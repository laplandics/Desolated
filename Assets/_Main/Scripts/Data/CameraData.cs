using System;
using Config;
using UnityEngine;

namespace Data.Config
{
    [Serializable]
    public class CameraData : ConfigData
    {
        public string prefabPath;
        public Vector3 initialPosition;
        public Vector3 initialRotation;
    }
}