using System.Collections;
using Data;
using R3;
using Stage;
using UnityEngine;

namespace Boot
{
    public class StageBoot : SceneBoot
    {
        [SerializeField] private BaseStage stage;
        
        public override IEnumerator Boot(Subject<SceneParams> onExit, SceneParams sceneParams)
        {
            var label = sceneParams.scene.ToString();
            yield return G.Resolve<DataProvider>().LoadData<Data.State.Stage>(label);
            var stageData = G.Resolve<DataProvider>().GetProxy<Data.Proxy.Stage>(label);
            stage.BindData(stageData);

            yield return null;
        }
    }
}