using System;
using Config;
using UnityEngine;

namespace Data.Config
{
    [Serializable]
    public class AppData : ConfigData
    {
        public int fps;
        [Range(0, 2)] public int vSync;
    }
}