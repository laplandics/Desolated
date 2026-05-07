using System.Collections;
using Constant;
using UnityEngine;

namespace Service
{
    public class CameraService : BaseService, IOnSceneBeginLoadService, IOnSceneBeginBootService
    {
        public Camera CurrentMain { get; private set; }
        
        public IEnumerator OnSceneBeginLoad()
        {
            InstantiateCamera(CameraTypes.DefaultCamera);
            yield return null;
        }

        public IEnumerator OnSceneBeginBoot()
        {
            var sceneParams = Main.CurrentSceneParams;
            InstantiateCamera(sceneParams.Value.cameraType);
            yield return null;
        }

        private void InstantiateCamera(CameraTypes cameraType)
        {
            var camPrefab = Resources.Load<GameObject>($"Prefab/World/{cameraType.ToString()}");
            var cam = Object.Instantiate(camPrefab);
            cam.name = "MainCamera";
            cam.transform.position = Vector3.zero;
            cam.transform.rotation = Quaternion.identity;
            CurrentMain = cam.GetComponent<Camera>();
            Main.CurrentMainCamera.Value = CurrentMain;
        }
    }
}