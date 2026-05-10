using System;
using System.Collections;
using R3;
using Space;
using UIElement;

namespace Service
{
    public class ScreenService :
        ServiceBase,
        IOnProjectBeginLoadService,
        IOnSceneBeginUnloadService,
        IOnSceneBeginBootService
    {
        private IDisposable _onSceneScreenChanged;
        
        public IEnumerator OnProjectBeginLoad()
        {
            _onSceneScreenChanged?.Dispose();
            G.Resolve<UI>().OpenScreen(new LoadingScreenVm());
            yield return null;
        }

        public IEnumerator OnSceneBeginUnload()
        {
            _onSceneScreenChanged?.Dispose();
            G.Resolve<UI>().OpenScreen(new LoadingScreenVm());
            yield return null;
        }

        public IEnumerator OnSceneBeginBoot()
        {
            var sceneScreenRProperty = Main.CurrentSceneConfig.Value.ScreenVm;
            _onSceneScreenChanged = sceneScreenRProperty.Subscribe(ChangeSceneScreen);
            yield return null;
        }

        private void ChangeSceneScreen(UIElementVm screenVm) => G.Resolve<UI>().OpenScreen(screenVm);
    }
}