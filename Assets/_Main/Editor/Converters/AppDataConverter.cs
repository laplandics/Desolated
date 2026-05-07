using Boot;
using Constant;
using Data;
using UnityEngine;

namespace EditorTools.DataConverter
{
    [CreateAssetMenu(fileName = "AppDataConverter", menuName = "Editor/Converters/AppDataConverter")]
    public class AppDataConverter : BaseConverter
    {
        public int fps;
        public int vSync;
        public SceneNames loadScene;
        public CameraTypes firstCameraType;
        
        protected override string Label => nameof(Data.State.App);

        public override DataState GetState()
        {
            var state = new Data.State.App();
            state.FPS = fps;
            state.VSync = vSync;
            state.FirstSceneParams = new SceneParams();
            state.FirstSceneParams.scene = loadScene;
            state.FirstSceneParams.cameraType = firstCameraType;
            return state;
        }
    }
}
