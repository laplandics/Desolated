using Boot;
using R3;

namespace UIElement
{
    public class MainMenuVm : UIElementVm
    {
        public override string BinderKey => "MainMenu";

        private Subject<SceneParams> _onExit;
    }
}