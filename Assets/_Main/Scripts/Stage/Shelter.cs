using Space;
using UIElement;

namespace Stage
{
    public class Shelter : BaseStage
    {
        protected override void OnLoad()
        {
            G.Resolve<UI>().OpenScreen(new GameScreenVm());
        }
    }
}