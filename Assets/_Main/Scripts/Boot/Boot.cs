using System;
using System.Collections;
using Constant;
using Data;
using R3;
using Space;
using UIElement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Object = UnityEngine.Object;

namespace Boot
{
    public class Boot
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap() => _ = new Boot();

        private Boot()
        {
            G.Register(new UI());
            G.Register(new Inputs());
            G.Register(new Scenes());
            G.Register(new Managers());
            G.Register(new Coroutines());
            G.Register(new DataProvider());
            
            var sceneName = SceneManager.GetActiveScene().name;
            if (!Enum.TryParse<SceneNames>(sceneName, out _)) return;
            
            G.Resolve<Coroutines>().Start(LoadProject());
        }

        private IEnumerator LoadProject()
        {
            G.Resolve<UI>().OpenScreen(new LoadingScreenVm());
            yield return G.Resolve<Scenes>().ToBoot();

            const string appStateLabel = nameof(Data.State.App);
            yield return G.Resolve<DataProvider>().LoadData<Data.State.App>(appStateLabel);
            var appProxy = G.Resolve<DataProvider>().GetProxy<Data.Proxy.App>(appStateLabel);
            
            QualitySettings.vSyncCount = appProxy.VSync.Value;
            Application.targetFrameRate = appProxy.FPS.Value;
            
            var firstSceneParams = appProxy.FirstSceneParams;
            G.Resolve<Coroutines>().Start(LoadScene(firstSceneParams));
        }

        private IEnumerator LoadScene(SceneParams loadParams)
        {
            yield return new WaitForSeconds(2f);
            
            var sceneName = loadParams.scene.ToString();
            yield return G.Resolve<Scenes>().ToScene(sceneName);
            
            var sceneBoot = Object.FindAnyObjectByType<SceneBoot>();
            if (sceneBoot == null) { Debug.LogWarning(NoSceneBootWarning); yield break; }
            
            var onExit = new Subject<SceneParams>();
            onExit.Subscribe(sp => G.Resolve<Coroutines>().Start(UnloadScene(sp)));
            
            G.Resolve<Coroutines>().Start(sceneBoot.Boot(onExit, loadParams));
        }
        
        private IEnumerator UnloadScene(SceneParams unloadParams)
        {
            G.Resolve<UI>().OpenScreen(new LoadingScreenVm());
            yield return G.Resolve<Scenes>().ToBoot();
            G.Resolve<Coroutines>().Start(LoadScene(unloadParams));
        }

        private string NoSceneBootWarning => $"Couldn't find SceneBoot on this scene: {SceneManager.GetActiveScene().name}";
    }
}

namespace Boot
{
    public abstract class SceneBoot : MonoBehaviour
    { public abstract IEnumerator Boot(Subject<SceneParams> onExit, SceneParams sceneParams); }
}