using System.Collections;
using R3;
using Space;
using UIElement;

namespace Boot
{
    public class MenuBoot : SceneBoot
    {
        public override IEnumerator Boot(Subject<SceneParams> onExit, SceneParams sceneParams)
        {
            G.Resolve<UI>().OpenScreen(new MainMenuVm());
            
            yield return null;
        }
    }
}