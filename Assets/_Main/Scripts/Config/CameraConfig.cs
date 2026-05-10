using Data.Config;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "CameraConfig_[CAMERA_NAME]", menuName = "Config/Camera")]
    public class CameraConfig : ConfigBase<CameraData>
    {
        public string PrefabPath => data.prefabPath;
        public Vector3 InitialPosition => data.initialPosition;
        public Vector3 InitialRotation => data.initialRotation;
        
        protected override void OnRefresh()
        { data.prefabPath = "Prefab/World/Camera/" + name.Split('_')[1] + "Camera"; }
    }
}