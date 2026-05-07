using Boot;
using Constant;
using R3;

namespace UIElement
{
    public class MainMenuVm : UIElementVm
    {
        public override string BinderKey => "MainMenu";

        private readonly Subject<SceneParams> _onExit;

        public MainMenuVm(Subject<SceneParams> onExit) { _onExit = onExit; }

        public void PlayGame()
        {
            var sceneParams = new SceneParams();
            sceneParams.cameraType = CameraTypes.DefaultCamera;
            sceneParams.scene = SceneNames.StageShelter;
            _onExit.OnNext(sceneParams);
        }
    }
}