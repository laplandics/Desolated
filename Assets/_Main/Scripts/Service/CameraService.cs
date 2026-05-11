using System;
using System.Collections;
using Config;
using R3;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Service
{
    public class CameraService : ServiceBase, IOnSceneBeginLoadService, IOnSceneEndLoadService
    {
        private IDisposable _onSceneCameraChanged;
        
        public IEnumerator OnSceneBeginLoad()
        {
            _onSceneCameraChanged?.Dispose();
            InstantiateCamera(R.CameraConfigDefault);
            yield return null;
        }

        public IEnumerator OnSceneEndLoad()
        {
            var cameraConfigRProperty = Main.CurrentSceneConfig.Value.SceneCamera;
            _onSceneCameraChanged = cameraConfigRProperty.Subscribe(InstantiateCamera);
            yield return null;
        }

        private void InstantiateCamera(CameraConfig cameraConfig)
        {
            var camPrefab = R.Get<GameObject>(cameraConfig.PrefabPath);
            var cam = Object.Instantiate(camPrefab);
            
            cam.name = "MainCamera";
            
            var current = cam.GetComponent<Camera>();
            if (current == null) current = cam.GetComponentInParent<Camera>();
            
            Main.CurrentMainCamera.Value = current;
        }
    }
}