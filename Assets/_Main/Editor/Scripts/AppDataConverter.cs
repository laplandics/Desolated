using Boot;
using Constant;
using Data;
using UnityEngine;

namespace EditorTools
{
    [CreateAssetMenu(fileName = "AppDataConverter", menuName = "Editor/Converters/AppDataConverter")]
    public class AppDataConverter : BaseConverter
    {
        public int fps;
        public int vSync;
        public SceneNames loadScene;
        
        protected override string Label => nameof(Data.State.App);

        public override DataState GetState()
        {
            var state = new Data.State.App();
            state.FPS = fps;
            state.VSync = vSync;
            state.FirstSceneParams = new SceneParams();
            state.FirstSceneParams.scene = loadScene;
            return state;
        }
    }
}
