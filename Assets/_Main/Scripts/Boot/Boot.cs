using System.Collections;
using Config;
using DebugTools;
using R3;
using Space;
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
            G.Register(new Systems());
            G.Register(new Debugger());
            G.Register(new Entities());
            G.Register(new Services());
            G.Register(new Coroutines());
            
            G.Resolve<Coroutines>().Start(LoadProject());
        }

        private IEnumerator LoadProject()
        {
            yield return Resources.UnloadUnusedAssets();
            
            yield return G.Resolve<Services>().OnProjectBeginLoad();
            
            yield return G.Resolve<Scenes>().ToBoot();
            
            var appData = R.AppConfig;
            QualitySettings.vSyncCount = appData.VSync.Value;
            Application.targetFrameRate = appData.FPS.Value;

            Main.CurrentPlayerConfig.Value = R.PlayerConfig;
            
            var menuConfig = R.SceneConfigMenu;
            yield return G.Resolve<Services>().OnProjectEndLoad();
            G.Resolve<Coroutines>().Start(LoadScene(menuConfig));
        }

        private IEnumerator LoadScene(SceneConfig sceneConfig)
        {
            Main.CurrentSceneConfig.Value = sceneConfig;
            yield return G.Resolve<Services>().OnSceneBeginLoad();
            
            var sceneName = sceneConfig.SceneName;
            yield return G.Resolve<Scenes>().ToScene(sceneName);
            
            var sceneBoot = Object.FindAnyObjectByType<SceneBoot>();
            if (sceneBoot == null) { Debug.LogWarning(NoSceneBootWarning); yield break; }
            
            var onExit = new Subject<SceneConfig>();
            onExit.Subscribe(config => G.Resolve<Coroutines>().Start(UnloadScene(config)));
            sceneConfig.ExitSubject = onExit;
            
            yield return G.Resolve<Services>().OnSceneEndLoad();
            yield return null;
            
            yield return G.Resolve<Services>().OnSceneBeginBoot();
            yield return G.Resolve<Coroutines>().Start(sceneBoot.Boot(sceneConfig));
            yield return G.Resolve<Services>().OnSceneEndBoot();
        }
        
        private IEnumerator UnloadScene(SceneConfig sceneConfig)
        {
            yield return Resources.UnloadUnusedAssets();
            
            yield return G.Resolve<Services>().OnSceneBeginUnload();
            
            yield return G.Resolve<Scenes>().ToBoot();
            
            yield return G.Resolve<Services>().OnSceneEndUnload();
            G.Resolve<Coroutines>().Start(LoadScene(sceneConfig));
        }

        private string NoSceneBootWarning => $"Couldn't find SceneBoot on this scene: {SceneManager.GetActiveScene().name}";
    }
}